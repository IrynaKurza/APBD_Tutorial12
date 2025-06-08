using Microsoft.AspNetCore.Mvc;
using TripManagementApi.Services;

namespace TripManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ITripManagementService _tripManagementService;

        public ClientsController(ITripManagementService tripManagementService)
        {
            _tripManagementService = tripManagementService;
        }

        [HttpDelete("{idClient}")]
        public async Task<ActionResult> DeleteClient(int idClient)
        {
            try
            {
                var result = await _tripManagementService.DeleteClientAsync(idClient);
                return Ok(new { Message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
    }
}