using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public int id;
    public string skillName;
    public int clickCD;

    public string skillDescription;
    public SkillType skillType;
    public Sprite skillIcon;
    public GameObject effect;
    public EffectPosition effectPosition;

    public abstract void SkillCast(Character target, Character caster);
}

public enum SkillType
{
    DamageSkill,
    HealingSkill,
    StatusSkill
}

public enum EffectPosition
{
    Head,
    Heart,
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot,
    Random
}