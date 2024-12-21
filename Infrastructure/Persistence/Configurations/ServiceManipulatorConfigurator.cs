using Domain;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ServiceManipulatorConfigurator: IEntityTypeConfiguration<ServiceManipulator>
{
    public void Configure(EntityTypeBuilder<ServiceManipulator> builder)
    {
        builder.Property(x => x.ServesAmount).HasDefaultValue(0);
    }
}