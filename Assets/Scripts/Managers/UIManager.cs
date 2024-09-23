using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Player player;

    public SpriteRenderer playerSprite;

    [Header("Panels")]
    public GameObject mainCanvas;
    public GameObject playerPanel;
    public GameObject restPanel;
    public GameObject artifactDropPanel;
    public GameObject battleArtifactPanel;
    public GameObject normalArtifactPanel;
    public GameObject artifactMoneyPanel;
    public GameObject pauseMenu;
    public GameObject endPanel;
    public GameObject questionPanel;

    [Header("Pause")]
    public GameObject pauseStatObj;

    [Header("Chest")]
    public GameObject chestPanel;
    public Text chestMoneyText;

    [Header("MoneyDrop")]
    public GameObject moneyPanel;
    public Text moneyText;

    [Header("StatDisplay")]
    public GameObject battlePanel;
    public GameObject playerStatPanel;
    public Text playerName;
    public Slider playerHP;
    public Slider playerShield;
    public GameObject enemyStatPanel;
    public Text enemyName;
    public Slider enemyHP;
    public Slider enemyShield;
    public float tweenTime;

    //[Header("Event")]
    //public GameObject eventPanel;
    //public Text eventButtonText;PAUSE
    //public Text eventName;
    //public Text eventDescription;
    //public GameObject AcceptButton;
    //public GameObject DenialButton;
    //public GameObject ConfirmButton;

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

    private void Start()
    {
        player = Player.instance;
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    public void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (!pauseMenu.activeSelf)
        {
            pauseStatObj.transform.GetChild(0).GetComponent<Text>().text = player.characterName;
            pauseStatObj.transform.GetChild(1).GetComponent<Text>().text = player.hp.ToString();
            pauseStatObj.transform.GetChild(2).GetComponent<Text>().text = player.strength.ToString();
            pauseStatObj.transform.GetChild(3).GetComponent<Text>().text = player.intelligence.ToString();
            pauseStatObj.transform.GetChild(4).GetComponent<Text>().text = player.dexterity.ToString();
            pauseStatObj.transform.GetChild(5).GetComponent<Text>().text = player.armor.ToString();
            pauseStatObj.transform.GetChild(6).GetComponent<Text>().text = player.magicResistent.ToString();
            pauseStatObj.transform.GetChild(7).GetComponent<Text>().text = player.dodgeRate.ToString();
            pauseStatObj.transform.GetChild(8).GetComponent<Text>().text = player.baseDamage.ToString();
            pauseStatObj.transform.GetChild(9).GetComponent<Text>().text = player.critChance.ToString();
            pauseStatObj.transform.GetChild(10).GetComponent<Text>().text = player.luck.ToString();
            pauseStatObj.transform.GetChild(11).GetComponent<Text>().text = player.faith.ToString();
        }
    }

    public void ReturnToMenu(bool endGame)
    {
        if(endGame)
        {
            GameManager.instance.saveData.Desave();
        }
        SceneController.instance.SceneChange(0);
    }

    //Refresh player stat display
    public void PlayerStatRefresh()
    {
        playerHP.maxValue = playerShield.maxValue = player.maxHp;
        playerStatPanel.SetActive(true);
        TweenValue(playerHP, player.hp);
        TweenValue(playerShield, player.temphp);
    }

    //Refresh enemy stat display
    public void EnemyStatRefresh()
    {
        enemyHP.maxValue = enemyShield.maxValue = Enemy.instance.maxHp;
        enemyStatPanel.SetActive(true);
        TweenValue(enemyHP, Enemy.instance.hp);
        TweenValue(enemyShield, Enemy.instance.temphp);
    }

    public void OpenQuestionPanel()
    {
        questionPanel.SetActive(true);
    }

    public void CloseQuestionPanel()
    {
        questionPanel.SetActive(false);
    }

    public void TweenPanelAlpha(CanvasGroup Cg, TweenType Type, float time)
    {
        switch (Type)
        {
            case TweenType.Start:
                LeanTween.value(gameObject, 0f, 1f, time).setOnUpdate((float val) =>
                {
                    Cg.alpha = val;
                });
                Cg.blocksRaycasts = true;
                Cg.interactable = true;
                break;
            case TweenType.End:
                LeanTween.value(gameObject, 1f, 0f, time).setOnUpdate((float val) =>
                {
                    Cg.alpha = val;
                });
                Cg.blocksRaycasts = false;
                Cg.interactable = false;
                break;
        }
    }

    //////////////////
    //Tween down panel
    //////////////////
    public void TweenDownPanel(RectTransform rectTransform, float finalY, float tweenTime)
    {
        LeanTween.moveY(rectTransform, finalY, tweenTime).setEase(LeanTweenType.linear);
    }

    private void TweenValue(Slider slider, int finalVal)
    {
        LeanTween.value(gameObject, slider.value, finalVal, tweenTime).setEase(LeanTweenType.linear).setOnUpdate((float val)=>
        {
            slider.value = val;
        });
    }

    //////////////////////////
    //Tween any panel to 1,1,1
    //////////////////////////
    public void TweenPanelScale(GameObject panel)
    {
        LeanTween.scale(panel, Vector3.one, 1f);
    }
}

public enum TweenType
{
    Start,
    End
}