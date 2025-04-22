namespace Repository.Entyties;

public class UserInitData
{
    public long AccountId { get; set; }
    public required List<Money> MoneyList { get; init; }
    public required List<Stat> StatList { get; init; }
}