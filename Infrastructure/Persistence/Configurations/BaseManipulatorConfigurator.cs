using Domain;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseManipulatorConfigurator: IEntityTypeConfiguration<BaseManipulator>
{
    public void Configure(EntityTypeBuilder<BaseManipulator> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Type)
            .HasConversion(new ManipulatorTypeConverter());
        builder.Property(x => x.Position).IsRequired().HasColumnType("varchar(255)");
    }
}