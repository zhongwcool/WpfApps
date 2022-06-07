using System.Data.Entity;
using App11.Databases.Models;

namespace App11.Databases.HotelDbContext;

public class HotelContext : DbContext
{
    public HotelContext(string connectionString = "MyHotel") : base(connectionString)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Configurations.Add(new PersonConfig());
        modelBuilder.Configurations.Add(new ClientConfig());
        modelBuilder.Configurations.Add(new RoomConfig());
    }
}