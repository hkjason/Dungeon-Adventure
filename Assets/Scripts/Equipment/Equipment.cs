using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Equipment")]
public class Equipment : ScriptableObject
{
    public int id;

    public string itemName;
    public string description;

    public Type equipmentType;
    public Attributes equipmentAttributes;

    public Sprite artWork;
    public int attributeValue;
    public MinorAttribute[] minorAttributesDB;
    public List<MinorAttribute> minorAttributes = new List<MinorAttribute>();

    public Equipment DeepCopy()
    {
        Equipment copy = CreateInstance<Equipment>();
        copy.id = id;
        copy.minorAttributes = new List<MinorAttribute>();
        copy.itemName = itemName;
        copy.description = description;
        copy.equipmentType = equipmentType;
        copy.equipmentAttributes = equipmentAttributes;
        copy.artWork = artWork;
        copy.attributeValue = (int)(attributeValue * ItemManager.instance.GrowthRate);

        if (minorAttributesDB != null)
        {
            for (int i = 0; i < minorAttributesDB.Length; i++)
            {
                int randomValue = Random.Range(1, 100);
                if (minorAttributesDB[i].obtainPercentage >= randomValue)
                {
                    copy.minorAttributes.Add(minorAttributesDB[i]);
                    copy.minorAttributes.Last().value = (int)(copy.minorAttributes.Last().value * ItemManager.instance.GrowthRate);
                }
            }
        }
        return copy;
    }
}

[System.Serializable]
public class EquipmentArray
{
    public Equipment[] equipment;
}


[System.Serializable]
public enum Type
{
    Helmat,
    Chestplate,
    Gauntlet,
    Greaves,
    Axe,
    Hammer,
    Sword
}

[System.Serializable]
public enum Attributes
{
    Strength,
    Intelligence,
    Dexterity,
    MaxHp,
    Armor,
    MagicResistent
}