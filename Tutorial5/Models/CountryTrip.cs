using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[PrimaryKey(nameof(IdCountry), nameof(IdTrip))]
[Table("Country_Trip")]
public class CountryTrip
{
    [ForeignKey(nameof(Country))]
    public int IdCountry { get; set; }
    
    [ForeignKey(nameof(Trip))]
    public int IdTrip { get; set; }

    public Country Country { get; set; } = null!;
    public Trip Trip { get; set; } = null!;
}