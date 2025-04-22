using CsvHelper.Configuration.Attributes;

namespace ApiServer.GameData.Data;

public class StatGD
{
    [Name("UID")] public int UId { get; set; }
    [Name("Stat_Type")] public StatType StatType { get; set; }
    [Name("Max_Level")] public int MaxLevel { get; set; }
    [Name("Base_Cost")] public long BaseCost { get; set; }
    [Name("Multiplier")] public float Multiplier { get; set; }
}