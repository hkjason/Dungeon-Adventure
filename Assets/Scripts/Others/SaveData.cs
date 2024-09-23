using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public class SaveSlot
    {
        public string characterName = "";

        public int maxhp;
        public int hp;
        public int temphp;

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
        public string normalAttack;
        public List<int> skills = new List<int>();
        public List<ArtifactSave> artifacts = new List<ArtifactSave>();

        public EquipmentSave[] equipments = new EquipmentSave[5];

        public int roomProgress;
        public RoomType[] leftRoom = new RoomType[2];
        public RoomType[] midRoom = new RoomType[2];
        public RoomType[] rightRoom = new RoomType[2];

        [Header("GameStatistic")]
        public int score;
        public int killCount;
        public int bossKillCount;
        public int damageDealt;
        public int highestDamageDealt;
        public int damageTaken;
        public int moneyCollected;
    }

    public SaveSlot[] saveSlot;

    public int totalMoneyCollected = 0;
    public int lastPlayedSlot = 99;
    public LoadType loadType;

   public void Initialize()
   {
        saveSlot = new SaveSlot[3];
        for (int i = 0; i < 3; i++)
        {
            saveSlot[i] = new SaveSlot();
        }

        lastPlayedSlot = 99;
   }

   public void Desave()
   {

      saveSlot[lastPlayedSlot] = new SaveSlot();
      lastPlayedSlot = 99;
      GameManager.instance.SaveData();
   }

    [System.Serializable]
    public class EquipmentSave
    {
        public string itemName = "";
        public string description = "";

        public int id;
        public int attributeValue;

        public Type equipmentType;
        public Attributes equipmentAttributes;

        public List<MinorAttribute> minorAttributes = new List<MinorAttribute>();

        //artwork not saved
    }

    [System.Serializable]
    public class ArtifactSave
    {
        public ArtifactType artifactType;
        public bool used;
    }
}
