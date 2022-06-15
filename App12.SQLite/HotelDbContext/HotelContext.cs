using App12.SQLite.Models;
using Microsoft.EntityFrameworkCore;

namespace App12.SQLite.HotelDbContext;

public class HotelContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=MyHotel.db");
        optionsBuilder.UseLazyLoadingProxies();
    }
}