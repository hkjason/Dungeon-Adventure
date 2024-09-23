using UnityEngine;

[CreateAssetMenu(fileName = "")]
public class HealingSkill : Skill
{
    public float healingMultiplier;

    public override void SkillCast(Character target, Character caster)
    {
        caster.TakeHeal((int)(caster.faith * healingMultiplier),0);

        GameObject temp = Instantiate(effect, caster.transform.position, Quaternion.identity);
        Destroy(temp,1f);
    }
}
