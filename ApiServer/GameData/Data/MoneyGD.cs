using CsvHelper.Configuration.Attributes;

namespace ApiServer.GameData.Data;

public class MoneyGD
{
    [Name("UID")] public int UId { get; set; }
    [Name("Money_Type")] public MoneyType MoneyType { get; set; }
}