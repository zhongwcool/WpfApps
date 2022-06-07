using System.Data.Entity.ModelConfiguration;
using App11.Databases.Models;

namespace App11.Databases.HotelDbContext;

public class RoomConfig : EntityTypeConfiguration<Room>
{
    public RoomConfig()
    {
        HasKey(room => room.RoomId);
        Property(room => room.Number).IsRequired().HasMaxLength(5);
        Property(room => room.Type).IsRequired();

        ToTable("Rooms");
    }
}