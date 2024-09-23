using UnityEngine;

public class SkillDataBase : MonoBehaviour
{
    public static SkillDataBase instance;

    public Skill[] damageSkillDB;
    public Skill[] healingSkillDB;
    public Skill[] statusSkillDB;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("two singleton failure");
        }
    }
}
