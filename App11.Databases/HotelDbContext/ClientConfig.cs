using System.Data.Entity.ModelConfiguration;
using App11.Databases.Models;

namespace App11.Databases.HotelDbContext;

public class ClientConfig : EntityTypeConfiguration<Client>
{
    public ClientConfig()
    {
        Property(client => client.Account).IsOptional().HasMaxLength(20);
        HasRequired(client => client.Room).WithMany(room => room.Clients).WillCascadeOnDelete(true);

        ToTable("Clients");
    }
}