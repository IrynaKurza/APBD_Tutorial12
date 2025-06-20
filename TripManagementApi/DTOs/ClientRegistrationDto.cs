namespace TripManagementApi.DTOs
{
    public class ClientRegistrationDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string Pesel { get; set; } = null!;
        public int IdTrip { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}