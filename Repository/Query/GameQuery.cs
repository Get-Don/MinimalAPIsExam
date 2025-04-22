namespace Repository.Query;

public static class GameQuery
{
    public const string InsertMoney = """
                                      INSERT INTO t_money (id, aid, money_type, value)
                                      VALUES (@Id, @AccountId, @MoneyType, @Value);
                                      """;
    
    public const string UpdateMoney = "UPDATE t_money SET value = @Value WHERE id = @Id;";
    
    public const string LoadMoney = """
                                    SELECT 
                                        id AS Id, 
                                        aid AS AccountId, 
                                        money_type AS MoneyType, 
                                        `value` AS Value
                                    FROM t_money
                                    WHERE aid = @AccountId;
                                    """;
    
    public const string InsertStat = """
                                     INSERT INTO t_stats (id, aid, stat_type, level)
                                     VALUES (@Id, @AccountId, @StatType, @Level);
                                     """;
    
    public const string UpdateStat = "UPDATE t_stats SET level = @Level WHERE id = @Id;";
    
    public const string LoadStat = """
                                   SELECT 
                                       id AS Id, 
                                       stat_type AS StatType, 
                                       level AS Level
                                   FROM t_stats
                                   WHERE aid = @AccountId;
                                   """;
}