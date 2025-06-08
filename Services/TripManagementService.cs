using Microsoft.EntityFrameworkCore;
using TripManagementApi.Data;
using TripManagementApi.DTOs;
using TripManagementApi.Models;

namespace TripManagementApi.Services
{
    public class TripManagementService : ITripManagementService
    {
        private readonly ApbdContext _context;

        public TripManagementService(ApbdContext context)
        {
            _context = context;
        }

        public async Task<TripResponseDto> GetTripsAsync(int page, int pageSize)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);
            
            var trips = await _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                    .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new CountryDto
                    {
                        Name = c.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                })
                .ToListAsync();

            return new TripResponseDto
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = totalPages,
                Trips = trips
            };
        }

        public async Task<string> AssignClientToTripAsync(int idTrip, ClientRegistrationDto clientDto)
        {
            // check if client with given PESEL already exists
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);

            if (existingClient != null)
            {
                throw new InvalidOperationException("Client with given PESEL already exists");
            }

            // check if trip exists
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null)
            {
                throw new KeyNotFoundException("Trip not found");
            }

            // check if DateFrom is in the future
            if (trip.DateFrom <= DateTime.Now)
            {
                throw new InvalidOperationException("Cannot register for a trip that has already occurred");
            }

            // create new client
            var newClient = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            // create client-trip relationship
            var clientTrip = new ClientTrip
            {
                IdClient = newClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return "Client successfully registered for the trip";
        }

        public async Task<string> DeleteClientAsync(int idClient)
        {
            // check if client exists
            var client = await _context.Clients.FindAsync(idClient);
            if (client == null)
            {
                throw new KeyNotFoundException("Client not found");
            }

            // check if client has any assigned trips
            var hasTrips = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == idClient);

            if (hasTrips)
            {
                throw new InvalidOperationException("Cannot delete client with assigned trips");
            }

            // delete the client
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return "Client successfully deleted";
        }
    }
}