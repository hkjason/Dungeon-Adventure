using UnityEngine;

[CreateAssetMenu(fileName ="")]
public class DamageSkill : Skill
{
    public DamageSkillType damageSkillType;
    public float damageMultiplier;
    public Status attachedStatus;
    public float attachChance;
    public override void SkillCast(Character target, Character caster)
    {
        switch (damageSkillType)
        {
            case (DamageSkillType.Strength):
                if (((int)(caster.strength * damageMultiplier) - target.armor) > 0)
                {
                    target.TakeDamage((int)(caster.strength * damageMultiplier),0, caster.critChance);
                    //target.TakeDamage((int)(caster.Strength * damageMultiplier) - target.armor,0);
                }
                else
                {
                    target.TakeDamage(0,0, caster.critChance);
                }
                break;
            case (DamageSkillType.Intelligence):
                if (((int)(caster.intelligence * damageMultiplier) - target.magicResistent) > 0)
                {
                    target.TakeDamage((int)(caster.intelligence * damageMultiplier),1, caster.critChance);
                    //target.TakeDamage((int)(caster.Intelligence * damageMultiplier) - target.magicResistent,1);
                }
                else
                {
                    target.TakeDamage(0,1, caster.critChance);
                }
                break;
            case (DamageSkillType.Dexterity):
                if (((int)(caster.dexterity * damageMultiplier) - (target.armor/2) - (target.magicResistent/2)) > 0)
                {
                    target.TakeDamage((int)(caster.dexterity * damageMultiplier),0, caster.critChance);
                    //target.TakeDamage((int)(caster.Dexterity * damageMultiplier) - target.armor,2);
                }
                else
                {
                    target.TakeDamage(0,2, caster.critChance);
                }
                break;
        }
        switch(effectPosition)
        {
            case EffectPosition.Random:
                int rtd = Random.Range(0, Enemy.instance.effectPosition.Length);
                GameObject temp = Instantiate(effect, Enemy.instance.effectPosition[rtd].transform.position, Quaternion.identity);
                Destroy(temp,1f);
                break;
            default:
                GameObject temp1 = Instantiate(effect, Enemy.instance.effectPosition[(int)effectPosition].transform.position, Quaternion.identity);
                Destroy(temp1,1f);
                break;
        }
        if (attachedStatus != null)
        {
            float tempChance = Random.Range(0, 1f);
            if (tempChance <= attachChance)
            {
                Player.instance.status.Add(attachedStatus.Deepcopy());
            }
        }
    }
}

public enum DamageSkillType
{
    Strength,
    Intelligence,
    Dexterity
}
