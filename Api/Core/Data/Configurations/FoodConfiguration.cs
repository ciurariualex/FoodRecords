using Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Foods");

            builder.HasQueryFilter(food => !food.IsDeleted);

            builder.HasKey(food => food.Id)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Property(food => food.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(food => food.Ingredients)
                .HasMaxLength(4001)
                .IsRequired();

            builder.Property(food => food.Minutes)
                .IsRequired();

            builder.Property(food => food.Price)
                .IsRequired();

            builder.Property(food => food.CreatedAt)
                .IsRequired();

            builder.Property(food => food.CreatedBy)
                .IsRequired();

            builder.Property(food => food.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}

