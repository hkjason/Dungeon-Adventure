using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Player player;

    public Skill[] skills;
    public Skill newSkill;

    public GameObject skillPanel;
    public GameObject[] currentSkillObjs;
    public GameObject newSkillObj;
    public Text title;
    public GameObject skillDescriptionBox;
    public Text skillDescriptionBoxNameText;
    public Text skillDescriptionBoxDescriptionText;

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

    private void Start()
    {
        player = Player.instance;
    }

    public void SkillDescriptionBoxOn(int skillIndex)
    {
        if (skillIndex < player.skills.Count)
        {
            skillDescriptionBox.SetActive(true);
            skillDescriptionBoxNameText.text = player.skills[skillIndex].skillName;
            skillDescriptionBoxDescriptionText.text = player.skills[skillIndex].skillDescription;
            skillDescriptionBox.transform.position = currentSkillObjs[skillIndex].transform.position - new Vector3(0, 100, 0);
        }
    }

    public void SkillDescriptionBoxOff()
    {
        skillDescriptionBox.SetActive(false);
    }

    public void SkillDeliver()
    {
        int tempSkillPointer = Random.Range(0, skills.Length);
        SkillDisplay(skills[tempSkillPointer]);
    }

    private void SkillDisplay(Skill skill)
    {
        newSkill = skill;
        title.text = "You discovered " + skill.skillName;
        //foreach (GameObject obj in currentSkillObjs)
        //{
        //    obj.SetActive(false);
        //}
        for (int i = 0; i < player.skills.Count; i++)
        {
            //currentSkillObjs[i].SetActive(true);
            if(player.skills[i] != null)
            {
                Image img = currentSkillObjs[i].GetComponent<Image>();
                img.enabled = true;
                img.sprite = player.skills[i].skillIcon;
                currentSkillObjs[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = player.skills[i].skillName;
            }

        }
        newSkillObj.GetComponent<Image>().sprite = skill.skillIcon;
        newSkillObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = skill.skillName + '\n' + skill.skillDescription;
        skillPanel.SetActive(true);
    }

    public void SkillChange(int skillIndex)
    {
        if (skillIndex >= player.skills.Count)
        {
            player.skills.Add(newSkill);
        }
        else
        {
            player.skills[skillIndex] = newSkill;
        }
        skillPanel.SetActive(false);
        //go to next room
        RoomManager.instance.CheckRoomEnd();
    }

    public void BattleEndSkillIgnore()
    {
        skillPanel.SetActive(false);
        //go to next room
        RoomManager.instance.CheckRoomEnd();
    }
}
