using UnityEngine;

[CreateAssetMenu(fileName = "")]
public class ValueStatus : Status
{
    public ValueStatusType valueStatusType;
    public float multiplier;
    public bool statusModified;

    public override Status Deepcopy()
    {
        ValueStatus copy = CreateInstance<ValueStatus>();
        copy.id = id;
        copy.statusName =statusName;
        copy.remainedClick = remainedClick;
        copy.multiplier = multiplier;
        copy.artWork = artWork;
        return copy;
    }
    public override void StatusFunctioning(Character target)
    {
        if (remainedClick == 0)
        {
            Debug.Log("remove");
            //remove
            switch (valueStatusType)
            {
                case (ValueStatusType.Strength):
                    target.strength -= Mathf.RoundToInt((target.baseStat.strength * multiplier));
                    break;
                case (ValueStatusType.Intelligence):
                    target.intelligence -= Mathf.RoundToInt((target.baseStat.intelligence * multiplier));
                    break;
                case (ValueStatusType.Dexterity):
                    target.dexterity -= Mathf.RoundToInt((target.baseStat.dexterity * multiplier));
                    break;
                case (ValueStatusType.Armor):
                    target.armor -= Mathf.RoundToInt((target.baseStat.armor * multiplier));
                    break;
                case (ValueStatusType.MagicResistent):
                    target.magicResistent -= Mathf.RoundToInt((target.baseStat.magicResistent * multiplier));
                    break;
                case (ValueStatusType.DodgeRate):
                    target.dodgeRate -= Mathf.RoundToInt((target.baseStat.dodgeRate * multiplier));
                    break;
                case (ValueStatusType.BaseDamage):
                    target.baseDamage -= Mathf.RoundToInt((target.baseStat.baseDamage * multiplier));
                    break;
                case (ValueStatusType.CritChance):
                    target.critChance -= Mathf.RoundToInt((target.baseStat.critChance * multiplier));
                    break;
                case (ValueStatusType.Luck):
                    target.luck -= Mathf.RoundToInt((target.baseStat.luck * multiplier));
                    break;
                case (ValueStatusType.Faith):
                    target.faith -= Mathf.RoundToInt((target.baseStat.faith * multiplier));
                    break;
            }
        }
        else
        {
            if (!statusModified)
            {
                switch (valueStatusType)
                {
                    case (ValueStatusType.Strength):
                        target.strength += Mathf.RoundToInt((target.baseStat.strength * multiplier));
                        break;
                    case (ValueStatusType.Intelligence):
                        target.intelligence += Mathf.RoundToInt((target.baseStat.intelligence * multiplier));
                        break;
                    case (ValueStatusType.Dexterity):
                        target.dexterity += Mathf.RoundToInt((target.baseStat.dexterity * multiplier));
                        break;
                    case (ValueStatusType.Armor):
                        target.armor += Mathf.RoundToInt((target.baseStat.armor * multiplier));
                        break;
                    case (ValueStatusType.MagicResistent):
                        target.magicResistent += Mathf.RoundToInt((target.baseStat.magicResistent * multiplier));
                        break;
                    case (ValueStatusType.DodgeRate):
                        target.dodgeRate += Mathf.RoundToInt((target.baseStat.dodgeRate * multiplier));
                        break;
                    case (ValueStatusType.BaseDamage):
                        target.baseDamage += Mathf.RoundToInt((target.baseStat.baseDamage * multiplier));
                        break;
                    case (ValueStatusType.CritChance):
                        target.critChance += Mathf.RoundToInt((target.baseStat.critChance * multiplier));
                        break;
                    case (ValueStatusType.Luck):
                        target.luck += Mathf.RoundToInt((target.baseStat.luck * multiplier));
                        break;
                    case (ValueStatusType.Faith):
                        target.faith += Mathf.RoundToInt((target.baseStat.faith * multiplier));
                        break;
                }
                statusModified = true;
            }
        }
    }
}
[System.Serializable]
public enum ValueStatusType
{
    Strength,
    Intelligence,
    Dexterity,
    Armor,
    MagicResistent,
    DodgeRate,
    BaseDamage,
    CritChance,
    Luck,
    Faith
}