using ApiServer.GameData;

namespace ApiServer.Protocol;

public class StatDTO
{
    public long Id { get; set; }
    public StatType StatType { get; set; }
    public int Level { get; set; }
}