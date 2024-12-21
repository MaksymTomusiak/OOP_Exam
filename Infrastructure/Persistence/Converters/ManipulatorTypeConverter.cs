using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters;

public class ManipulatorTypeConverter(): ValueConverter<ManipulatorType, short>(
    x => (short)x,
    x => (ManipulatorType)Enum.ToObject(typeof(ManipulatorType), x));