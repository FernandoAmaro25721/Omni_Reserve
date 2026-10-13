using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        // Nombre explícito de la tabla
        builder.ToTable("Rooms");

        // Identificador primario
        builder.HasKey(r => r.Id);

        // Limitar el tamaño del número de habitación
        builder.Property(r => r.RoomNumber)
            .IsRequired()
            .HasMaxLength(10);

        // Precisión para decimales financieros
        builder.Property(r => r.PricePerNight)
            .IsRequired()
            .HasPrecision(18, 2);

        // Conversión del Enum a Texto plano en la base de datos
        builder.Property(r => r.Type)
            .IsRequired()
            .HasConversion<string>();
    }
}