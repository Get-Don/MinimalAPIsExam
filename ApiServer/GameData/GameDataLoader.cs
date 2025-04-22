using ApiServer.GameData.Data;
using ApiServer.Utility;

namespace ApiServer.GameData;

public partial class GameDataContainer
{
    public void Load(string csvDir)
    {
        csvDir = csvDir.TrimEnd('/').TrimEnd('\\');
        var fnCombinePath = Util.CombinePath(csvDir);

        Util.LoadCsv<MoneyGD>(fnCombinePath("MoneyGD.csv"), data =>
        {
            _moneyGDList = data.AsReadOnly();
        });
        
        Util.LoadCsv<StatGD>(fnCombinePath("StatGD.csv"), data =>
        {
            _statGDList = data.AsReadOnly();
        });
    }
}