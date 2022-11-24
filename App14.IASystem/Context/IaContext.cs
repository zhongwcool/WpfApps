using App14.IASystem.Models;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.Context;

public class IaContext : DbContext
{
    public DbSet<Farm> Farms { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<RecWqm> RecWqms { get; set; }
    public DbSet<Pool> Pools { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ia-system.db");
        //optionsBuilder.UseLazyLoadingProxies();
    }
}