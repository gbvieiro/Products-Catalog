using ProductsCatalog.Application.Common.Interfaces;

namespace ProductsCatalog.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
