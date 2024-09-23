using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string enemyName;
    public RuntimeAnimatorController animationController;
    public Vector3[] effectPositions = new Vector3[6];
    // public Vector3 headPosition;
    // public Vector3 heartPosition;
    // public Vector3 leftHandPosition;
    // public Vector3 rightHandPosition;
    // public Vector3 leftFootPosition;
    // public Vector3 rightFoodPosition;

    [Header("Enemy Stats")]
    public int hp;
    public int strength;
    public int intelligence;
    public int dexterity;
    public int armor;
    public int magicResistent;
    public float dodgeRate;
    public int baseDamage;
    public float critChance;
    public int luck;
    public int faith;

    public int cd;

    public List<Skill> skills;
    public List<float> skillsChance;
}
