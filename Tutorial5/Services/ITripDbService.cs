using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface ITripDbService
{
    Task<TripListResponseDto> GetTrips(int page, int pageSize);
    Task AssignClientToTrip(int tripId, AssignClientToTripDto dto);
}