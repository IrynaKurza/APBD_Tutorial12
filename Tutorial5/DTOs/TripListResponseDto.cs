namespace Tutorial5.DTOs;

public class TripListResponseDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripWithDetailsDto> Trips { get; set; }
}