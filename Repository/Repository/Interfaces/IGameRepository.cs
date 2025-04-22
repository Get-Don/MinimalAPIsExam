using Repository.Entyties;

namespace Repository;

public interface IGameRepository
{
    Task<bool> CreateDefaultData(UserInitData userInitData);
    
    Task InsertMoney(Money money);
    Task UpdateMoney(Money money);
    Task<List<Money>> LoadMoney(long accountId);
    
    Task InsertStat(Stat stat);
    Task UpdateStats(Stat stat);
    Task<List<Stat>> LoadStat(long accountId);
}