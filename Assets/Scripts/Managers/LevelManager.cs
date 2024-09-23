using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Temporary Statistic")]
    public int tempScore;
    public int tempKillCount;
    public int tempBossKillCount;
    public int tempDamageDealt;
    public int tempHighestDamageDealt;
    public int tempDamageTaken;
    public int tempMoneyCollected;

    public Player player;
    public Equipment[] tempEquipment = new Equipment[5];

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

    public void StatisticUpdate()
    {
        GameManager.instance.LoadData();
        SaveData.SaveSlot saveSlot = GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot];
        if (player == null)
            player = Player.instance;
        
        saveSlot.score += tempScore;
        saveSlot.killCount += tempKillCount;
        saveSlot.bossKillCount += tempBossKillCount;
        saveSlot.damageDealt += tempDamageDealt;
        saveSlot.highestDamageDealt += tempHighestDamageDealt;
        saveSlot.damageTaken += tempDamageTaken;
        saveSlot.moneyCollected += tempMoneyCollected;
        GameManager.instance.saveData.totalMoneyCollected += tempMoneyCollected;

        saveSlot.hp = player.hp;
        saveSlot.skills.Clear();
        for (int i = 0; i < player.skills.Count; i++)
        {
            saveSlot.skills.Add(player.skills[i].id);
        }

        tempEquipment[0] = player.helmat;
        tempEquipment[1] = player.chestplate;
        tempEquipment[2] = player.gauntlet;
        tempEquipment[3] = player.greaves;
        tempEquipment[4] = player.weapon;

        for (int i = 0; i < 5; i++)
        {
            if (tempEquipment[i] == null)
                continue;
            saveSlot.equipments[i] = new SaveData.EquipmentSave
            {
                id = tempEquipment[i].id,
                itemName = tempEquipment[i].itemName,
                description = tempEquipment[i].description,
                equipmentType = tempEquipment[i].equipmentType,
                attributeValue = tempEquipment[i].attributeValue,
                equipmentAttributes = tempEquipment[i].equipmentAttributes,
                minorAttributes = tempEquipment[i].minorAttributes
            };
        }

        saveSlot.artifacts.Clear();
        foreach (Artifacts a in player.artifacts)
        {
            SaveData.ArtifactSave temp = new SaveData.ArtifactSave
            {
                artifactType = a.artifactType,
                used = a.used
            };
            saveSlot.artifacts.Add(temp);
        }
        GameManager.instance.SaveData();
    }

    public void StatisticClear()
    {
        tempScore = 0;
        tempKillCount = 0;
        tempBossKillCount = 0;
        tempDamageDealt = 0;
        tempHighestDamageDealt = 0;
        tempDamageTaken = 0;
        tempMoneyCollected = 0;
    }
}
