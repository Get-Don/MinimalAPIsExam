using StackExchange.Redis;

namespace MemoryDB;

public class AccountCache(IConnectionMultiplexer redis, long loginExpiredMinute) : IAccountCache
{
    // 상황에 맞게 Database 번호 사용
    private readonly IDatabase _db = redis.GetDatabase();

    private static string GetLoginKey(long accountId) => $"account:login:{accountId}";
    private static string GetLockKey(long accountId) => $"account:lock:{accountId}";

    public async Task<bool> Lock(long accountId) =>
        await _db.StringSetAsync(GetLockKey(accountId), "1", TimeSpan.FromSeconds(10), When.NotExists);

    public async Task<bool> Unlock(long accountId) => await _db.KeyDeleteAsync(GetLockKey(accountId));

    public async Task Login(long accountId)
    {
        var key = GetLoginKey(accountId);
        await _db.StringSetAsync(
            key,
            DateTime.UtcNow.ToString("o"),
            expiry: TimeSpan.FromMinutes(loginExpiredMinute)
        );
    }

    public async Task ExtendLogin(long accountId)
    {
        var key = GetLoginKey(accountId);
        if (await _db.KeyExistsAsync(key))
        {
            await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(loginExpiredMinute));
        }
    }

    public async Task<bool> CheckLogin(long accountId)
    {
        var key = GetLoginKey(accountId);
        var value = await _db.StringGetAsync(key);

        return !value.IsNullOrEmpty;
    }
}