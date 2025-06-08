namespace TripManagementApi.DTOs
{
    public class ClientRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
        public int IdTrip { get; set; }
        public string? TripName { get; set; } // Optional - not needed per teacher's note
        public DateTime? PaymentDate { get; set; }
    }
}