using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Repository.Entyties;
using Repository.Query;

namespace Repository;

public class GameRepository(IConfiguration configuration) : IGameRepository
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<bool> CreateDefaultData(UserInitData userInitData)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();
        try
        {
            foreach (var money in userInitData.MoneyList)
            {
                await connection.ExecuteAsync(GameQuery.InsertMoney, money, transaction);
            }

            foreach (var stat in userInitData.StatList)
            {
                await connection.ExecuteAsync(GameQuery.InsertStat, stat, transaction);
            }
            
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await transaction.RollbackAsync();
            return false;
        }
    }
    
    public async Task InsertMoney(Money money)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(GameQuery.InsertMoney, money);
    }
    
    public async Task UpdateMoney(Money money)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(GameQuery.UpdateMoney, money);
    }

    public async Task<List<Money>> LoadMoney(long accountId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var moneyList = await connection.QueryAsync<Money>(GameQuery.LoadMoney, new { accountId });
        return moneyList.ToList();
    }

    public async Task InsertStat(Stat stat)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(GameQuery.InsertStat, stat);
    }

    public async Task UpdateStats(Stat stat)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(GameQuery.UpdateStat, stat);
    }

    public async Task<List<Stat>> LoadStat(long accountId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var statList = await connection.QueryAsync<Stat>(GameQuery.LoadStat, new { accountId });
        return statList.ToList();
    }
    
}