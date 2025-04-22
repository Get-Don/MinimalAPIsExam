using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Repository.Entyties;
using Repository.Query;

namespace Repository;

public class AccountRepository(IConfiguration configuration) : IAccountRepository
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<bool> Exists(string email)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var exists = await connection.ExecuteScalarAsync<bool>(AccountQuery.ExistsAccount, new { email });
        return exists;
    }

    public async Task<long> CreateAccount(string email, string password)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var id = await connection.ExecuteScalarAsync<long>(AccountQuery.CreateAccount, new { Email = email, Password = password });
        return id;
    }

    public async Task RemoveAccount(long accountId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var id = await connection.ExecuteAsync(AccountQuery.RemoveAccount, new { accountId });
    }

    public async Task<Account?> GetAccount(string email)
    {
        await using var connection = new MySqlConnection(_connectionString);
        var account = await connection.QueryFirstOrDefaultAsync<Account>(AccountQuery.GetAccount, new { email });
        return account;
    }

    public async Task Login(long accountId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(AccountQuery.Login, new { accountId });
    }
    
    
}