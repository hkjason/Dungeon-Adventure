using System.Collections.Generic;

[System.Serializable]
public class CharacterDataBase
{
    public string characterName;
    public int maxhp;
    public int strength;
    public int intelligence;
    public int Dexterity;
    public int armor;
    public int magicResistent;
    public float dodgeRate;
    public int baseDamage;
    public float critChance;
    public int luck;
    public int faith;
    public string normalAttack;
    public List<int> skills = new List<int>();
}
