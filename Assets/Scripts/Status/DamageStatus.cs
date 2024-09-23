using UnityEngine;

[CreateAssetMenu(fileName = "")]
public class DamageStatus : Status
{
    public int damage;
    public override Status Deepcopy()
    {
        DamageStatus copy = CreateInstance<DamageStatus>();
        copy.id = id;
        copy.statusName = statusName;
        copy.remainedClick = remainedClick;
        copy.damage = damage;
        copy.artWork = artWork;
        return copy;
    }
    public override void StatusFunctioning(Character target)
    {
        target.TakeDamage(damage,3,0);
    }
}
