using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    [SerializeField]
    private int _eventIdx;

    public int eventsNumber;
    public Event[] events;

    public GameObject eventPanel;
    public Text eventName;
    public Text eventDescription;
    public Image eventImage;
    public GameObject choicePanel;
    public Image currentImg;
    public Text currentText;
    public Image newImg;
    public Text newText;

    public Equipment eventItem;

    public Status[] eventBuff;
    public Status[] eventDebuff;
    public Sprite eventDebuffSprite;

    void Awake()
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


    public int counter = 0;
    public void EventDisplay()
    {
        int tempLuckSwitch = Random.Range(0, 4);
        if (tempLuckSwitch == 0)
        {
            if (Player.instance.luck <= 25)      //only bad event
                _eventIdx = Random.Range(5, 7);
            else if (Player.instance.luck <= 50)
                _eventIdx = Random.Range(4, 7);  // more bad event
            else if (Player.instance.luck <= 75)
                _eventIdx = Random.Range(2, 7);  // more good event
            else
                _eventIdx = Random.Range(0, 5); //only good event
        }
        else
        {
            _eventIdx = Random.Range(0, 7);     // random event
        }

        if(ArtifactManager.instance.ArtifactCheck(ArtifactType.Event))
        {
            _eventIdx = Random.Range(0, 5);
        }

        switch (_eventIdx)
        {
            case 0:
                //Item drop (t1:35%  t2:25%  t3:20%  t4:15%  t5:5%)
                ItemManager.instance.EventItemDeliver();
                break;
            case 1:
                //Skill drop
                SkillManager.instance.SkillDeliver();
                break;
            case 2:
                //buff event
                BuffAddingEvent(true);
                break;
            case 3:
                ArtifactManager.instance.GenerateArtifact();
                break;
            case 4:
                //loot
                eventPanel.SetActive(true);
                eventName.text = events[_eventIdx].eventName;
                eventDescription.text = events[_eventIdx].eventDescription;
                eventImage.sprite = events[_eventIdx].eventPicture;
                break;
            case 5:
                //debuff event
                BuffAddingEvent(false);
                break;
            case 6:
                float temp = Random.Range(0.1f, 1);
                if (temp <= 0.4f)
                {
                    //money drop
                    eventPanel.SetActive(true);
                    eventName.text = events[_eventIdx].eventName;
                    eventDescription.text = events[_eventIdx].eventDescription + "You drop gold";
                    GameManager.instance.LoadData();
                    if (GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot].moneyCollected + LevelManager.instance.tempMoneyCollected <= 10)
                    {
                        LevelManager.instance.tempMoneyCollected -= GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot].moneyCollected + LevelManager.instance.tempMoneyCollected;
                    }
                    else
                        LevelManager.instance.tempMoneyCollected -= 10;
                    eventImage.sprite = events[_eventIdx].eventPicture;
                }
                else if (temp <= 0.7f)
                {
                    //battle
                    BattleManager.instance.StartBattle(RoomType.Monster);
                }
                else
                {
                    //damage
                    eventPanel.SetActive(true);
                    eventName.text = events[_eventIdx].eventName;
                    eventDescription.text = events[_eventIdx].eventDescription + "You hurt";
                    eventImage.sprite = events[_eventIdx].eventPicture;
                    Player.instance.TakeDamage((int)(Player.instance.hp * 0.1),3, 0);
                }
                break;
            default:
                Debug.Log("event not found");
                break;
        }
    }

    public void EventAccept()
    {
        switch (_eventIdx)
        {
            case 2:
                //buff ed before accept
                eventPanel.SetActive(false);
                break;
            case 3:
                ArtifactEvent();
                break;
            case 4:
                DeadBodyEvent();
                break;
            case 5:
                //debuff ed before accept
                eventPanel.SetActive(false);
                break;
            default:
                Debug.Log("event not found");
                break;
        }
        RoomManager.instance.EndEvent();
    }

    void BuffAddingEvent(bool buff)
    {
        Status tempStatus;
        if (buff)
        {
            int temp = Random.Range(0, eventBuff.Length);
            tempStatus = eventBuff[temp];
        }
        else
        {
            if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Debuff))
            {
                eventName.text = "Debuff?";
                eventDescription.text = "You can't get debuff. No worry.";
                eventImage.sprite = eventDebuffSprite;
                eventPanel.SetActive(true);
                return;
            }
            int temp = Random.Range(0, eventDebuff.Length);
            tempStatus = eventDebuff[temp];
        }
        Player.instance.status.Add(tempStatus.Deepcopy());

        eventName.text = events[_eventIdx].eventName;
        eventDescription.text = events[_eventIdx].eventDescription;
        eventImage.sprite = tempStatus.artWork;
        eventPanel.SetActive(true);
    }
    private void DeadBodyEvent()
    {
        //tba
        LevelManager.instance.tempMoneyCollected += 100;
    }
    private void ArtifactEvent()
    {
        ArtifactManager.instance.GenerateArtifact();
        eventPanel.SetActive(false);
    }
}
