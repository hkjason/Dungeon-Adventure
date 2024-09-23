using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public static Player instance;

    public List<Artifacts> artifacts;
    public float reflectMagnitude=0f;
    public string normalAttackName;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        InitializePlayerStat();
    }

    private void InitializePlayerStat()
    {
        GameManager.instance.LoadData();

        int idx = GameManager.instance.saveData.lastPlayedSlot;
        SaveData.SaveSlot saveSlot = GameManager.instance.saveData.saveSlot[idx];
        CharacterDataBase characterData = CharacterSelection.instance.characterData[CharacterSelection.instance.idx];

        if (GameManager.instance.saveData.loadType == LoadType.NewGame)
        {
            saveSlot.characterName = characterData.characterName;
            saveSlot.maxhp = characterData.maxhp;
            saveSlot.hp = characterData.maxhp;
            saveSlot.strength = characterData.strength;
            saveSlot.intelligence = characterData.intelligence;
            saveSlot.dexterity = characterData.Dexterity;
            saveSlot.temphp = 0;
            saveSlot.armor = characterData.armor;
            saveSlot.magicResistent = characterData.magicResistent;
            saveSlot.dodgeRate = characterData.dodgeRate;
            saveSlot.baseDamage = characterData.baseDamage;
            saveSlot.critChance = characterData.critChance;
            saveSlot.luck = characterData.luck;
            saveSlot.faith = characterData.faith;
            saveSlot.normalAttack = characterData.normalAttack;
            saveSlot.skills = characterData.skills;
        }
        characterName = saveSlot.characterName;

        maxHp = saveSlot.maxhp;
        hp = saveSlot.hp;
        temphp = saveSlot.temphp;

        strength = saveSlot.strength;
        intelligence = saveSlot.intelligence;
        dexterity = saveSlot.dexterity;

        armor = saveSlot.armor;
        magicResistent = saveSlot.magicResistent;
        dodgeRate = saveSlot.dodgeRate;
        baseDamage = saveSlot.baseDamage;
        critChance = saveSlot.critChance;
        luck = saveSlot.luck;
        faith = saveSlot.faith;

        normalAttackName = saveSlot.normalAttack;

        for (int i = 0; i < 5; i++)
        {
            if (saveSlot.equipments[i] != null && saveSlot.equipments[i].itemName != "")
            {
                Equipment temp = ScriptableObject.CreateInstance<Equipment>();
                temp.id = saveSlot.equipments[i].id;
                temp.itemName = saveSlot.equipments[i].itemName;
                temp.description = saveSlot.equipments[i].description;
                temp.equipmentType = saveSlot.equipments[i].equipmentType;
                temp.attributeValue = saveSlot.equipments[i].attributeValue;
                temp.equipmentAttributes = saveSlot.equipments[i].equipmentAttributes;
                temp.minorAttributes = saveSlot.equipments[i].minorAttributes;

                foreach (EquipmentArray arr in ItemManager.instance.t1Equipment)
                {
                    foreach (Equipment e in arr.equipment)
                    {
                        if (temp.id == e.id)
                            temp.artWork = e.artWork;
                    }
                }
                foreach (EquipmentArray arr in ItemManager.instance.t2Equipment)
                {
                    foreach (Equipment e in arr.equipment)
                    {
                        if (temp.id == e.id)
                            temp.artWork = e.artWork;
                    }
                }
                foreach (EquipmentArray arr in ItemManager.instance.t3Equipment)
                {
                    foreach (Equipment e in arr.equipment)
                    {
                        if (temp.id == e.id)
                            temp.artWork = e.artWork;
                    }
                }
                foreach (EquipmentArray arr in ItemManager.instance.t4Equipment)
                {
                    foreach (Equipment e in arr.equipment)
                    {
                        if (temp.id == e.id)
                            temp.artWork = e.artWork;
                    }
                }
                foreach (EquipmentArray arr in ItemManager.instance.t5Equipment)
                {
                    foreach (Equipment e in arr.equipment)
                    {
                        if (temp.id == e.id)
                            temp.artWork = e.artWork;
                    }
                }
                EquipItem(temp);
            }
        }

        skills.Clear();
        foreach (int i in saveSlot.skills)
        {
            foreach (Skill s in SkillDataBase.instance.damageSkillDB)
            {
                if (i == s.id)
                {
                    skills.Add(s);
                    continue;
                }
            }
            foreach (Skill s in SkillDataBase.instance.healingSkillDB)
            {
                if (i == s.id)
                {
                    skills.Add(s);
                    continue;
                }
            }
            foreach (Skill s in SkillDataBase.instance.statusSkillDB)
            {
                if (i == s.id)
                {
                    skills.Add(s);
                    continue;
                }
            }
        }

        List<Artifacts> artifactDataBase = ArtifactManager.instance.artifacts;
        artifacts.Clear();
        foreach (SaveData.ArtifactSave a in saveSlot.artifacts)
        {
            for (int i=0;i<artifactDataBase.Count;i++)
            {
                if (a.artifactType == artifactDataBase[i].artifactType)
                {
                    Artifacts temp = artifactDataBase[i].Deepcopy();
                    temp.used = a.used;
                    artifacts.Add(temp);
                    GameObject go1 = UIManager.instance.battleArtifactPanel.transform.GetChild(artifacts.Count + 1).gameObject;
                    GameObject go2 = UIManager.instance.normalArtifactPanel.transform.GetChild(artifacts.Count + 1).gameObject;
                    go1.GetComponent<Image>().sprite = go2.GetComponent<Image>().sprite = artifactDataBase[i].artifactArtwork;
                    go1.SetActive(true);
                    go2.SetActive(true);
                    artifactDataBase.RemoveAt(i);
                }
            }
        }

        GameManager.instance.SaveData();
    }

    public override void AfterAttack()
    {
        if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Vampiric))
        {
            TakeHeal((int)(Enemy.instance.lastDamageTaken * 0.25f), 1);
        }
    }

    protected override void AfterTakeDamage(int damageVal, PopUpType popUpType)
    {
        LevelManager.instance.tempDamageTaken += damageVal;

        UIManager.instance.PlayerStatRefresh();
        TextMesh textMesh = BattleManager.instance.damagePopUpObj.transform.GetChild(0).GetComponent<TextMesh>();
        textMesh.color = new Color(255, 255, 255, 255);
        textMesh.text = damageVal.ToString();
        PopUp(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), popUpType);
        //Instantiate(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);

        if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Reflect))
        {
            reflectMagnitude = 0.25f;
        }
        Enemy.instance.TakeDamage((int)(damageVal*reflectMagnitude),3, critChance);

        if(hp<=0)
        {
            if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Death))
            {
                hp = 0;
                TakeHeal((int)(maxHp * 0.25f), 1);
            }
            if (hp <= 0) 
            {
                isDead = true;
                BattleManager.instance.EndFight();
                animator.SetTrigger("Death");
                Debug.Log("Player Dead");
                GameManager.instance.gameState = GameState.LevelFailed;

                ScoreBoard.instance.DisplayScore();
            }
        }
    }

    protected override void AfterHeal(int delta)
    {
        UIManager.instance.PlayerStatRefresh();

        TextMesh textMesh = BattleManager.instance.damagePopUpObj.transform.GetChild(0).GetComponent<TextMesh>();
        textMesh.color = new Color(0, 255, 177, 255);
        textMesh.text = delta.ToString();
        PopUp(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), PopUpType.Healing);
        //Instantiate(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
    }

    public override void EquipItem(Equipment item)
    {
        int oldValue = 0;
        Attributes oldAttr = Attributes.Strength;
        List<MinorAttribute> oldMinorAttr = new List<MinorAttribute>();

        switch (item.equipmentType)
        {
            case Type.Helmat:
                if (helmat != null)
                {
                    oldValue = helmat.attributeValue;
                    oldAttr = helmat.equipmentAttributes;
                    oldMinorAttr = helmat.minorAttributes;
                }
                helmat = item;
                break;
            case Type.Chestplate:
                if (chestplate != null)
                {
                    oldValue = chestplate.attributeValue;
                    oldAttr = chestplate.equipmentAttributes; ;
                    oldMinorAttr = chestplate.minorAttributes;
                }
                chestplate = item;
                break;
            case Type.Gauntlet:
                if (gauntlet != null)
                {
                    oldValue = gauntlet.attributeValue;
                    oldAttr = gauntlet.equipmentAttributes;
                    oldMinorAttr = gauntlet.minorAttributes;
                }
                gauntlet = item;
                break;
            case Type.Greaves:
                if (greaves != null)
                {
                    oldValue = greaves.attributeValue;
                    oldAttr = greaves.equipmentAttributes;
                    oldMinorAttr = greaves.minorAttributes;
                }
                greaves = item;
                break;
            case Type.Axe:
            case Type.Hammer:
            case Type.Sword:
                {
                    if (weapon != null)
                    {
                        oldValue = weapon.attributeValue;
                        oldAttr = weapon.equipmentAttributes; ;
                        oldMinorAttr = weapon.minorAttributes;
                    }
                    weapon = item;
                    break;
                }
            default:
                Debug.Log("FAILURE");
                break;
        }
        AttributeCheck(oldValue, oldAttr, oldMinorAttr, item);
    }

    private void AttributeCheck(int oldAttributeValue, Attributes oldAttributeType, List<MinorAttribute> oldMinorAttributes, Equipment newItem)
    {
        //Remove old item attribute value
        switch (oldAttributeType)
        {
            case Attributes.Strength:
                strength -= oldAttributeValue;
                break;
            case Attributes.Intelligence:
                intelligence -= oldAttributeValue;
                break;
            case Attributes.Dexterity:
                dexterity -= oldAttributeValue;
                break;
            case Attributes.MaxHp:
                maxHp -= oldAttributeValue;
                if (hp >= maxHp)
                    hp = maxHp;
                break;
            case Attributes.Armor:
                armor -= oldAttributeValue;
                break;
            case Attributes.MagicResistent:
                magicResistent -= oldAttributeValue;
                break;
        }
        //Remove old item minorAttribute;
        for (int i = 0; i < oldMinorAttributes.Count; i++)
        {
            switch (oldMinorAttributes[i].minorAttributeType)
            {
                case MinorAttributeType.Armor:
                    armor /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.BaseDamage:
                    baseDamage /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.CritChance:
                    critChance /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.DodgeRate:
                    dodgeRate /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.Faith:
                    faith /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.Luck:
                    luck /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.MagicResistent:
                    magicResistent /= 1 + oldMinorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.MaxHp:
                    maxHp /= 1 + oldMinorAttributes[i].value / 100;
                    break;
            }
        }
        switch (newItem.equipmentAttributes)
        {
            case Attributes.Strength:
                strength += newItem.attributeValue;
                break;
            case Attributes.Intelligence:
                intelligence += newItem.attributeValue;
                break;
            case Attributes.Dexterity:
                dexterity += newItem.attributeValue;
                break;
            case Attributes.MaxHp:
                maxHp += newItem.attributeValue;
                hp += newItem.attributeValue;
                break;
            case Attributes.Armor:
                armor += newItem.attributeValue;
                break;
            case Attributes.MagicResistent:
                magicResistent += newItem.attributeValue;
                break;
        }

        for (int i = 0; i < newItem.minorAttributes.Count; i++)
        {
            switch (newItem.minorAttributes[i].minorAttributeType)
            {
                case MinorAttributeType.Armor:
                    armor *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.BaseDamage:
                    baseDamage *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.CritChance:
                    critChance *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.DodgeRate:
                    dodgeRate *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.Faith:
                    faith *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.Luck:
                    luck *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.MagicResistent:
                    magicResistent *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
                case MinorAttributeType.MaxHp:
                    maxHp *= 1 + newItem.minorAttributes[i].value / 100;
                    break;
            }
        }
    }
}