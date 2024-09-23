using UnityEngine;

public class EnemyDB : MonoBehaviour
{
    public static EnemyDB instance;
    public EnemyDataBase[] enemyDataBase;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class EnemyDataBase
    {
        public string floorNumber;
        public FloorData[] typeDatas;
    }

    [System.Serializable]
    public class FloorData
    {
        public string dataBaseName;
        public EnemyData[] enemyDatas;
    }
}
