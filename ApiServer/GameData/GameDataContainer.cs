using ApiServer.GameData.Data;

namespace ApiServer.GameData;

public partial class GameDataContainer
{
    #region Singleton
    private static readonly Lazy<GameDataContainer> _instance = new(() => new GameDataContainer());
    public static GameDataContainer Instance => _instance.Value;
    
    private GameDataContainer()
    {
        
    }
    #endregion
    
    private IReadOnlyList<MoneyGD>? _moneyGDList;
    private IReadOnlyList<StatGD>? _statGDList;

    public IReadOnlyList<MoneyGD> MoneyGDList
    {
        get
        {
            if (_moneyGDList == null) throw new InvalidOperationException("MoneyGD is not loaded.");
            return _moneyGDList;
        }
    }
    
    public IReadOnlyList<StatGD> StatGDList
    {
        get
        {
            if (_statGDList == null) throw new InvalidOperationException("StatGD is not loaded.");
            return _statGDList;
        }
    }
}