using Repository.Entyties;

namespace Repository;

public interface IAccountRepository
{
    Task<bool> Exists(string email);
    Task<long> CreateAccount(string email, string password);
    Task RemoveAccount(long accountId);
    Task<Account?> GetAccount(string email);
    Task Login(long accountId);
}