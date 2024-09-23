using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public Player player;
    public Enemy enemy;

    public GameObject normalAttackButton;
    public GameObject[] skillButtons;

    [Header("Skill Description")]
    private int gridLayoutGroupSpacing;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject skillDescriptionPanel;
    public Text skillNameText;
    public Text skillDescriptionText;
    private Coroutine coroutine;

    public int currentSkillCheckIdx;
    public GameObject damagePopUpObj;

    private void Awake()
    {
        if(instance == null)
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
        enemy = Enemy.instance;

        gridLayoutGroupSpacing = (int)gridLayoutGroup.spacing.x;
    }

    private void BattleUIRefresh()
    {
        normalAttackButton.transform.GetChild(0).GetComponent<Text>().text = player.normalAttackName;
        for (int i = 0; i < player.skills.Count; i++)
        {
            skillButtons[i].SetActive(true);
            skillButtons[i].GetComponent<Image>().fillAmount = player.skillsCurrentClick[i]/player.skills[i].clickCD;
            skillButtons[i].transform.GetChild(0).GetComponent<Text>().text = player.skills[i].skillName;
            skillButtons[i].transform.GetComponent<Button>().interactable = false;
        }
        for (int i = player.skills.Count; i < 4; i++)
        {
            skillButtons[i].SetActive(false);

        }
        UIManager.instance.playerName.text = player.characterName;
        UIManager.instance.enemyName.text = enemy.characterName;
    }

    public void ClickAttack()
    {
        //player normal attack
        player.NormalAttack(Enemy.instance);

        //temphp decay
        if(player.temphp>0) 
            player.temphp-=5;
        if(player.temphp<0)
            player.temphp=0;
        if(enemy.temphp>0)
            enemy.temphp-=5;
        if(enemy.temphp<0)
            player.temphp=0;
        
        enemy.EnemyActionCheck();

        for (int i = 0; i < player.skills.Count; i++)
        {
            player.skillsCurrentClick[i]++;
            if (player.skillsCurrentClick[i] == player.skills[i].clickCD)
            {
                currentSkillCheckIdx = i;
                skillButtons[i].transform.GetComponent<Button>().interactable = true;
                if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Skill))
                    PlayerSkillAutoCast(currentSkillCheckIdx);
            }
        }
        StatusManager.instance.CheckStatus(player);
        StatusManager.instance.CheckStatus(Enemy.instance);
        if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Click))
            AutoClick();
        for (int i = 0; i < player.skills.Count; i++)
        {
            skillButtons[i].GetComponent<Image>().fillAmount = (float)player.skillsCurrentClick[i] / (float)player.skills[i].clickCD;
        }
    }

    public void AutoClick()
    {
        player.NormalAttack(enemy);

        for (int i = 0; i < player.skills.Count; i++)
        {
            player.skillsCurrentClick[i]++;
            if (player.skillsCurrentClick[i] == player.skills[i].clickCD)
            {
                currentSkillCheckIdx = i;
                skillButtons[i].transform.GetComponent<Button>().interactable = true;
                if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Skill))
                    PlayerSkillAutoCast(currentSkillCheckIdx);
            }
        }
    }

    public void PlayerSkillCast(int skillIndex)
    {
        player.skillsCurrentClick[skillIndex] = 0;
        enemy.EnemyActionCheck();
        skillButtons[skillIndex].transform.GetComponent<Button>().interactable = false;

        player.skills[skillIndex].SkillCast(enemy, player);

        if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Multicast))
            PlayerSkillMultiCast(currentSkillCheckIdx);
    }

    public void PlayerSkillAutoCast(int skillIndex)
    {
        player.skillsCurrentClick[skillIndex] = 0;
        skillButtons[skillIndex].transform.GetComponent<Button>().interactable = false;

        player.skills[skillIndex].SkillCast(enemy, player);

        if(ArtifactManager.instance.ArtifactCheck(ArtifactType.Multicast))
            PlayerSkillMultiCast(currentSkillCheckIdx);
    }

    public void PlayerSkillMultiCast(int skillIndex)
    {
        player.skillsCurrentClick[skillIndex] = 0;
        skillButtons[skillIndex].transform.GetComponent<Button>().interactable = false;

        player.skills[skillIndex].SkillCast(enemy, player);
    }

    public void StartBattle(RoomType enemyType)
    {
        UIManager.instance.battlePanel.SetActive(true);
        enemy.SpawnEnemy(enemyType);
        BattleUIRefresh();
        enemy.gameObject.SetActive(true);

        if (player.status.Count > 0)
        {
            StatusManager.instance.StatusDisplay(player);
        }
        for (int i = 0; i < player.skillsCurrentClick.Length; i++)
        {
            player.skillsCurrentClick[i] = 0;
            skillButtons[i].GetComponent<Image>().fillAmount = 0;
        }

        UIManager.instance.PlayerStatRefresh();

        UIManager.instance.playerSprite.enabled = true;
        player.BaseStatIni();
        enemy.BaseStatIni();
    }

    public void EndFight()
    {
        StatusManager.instance.ClearAll();
        UIManager.instance.battlePanel.SetActive(false);
    }

    public void SkillDescriptionEnter(int idx)
    {
        skillNameText.text = player.skills[idx].skillName;
        skillDescriptionText.text = player.skills[idx].skillDescription;

        coroutine = StartCoroutine(SkillDescriptionStart(idx));
    }

    private IEnumerator SkillDescriptionStart(int idx)
    {
        yield return new WaitForSeconds(0.15f);
        switch (idx)
        {
            case 0:
            case 3:
                skillDescriptionPanel.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case 1:
                skillDescriptionPanel.transform.localPosition = new Vector3(0 + (skillButtons[idx].GetComponent<RectTransform>().rect.width+gridLayoutGroupSpacing), 0, 0);
                break;
            case 2:
                skillDescriptionPanel.transform.localPosition = new Vector3(0 - (skillButtons[idx].GetComponent<RectTransform>().rect.width+gridLayoutGroupSpacing), 0, 0);
                break;
            default:
                Debug.Log("FAILURE");
                break;
        }
        skillDescriptionPanel.SetActive(true);
    }

    public void SkillDescriptionExit()
    {
        StopCoroutine(coroutine);
        skillDescriptionPanel.SetActive(false);
    }
}