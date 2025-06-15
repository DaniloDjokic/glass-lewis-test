using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Tests.Integration;

public class TestDbContextFactory
{
    public static DbContext CreateInMemoryContext<T>() where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .Options;

        var instance = Activator.CreateInstance(typeof(T), options) as T;
        return instance ?? throw new InvalidOperationException($"Could not create an instance of {typeof(T).Name}.");
    }

    public static T CreateInMemoryContext<T>(Action<T>? seedData = null) where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        var instance = Activator.CreateInstance(typeof(T), options) as T;
        var context = instance ?? throw new InvalidOperationException($"Could not create an instance of {typeof(T).Name}.");

        if (seedData != null)
        {
            seedData(context);
            context.SaveChanges();
        }

        return context;
    }
}
