using ApiServer.GameData;

namespace ApiServer.Protocol;

public class MoneyDTO
{
    public long Id { get; set; }
    public MoneyType MoneyType { get; set; }
    public int Value { get; set; }
}