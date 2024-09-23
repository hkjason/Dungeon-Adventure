using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public static Enemy instance;

    public int ActionCD;
    public int ActionCounter;

    public float normalAttackClickCDMultiplyer = 1;
    public List<float> skillSelectionFloat;

    public GameObject[] effectPosition;

    private EnemyType _currentEnemyType;

    private int _goldDropAmount;

    [SerializeField]
    private float _statLimitBound = 0.05f;

    private void Awake()
    {
        if (instance == null)
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
        gameObject.SetActive(false);
    }

    public void EnemyActionCheck()
    {
        if (!isDead)
        {
            if (ActionCounter > 1)
            {
                ActionCounter--;
            }
            else
            {
                //action
                float tempActionFloat = Random.Range(0, 1f);
                float tempSkillChance = 0;
                if (skillSelectionFloat != null && skillSelectionFloat.Count != 0)
                {
                    for (int i = 0; i < skillSelectionFloat.Count; i++)
                    {
                        tempSkillChance += skillSelectionFloat[i];
                    }
                }
                if (tempActionFloat < tempSkillChance) 
                {
                    for (int i = 0; i < skills.Count; i++)
                    {
                        if (i==0 && tempActionFloat <= skillSelectionFloat[i])
                        {
                            EnemySkillCast(skills[i]);
                        }
                        else if (i != 0 && tempActionFloat > skillSelectionFloat[i-1] && tempActionFloat <= skillSelectionFloat[i])
                        {
                            EnemySkillCast(skills[i]);
                        }
                    }
                }
                else
                {
                    NormalAttack(Player.instance);
                }
                ActionCounter = ActionCD;
            }
        }
    }

    private void EnemySkillCast(Skill skill)
    {
        skill.SkillCast(Player.instance, this);
    }

    protected override void AfterTakeDamage(int damageVal, PopUpType popUpType)
    {
        LevelManager.instance.tempDamageDealt += damageVal;
        GameManager.instance.LoadData();
        if (damageVal >= GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot].highestDamageDealt)
        {
            LevelManager.instance.tempHighestDamageDealt = damageVal;
        }

        UIManager.instance.EnemyStatRefresh();
        PopUp(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), popUpType);
        //Instantiate(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        if (hp <= 0)
        {
            isDead = true;
            BattleManager.instance.EndFight();

            switch (_currentEnemyType)
            {
                case EnemyType.Monster:
                    LevelManager.instance.tempKillCount++;
                    break;
                default:
                    LevelManager.instance.tempBossKillCount++;
                    break;
            }

            animator.SetTrigger("Death");
            Debug.Log("Enemy Dead");

            MoneyDrop();
        }
    }

    private void MoneyDrop()
    {
        UIManager.instance.moneyPanel.SetActive(true);
        _goldDropAmount = Random.Range(1, 100);
        UIManager.instance.moneyText.text = "Gold Found: " + _goldDropAmount.ToString() + " !";
    }

    public void MoneyDropEnd()
    {
        UIManager.instance.moneyPanel.SetActive(false);
        LevelManager.instance.tempMoneyCollected += _goldDropAmount;
        Drop();
    }

    private void Drop()
    {
        int temp = Random.Range(0, 10);
        if (temp <= 4)
        {
            List<Equipment> drop = new List<Equipment>();
            bool[] dropped = new bool[] { false, false, false, false, false };
            int dropPercentage = 100;
            List<int> l = new List<int>{0,1,2,3,4};
            List<int> list = RandomOrder.RandomOrderGenerator<int>(l);
            for (int i=0;i<5;i++)
            {
                int tempEquipmentPointer = list[0];
                list.RemoveAt(0);
                int randomInt = Random.Range(0, 100);
                if (randomInt >= dropPercentage)
                    continue;
                dropPercentage /= 2;
                switch (tempEquipmentPointer)
                {
                    case 0:
                        if (!dropped[0] && helmat != null)
                        {
                            drop.Add(helmat);
                            dropped[0] = true;
                        }
                        break;
                    case 1:
                        if (!dropped[1] && chestplate != null)
                        {
                            drop.Add(chestplate);
                            dropped[1] = true;
                        }
                        break;
                    case 2:
                        if (!dropped[2] && gauntlet != null)
                        {
                            drop.Add(gauntlet);
                            dropped[2] = true;
                        }
                        break;
                    case 3:
                        if (!dropped[3] && greaves != null)
                        {
                            drop.Add(greaves);
                            dropped[3] = true;
                        }
                        break;
                    case 4:
                        if (!dropped[4] && weapon != null)
                        {
                            drop.Add(weapon);
                            dropped[4] = true;
                        }
                        break;
                    default:
                        Debug.Log("FAILURE");
                        break;
                }
            }
            //Handle if nothing dropped
            while (drop.Count <= 0)
            {
                int tempEquipmentPointer = Random.Range(0, 5);
                switch (tempEquipmentPointer)
                {
                    case 0:
                        if (helmat != null)
                            drop.Add(helmat);
                        break;
                    case 1:
                        if (chestplate != null)
                            drop.Add(chestplate);
                        break;
                    case 2:
                        if (gauntlet != null)
                            drop.Add(gauntlet);
                        break;
                    case 3:
                        if (greaves != null)
                            drop.Add(greaves);
                        break;
                    case 4:
                        if (weapon != null)
                            drop.Add(weapon);
                        break;
                }
            }
            ItemManager.instance.Display(drop);
        }
        else
        {
            SkillManager.instance.SkillDeliver();
        }
    }

    protected override void AfterHeal(int delta)
    {
        PopUp(BattleManager.instance.damagePopUpObj, transform.position + new Vector3(0, 1.2f, 0), PopUpType.Healing);
        UIManager.instance.EnemyStatRefresh();
    }

    public void SpawnEnemy(RoomType enemyType)
    {
        skills.Clear();
        skillSelectionFloat.Clear();
        helmat = null;
        chestplate = null;
        gauntlet = null;
        greaves = null;
        weapon = null;

        int enemyTypeNumber = 0;
        switch(enemyType)
        {
            case RoomType.Monster:
                _currentEnemyType = EnemyType.Monster;
                int rng = Random.Range(0, 10);
                if (rng < 5)
                    enemyTypeNumber = 0;    //Weak
                else if (rng < 9)
                    enemyTypeNumber = 1;    //Moderate
                else
                    enemyTypeNumber = 2;    //Strong
                break;
            case RoomType.MiniBoss:
                _currentEnemyType = EnemyType.MiniBoss;
                break;
            case RoomType.Boss:
                _currentEnemyType = EnemyType.Boss;
                break;
            default:
                Debug.Log("FAILURE");
                break;
        }
        isDead = false;

        int floor = (RoomManager.instance.currentRoomLevel - 1)/ 5;
        int enemyNumber = Random.Range(0, EnemyDB.instance.enemyDataBase[floor].typeDatas[enemyTypeNumber].enemyDatas.Length);
        EnemyData enemyData = EnemyDB.instance.enemyDataBase[floor].typeDatas[enemyTypeNumber].enemyDatas[enemyNumber];

        animator.runtimeAnimatorController = enemyData.animationController;
        for(int i=0;i<effectPosition.Length;i++)
        {
            effectPosition[i].transform.localPosition = enemyData.effectPositions[i];
        }
        characterName = enemyData.enemyName;
        maxHp = enemyData.hp;
        hp = maxHp;
        strength = enemyData.strength;
        intelligence = enemyData.intelligence;
        dexterity = enemyData.dexterity;
        armor = enemyData.armor;
        magicResistent = enemyData.magicResistent;
        dodgeRate = enemyData.dodgeRate;
        baseDamage = enemyData.baseDamage;
        critChance = enemyData.critChance;
        luck = enemyData.luck;
        faith = enemyData.faith;

        skills = enemyData.skills;
        skillSelectionFloat = enemyData.skillsChance;

        maxHp = (int)(maxHp * EnemyRandomStatNumber());
        hp = maxHp;
        strength = (int)(strength * EnemyRandomStatNumber());
        intelligence = (int)(intelligence * EnemyRandomStatNumber());
        dexterity = (int)(dexterity * EnemyRandomStatNumber());
        armor = (int)(armor * EnemyRandomStatNumber());
        magicResistent = (int)(magicResistent * EnemyRandomStatNumber());
        dodgeRate = (int)(dodgeRate * EnemyRandomStatNumber());
        baseDamage = (int)(baseDamage * EnemyRandomStatNumber());
        critChance = (int)(critChance * EnemyRandomStatNumber());
        luck = (int)(luck * EnemyRandomStatNumber());
        faith = (int)(faith * EnemyRandomStatNumber());

        if (ArtifactManager.instance.ArtifactCheck(ArtifactType.EnemyCD))
        {
            normalAttackClickCDMultiplyer = 1.5f;
        }
        ActionCD = ActionCounter = (int)(enemyData.cd * normalAttackClickCDMultiplyer);
        /////////////
        /// Equip ///
        ////////////
        List<Equipment> tempList = ItemManager.instance.EnemyEquipmentGenerator();
        for (int i = 0; i < tempList.Count; i++)
        {
            EquipItem(tempList[i]);
        }
        UIManager.instance.EnemyStatRefresh();
    }

    public override void EquipItem(Equipment item)
    {
        switch (item.equipmentType)
        {
            case Type.Helmat:
                helmat = item;
                break;
            case Type.Chestplate:
                chestplate = item;
                break;
            case Type.Gauntlet:
                gauntlet = item;
                break;
            case Type.Greaves:
                greaves = item;
                break;
            case Type.Axe:
            case Type.Hammer:
            case Type.Sword:
                weapon = item;
                break;
        }
        AttributeCheck(item);
    }

    private void AttributeCheck(Equipment newItem)
    {
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

    private float EnemyRandomStatNumber()
    {
        return Random.Range(1f - _statLimitBound, 1f + _statLimitBound);
    }

    private enum EnemyType
    { 
        Monster,
        MiniBoss,
        Boss
    }    
}
