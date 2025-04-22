namespace Repository.Entyties;

public class Money
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public byte MoneyType { get; set; }
    public long Value { get; set; }
}