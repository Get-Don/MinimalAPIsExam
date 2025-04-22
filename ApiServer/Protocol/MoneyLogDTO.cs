using ApiServer.GameData;

namespace ApiServer.Protocol;

public class MoneyLogDTO
{
    public MoneyType MoneyType { get; set; }
    public long BeforeValue { get; set; }
    public long AfterValue { get; set; }
}