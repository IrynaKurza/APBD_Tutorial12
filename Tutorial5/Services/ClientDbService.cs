using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;

namespace Tutorial5.Services;

public class ClientDbService : IClientDbService
{
    private readonly ApbdContext _context;
    
    public ClientDbService(ApbdContext context)
    {
        _context = context;
    }
    
    public async Task DeleteClient(int clientId)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == clientId);

        if (client == null)
            throw new ArgumentException("Client not found");

        if (client.ClientTrips.Any())
            throw new InvalidOperationException("Cannot delete client with assigned trips");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}