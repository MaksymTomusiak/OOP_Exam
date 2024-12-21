using Domain;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class IndustrialManipulatorConfigurator: IEntityTypeConfiguration<IndustrialManipulator>
{
    public void Configure(EntityTypeBuilder<IndustrialManipulator> builder)
    {
        builder.Property(x => x.WeldsAmount).HasDefaultValue(0);
    }
}