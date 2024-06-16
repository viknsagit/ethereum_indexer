using Blockchain_Indexer.Blockchain;
using Blockchain_Indexer.Blockchain.Contracts;
using Blockchain_Indexer.Repositories;
using Blockchain_Indexer.Services;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOutputCache();
        builder.Services.AddSingleton<Indexer>();
        builder.Services.AddSingleton<ContractIndexer>();
        builder.Services.AddSingleton<PendingTransactionsStorage>();
        builder.Services.AddCors();
        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<TransactionsRepository>(options => options.UseNpgsql(connection));
        // Регистрация фабрики
        builder.Services.AddSingleton<TransactionsRepositoryFactory>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseOutputCache();
        app.MapControllers();
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        if (app.Configuration["reindex"] is "true")
        {
            await app.Services.GetService<Indexer>()!.ReindexBlocksAsync(0);
            await app.Services.GetService<ContractIndexer>()!.ReindexTokensFromDatabase();
        }

        await app.Services.GetService<Indexer>()!.NewBlockHeader();
        await app.Services.GetService<Indexer>()!.NewPendingTransactions();

        await app.RunAsync($"http://0.0.0.0:{app.Configuration["port"]}");
    }
}