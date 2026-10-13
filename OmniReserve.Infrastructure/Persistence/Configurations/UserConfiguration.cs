using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        // Value Conversion: El puente entre el Dominio complejo y la BD simple
        builder.Property(user => user.Email)
               .HasConversion(
                   emailObj => emailObj.Value,
                   emailStr => new EmailAddress(emailStr)
               )
               .HasColumnName("Email")
               .HasMaxLength(255)
               .IsRequired();

        // Garantizar unicidad del correo electrónico a nivel de base de datos
        builder.HasIndex(user => user.Email)
               .IsUnique();
    }
}