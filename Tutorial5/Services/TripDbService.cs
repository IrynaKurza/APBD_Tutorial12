using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class TripDbService : ITripDbService
{
    private readonly ApbdContext _context;
    
    public TripDbService(ApbdContext context)
    {
        _context = context;
    }
    
    public async Task<TripListResponseDto> GetTrips(int page, int pageSize)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips.Select(t =>
            new TripWithDetailsDto {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct =>
                    new CountryDto {
                        Name = ct.Country.Name
                    }).ToList(),
                Clients = t.ClientTrips.Select(ct =>
                    new ClientDto {
                        FirstName = ct.Client.FirstName,
                        LastName = ct.Client.LastName
                    }).ToList()
            })
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new TripListResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };
    }

    public async Task AssignClientToTrip(int tripId, AssignClientToTripDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
    
        try
        {
            // 1. Check trip exists and DateFrom is in future
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) 
                throw new ArgumentException("Trip not found");
            
            if (trip.DateFrom <= DateTime.Now)
                throw new InvalidOperationException("Cannot register for a trip that has already occurred");

            // 2. Check if client with PESEL already exists - if so, return error
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);

            if (existingClient != null)
                throw new InvalidOperationException("Client with this PESEL already exists");

            // 3. Create new client (always, since we verified PESEL doesn't exist)
            var newClient = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };
            
            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            // 4. Create client-trip assignment
            var clientTrip = new ClientTrip
            {
                IdClient = newClient.IdClient,
                IdTrip = tripId,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}