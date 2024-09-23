[System.Serializable]
public class MinorAttribute
{
    public MinorAttributeType minorAttributeType;
    public int value;
    public int obtainPercentage;
}

[System.Serializable]
public enum MinorAttributeType
{
    MaxHp,
    Armor,
    MagicResistent,
    DodgeRate,
    BaseDamage,
    CritChance,
    Luck,
    Faith
}
