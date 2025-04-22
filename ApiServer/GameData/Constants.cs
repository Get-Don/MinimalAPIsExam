namespace ApiServer.GameData;

public enum MoneyType
{
    Gold,
    FreeDia,    // 무료 다이아
    PaidDia,    // 유료 다이아
}

public enum StatType
{
    Attack, // 공격
    Health, // 체력
    AttackSpeed, // 공격 속도
    HealthRegen, // 체력 회복
    CriticalChance, // 크리티컬 확률
    CriticalMultiplier, // 크리티컬 배수
    AttackRange // 공격 범위
}