using TripManagementApi.DTOs;

namespace TripManagementApi.Services
{
    public interface ITripManagementService
    {
        Task<TripResponseDto> GetTripsAsync(int page, int pageSize);
        Task<string> AssignClientToTripAsync(int idTrip, ClientRegistrationDto clientDto);
        Task<string> DeleteClientAsync(int idClient);
    }
}