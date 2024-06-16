using Blockchain_Indexer.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionsRepositoryFactory
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TransactionsRepositoryFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public TransactionsRepository Create()
    {
        var scope = _scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TransactionsRepository>();
    }
}
