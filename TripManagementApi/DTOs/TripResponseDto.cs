namespace TripManagementApi.DTOs
{
    public class TripResponseDto
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<TripDto> Trips { get; set; } = new List<TripDto>();
    }
}