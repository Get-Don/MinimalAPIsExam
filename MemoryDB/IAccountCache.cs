namespace MemoryDB;

public interface IAccountCache
{
    Task Login(long accountId);
    Task<bool> CheckLogin(long accountId);
    Task ExtendLogin(long accountId);
    Task<bool> Lock(long accountId);
    Task<bool> Unlock(long accountId);
}