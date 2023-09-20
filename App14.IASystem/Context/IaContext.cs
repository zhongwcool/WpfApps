using System.Linq;
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
    }

    public override int SaveChanges()
    {
        base.SaveChanges();

        UpdateSortOrder(); //TODO 如何优化

        return base.SaveChanges();
    }

    private bool ShouldUpdateSortOrder()
    {
        var changedProducts = ChangeTracker.Entries<Pool>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted)
            .Select(e => e.Entity);

        return changedProducts.Any();
    }

    private void UpdateSortOrder()
    {
        // 更新排序逻辑
        var productsToUpdate = Pools.OrderBy(p => p.DisplayIndex).ToList();

        uint currentIndex = 0;
        foreach (var product in productsToUpdate)
        {
            if (product.DisplayIndex != currentIndex) product.DisplayIndex = currentIndex;

            currentIndex++;
        }
    }
}