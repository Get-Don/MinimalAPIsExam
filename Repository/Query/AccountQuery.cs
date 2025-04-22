namespace Repository.Query;

public static class AccountQuery
{
    public const string ExistsAccount = "SELECT EXISTS (SELECT 1 FROM t_account WHERE email = @Email);";

    public const string CreateAccount = """
                                        INSERT INTO t_account (email, password) 
                                        VALUES (@Email, @Password); 
                                        SELECT LAST_INSERT_ID();
                                        """;
    
    public const string RemoveAccount = "DELETE FROM t_account WHERE id = @AccountId;";

    public const string GetAccount = """
                                     SELECT 
                                         id AS AccountId, 
                                         email AS Email, 
                                         password AS Password, 
                                         last_login_time AS LastLoginTime, 
                                         create_time AS CreatedTime 
                                     FROM t_account 
                                     WHERE email = @Email;
                                     """;

    public const string Login = "UPDATE t_account SET last_login_time = Now() WHERE id = @AccountId;";
}