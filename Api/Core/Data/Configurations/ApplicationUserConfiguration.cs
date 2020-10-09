using Core.Data.Entities.Identity;
using Core.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.HasQueryFilter(user => !user.IsDeleted);

            builder.HasKey(user => user.Id)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(user => user.Role)
                .HasConversion(new EnumToStringConverter<Role>())
                .IsRequired();

            builder.Property(user => user.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(user => user.SettingsJson)
                .HasMaxLength(4001);

            builder.Property(user => user.ContactJson)
                .HasMaxLength(4001);

            builder.Property(user => user.CreatedAt)
                .IsRequired();

            builder.Property(user => user.CreatedBy)
                .IsRequired();

            builder.Property(user => user.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}

