using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class TripContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<ClientTrip> ClientTrips { get; set; }
    public DbSet<CountryTrip> CountryTrips { get; set; }
    
    protected TripContext()
    {
    }

    public TripContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(c =>
        {
            c.ToTable("Client");
            c.HasKey(e => e.IdClient);
            c.Property(e => e.FirstName).HasMaxLength(120);
            c.Property(e => e.LastName).HasMaxLength(120);
            c.Property(e => e.Email).HasMaxLength(120);
            c.Property(e => e.Telephone).HasMaxLength(120);
            c.Property(e => e.Pesel).HasMaxLength(120);
        });

        modelBuilder.Entity<Trip>(t =>
        {
            t.ToTable("Trip");
            t.HasKey(e => e.IdTrip);
            t.Property(e => e.Name).HasMaxLength(120);
            t.Property(e => e.Description).HasMaxLength(220);
        });

        modelBuilder.Entity<Country>(c =>
        {
            c.ToTable("Country");
            c.HasKey(e => e.IdCountry);
            c.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(ct =>
        {
            ct.ToTable("Client_Trip");
            ct.HasKey(e => new { e.IdClient, e.IdTrip });
            
            ct.HasOne(d => d.Client)
                .WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            ct.HasOne(d => d.Trip)
                .WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<CountryTrip>(ct =>
        {
            ct.ToTable("Country_Trip");
            ct.HasKey(e => new { e.IdCountry, e.IdTrip });
            
            ct.HasOne(d => d.Country)
                .WithMany(p => p.CountryTrips)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Country_Trip_Country");

            ct.HasOne(d => d.Trip)
                .WithMany(p => p.CountryTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Country_Trip_Trip");
        });
    }
}