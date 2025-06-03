using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial5.Models;

[Table("Trip")]
public class Trip
{
    [Key]
    public int IdTrip { get; set; }
    
    [MaxLength(120)]
    public string Name { get; set; }
    
    [MaxLength(220)]
    public string Description { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }
    
    public int MaxPeople { get; set; }

    public ICollection<ClientTrip> ClientTrips { get; set; }
    public ICollection<CountryTrip> CountryTrips { get; set; }
}