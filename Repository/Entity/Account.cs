namespace Repository.Entyties;

public class Account
{
    public long AccountId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime LastLoginTime { get; set; }
    public DateTime CreateTime { get; set; }
}