using ApiServer.GameData;

namespace ApiServer.Protocol;

public class StatUpgradeInfoDTO
{
    public StatType StatType { get; set; }
    public int BeforeLevel { get; set; }
    public int AfterLevel { get; set; }
    public long Cost { get; set; }
}

public class UpgradeStatsDTO : AccountDTO
{
    public List<StatUpgradeInfoDTO> UpgradeStatList { get; set; }
    public MoneyLogDTO MoneyLog { get; set; }
}