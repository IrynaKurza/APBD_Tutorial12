using Microsoft.AspNetCore.Mvc;
using TripManagementApi.DTOs;
using TripManagementApi.Services;

namespace TripManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripManagementService _tripManagementService;

        public TripsController(ITripManagementService tripManagementService)
        {
            _tripManagementService = tripManagementService;
        }

        [HttpGet]
        public async Task<ActionResult<TripResponseDto>> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _tripManagementService.GetTripsAsync(page, pageSize);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<ActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientRegistrationDto clientDto)
        {
            try
            {
                var result = await _tripManagementService.AssignClientToTripAsync(idTrip, clientDto);
                return Ok(new { Message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("PESEL"))
                {
                    return Conflict(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
    }
}
