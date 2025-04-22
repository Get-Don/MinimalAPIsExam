namespace Repository.Entyties;

public class Stat
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public byte StatType { get; set; }
    public int Level { get; set; }
}