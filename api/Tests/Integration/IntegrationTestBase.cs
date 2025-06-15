using Microsoft.EntityFrameworkCore;

namespace Tests.Integration;

public abstract class IntegrationTestBase<TDbContext> : IDisposable
        where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    private bool _disposed = false;

    protected IntegrationTestBase()
    {
        DbContext = TestDbContextFactory.CreateInMemoryContext<TDbContext>(SeedDatabase);
    }

    protected abstract void SeedDatabase(TDbContext context);

    protected async Task<T> ExecuteInSeparateContextAsync<T>(Func<TDbContext, Task<T>> operation)
    {
        await using var context = TestDbContextFactory.CreateInMemoryContext<TDbContext>(SeedDatabase);
        return await operation(context);
    }

    protected async Task ExecuteInSeparateContextAsync(Func<TDbContext, Task> operation)
    {
        await using var context = TestDbContextFactory.CreateInMemoryContext<TDbContext>(SeedDatabase);
        await operation(context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            DbContext?.Dispose();
            _disposed = true;
        }
    }
}
