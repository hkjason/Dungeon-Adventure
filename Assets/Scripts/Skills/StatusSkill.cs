using UnityEngine;

[CreateAssetMenu(fileName = "")]
public class StatusSkill : Skill
{
    public bool isSelfCast;
    public Status attachStatus;

    public override void SkillCast(Character target, Character caster)
    {
        if (isSelfCast)
            StatusManager.instance.ActivateStatus(attachStatus, caster);
        else
        {
            Player player = Player.instance;
            if (target == player)
            {
                if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Debuff))
                {
                    return;
                }
            }
            StatusManager.instance.ActivateStatus(attachStatus, target);
        }
    }
}