using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public Player player;
    public SaveData.SaveSlot saveSlot;

    public Image[] roomImage;
    public GameObject midPanel;

    public Button[] roomButton;
    public RoomType[] leftRoom;
    public RoomType[] midRoom;
    public RoomType[] rightRoom;

    private const int _roomSize = 2;
    public int currentSize;
    public int selectedRoom;

    public Sprite[] roomSprite;

    public int roomTypeNumber;
    public int idx = 0;

    public float transitionTime;

    public int currentRoomLevel;
    public Text roomNumberText;

    public int chestGoldAmount;

    [Header("RestPanelVaraible")]
    public Text restPanelText;
    private readonly string[] restStrings = { "You rested well", "That was a tough night", "Warm and cozy campfire" };

    [Header("Tween setting")]
    public CanvasGroup roomCanvasGroup;
    public float tweenTime;
    public Text healthText;
    public Slider healthBar;
    public Slider shieldBar;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.Log("Two singleton failure");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Player.instance;
        roomNumberText.text = "Room: " + currentRoomLevel;
        leftRoom = new RoomType[_roomSize];
        midRoom = new RoomType[_roomSize];
        rightRoom = new RoomType[_roomSize];

        GameManager.instance.LoadData();
        saveSlot = GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot];
        switch (GameManager.instance.saveData.loadType)
        {
            case LoadType.NewGame:
                RoomGen();
                break;
            case LoadType.LoadGame:
                RoomLoad();
                break;
            default:
                Debug.Log("failure");
                break;
        }
    }

    private void CharacterStatusBarRefresh()
    {
        healthText.text = (player.temphp + player.hp).ToString() + "/" + player.maxHp.ToString();
        healthBar.value = (float)player.hp / (float)player.maxHp;
        shieldBar.value = (float)player.temphp / (float)player.maxHp;
    }
    //GenerateRooms
    private void RoomGen()
    {
        LevelManager.instance.StatisticClear();
        saveSlot.roomProgress = currentRoomLevel;

        UIManager.instance.TweenPanelAlpha(roomCanvasGroup, TweenType.Start, tweenTime);

        foreach (Button button in roomButton)
        {
            button.interactable = true;
        }
        if(currentRoomLevel%20 != 0)
        {
            GenerateNormalRoom();
        }
        else if(currentRoomLevel == 0)
        {
            GenerateNormalRoom();
        }
        else
        {
            GenerateBossRoom();
        }
        GameManager.instance.SaveData();
    }

    private void GenerateNormalRoom()
    {
        UIManager.instance.TweenPanelAlpha(roomCanvasGroup, TweenType.Start, tweenTime);
        ////////////////////////////////////
        //For Monster Testing///////////////
        ////////////////////////////////////
        //midPanel.SetActive(true);
        //saveSlot.midRoom[0] = midRoom[0] = RoomType.Empty;
        //saveSlot.rightRoom[0] = rightRoom[0] = RoomType.Monster;
        //saveSlot.leftRoom[0] = leftRoom[0] = RoomType.Monster;
        //saveSlot.midRoom[1] = midRoom[1] = RoomType.End;
        //saveSlot.rightRoom[1] = rightRoom[1] = RoomType.Empty;
        //saveSlot.leftRoom[1] = leftRoom[1] = RoomType.Empty;
        //DisplayAllRooms(); 

        int[] firstRoomNumber = new int[3];
        int[] randomRoomNumber = new int[3];
        for (int i=0;i<_roomSize;i++)
        {
            midPanel.SetActive(true);
            if (i == 0)
            {
                int x = firstRoomNumber[0] = RandomRoomNumber();
                saveSlot.leftRoom[i] = leftRoom[i] = (RoomType)x;
                x = firstRoomNumber[1] = RandomRoomNumber();
                saveSlot.rightRoom[i] = rightRoom[i] = (RoomType)x;
                x = firstRoomNumber[2] = RandomRoomNumber();
                saveSlot.midRoom[i] = midRoom[i] = (RoomType)x;
            }
            if (i == 1)
            {
                do
                {
                    int x = randomRoomNumber[0] = RandomRoomNumber();
                    saveSlot.leftRoom[i] = leftRoom[i] = (RoomType)x;
                } while (firstRoomNumber[0] == randomRoomNumber[0]);
                do
                {
                    int x = randomRoomNumber[1] = RandomRoomNumber();
                    saveSlot.rightRoom[i] = rightRoom[i] = (RoomType)x;
                } while (firstRoomNumber[1] == randomRoomNumber[1]);
                do
                {
                    int x = randomRoomNumber[2] = RandomRoomNumber();
                    saveSlot.midRoom[i] = midRoom[i] = (RoomType)x;
                } while (firstRoomNumber[2] == randomRoomNumber[2]);
            }
        }
        DisplayAllRooms(); 
    }

    private void GenerateBossRoom()
    {
        midPanel.SetActive(false);
        saveSlot.leftRoom[0] = leftRoom[0] = RoomType.MiniBoss;
        saveSlot.midRoom[0] = midRoom[0] = RoomType.Empty;
        saveSlot.rightRoom[0] = rightRoom[0] = RoomType.Boss;
        saveSlot.leftRoom[1] = leftRoom[1] = RoomType.Empty;
        saveSlot.midRoom[1] = midRoom[1] = RoomType.Empty;
        saveSlot.rightRoom[1] = rightRoom[1] = RoomType.End;
        DisplayAllRooms();
    }

    private void DisplayAllRooms()
    {
        CharacterStatusBarRefresh();
        for (int i=0;i<_roomSize;i++)
        {
            RoomDisplay(leftRoom[i]);
            RoomDisplay(midRoom[i]);
            RoomDisplay(rightRoom[i]);
        }
    }

    private void RoomLoad()
    {
        LevelManager.instance.StatisticClear();

        GameManager.instance.LoadData();
        currentRoomLevel = saveSlot.roomProgress;
        roomNumberText.text = "Room: " + currentRoomLevel;

        for(int i=0; i<_roomSize;i++)
        {
            midRoom[i] = saveSlot.midRoom[i];
            rightRoom[i] = saveSlot.rightRoom[i];
            leftRoom[i] = saveSlot.leftRoom[i];
        }

        DisplayAllRooms();

        if(currentRoomLevel%20 ==0 && currentRoomLevel!=0)
        {
            midPanel.SetActive(false);
        }

        UIManager.instance.TweenPanelAlpha(roomCanvasGroup, TweenType.Start, tweenTime);

        foreach (Button button in roomButton)
        {
            button.interactable = true;
        }
    }

    //Display rooms in panel
    private void RoomDisplay(RoomType room)
    {
        roomImage[idx].enabled = true;
        switch(room)
        {
            case RoomType.Monster:
                roomImage[idx].sprite = roomSprite[0];
                break;
            case RoomType.Rest:
                roomImage[idx].sprite = roomSprite[1];
                break;
            case RoomType.Chest:
                roomImage[idx].sprite = roomSprite[2];
                break;
            case RoomType.Event:
                roomImage[idx].sprite = roomSprite[3];
                break;
            case RoomType.Boss:
                roomImage[idx].sprite = roomSprite[4];
                break;
            case RoomType.MiniBoss:
                roomImage[idx].sprite = roomSprite[5];
                break;
            case RoomType.End:
                roomImage[idx].sprite = roomSprite[6];
                break;
            case RoomType.Empty:
                roomImage[idx].enabled = false;
                break;
        }
        idx++;
        if(idx==3*_roomSize)
            idx=0;
    }

    ///////////////////////////////////////
    //EnterRoom called by selectRoomButtons
    //Pass in room index///////////////////
    //(top =0, left =1, right =2)//////////
    ///////////////////////////////////////
    public void EnterRoom(int roomID)
    {
        UIManager.instance.TweenPanelAlpha(roomCanvasGroup, TweenType.End, tweenTime);
        currentRoomLevel++;

        selectedRoom = roomID;
        currentSize = 0;
        foreach(Button button in roomButton)
        {
            button.interactable = false;
        }
        StartCoroutine(Fade(selectedRoom));
    }

    ///////////////////////////////////////////
    //Check which type of room is being entered
    ///////////////////////////////////////////
    private void CheckRoomType(RoomType type)
    {
        switch(type)
        {
            case RoomType.Monster:
                BattleManager.instance.StartBattle(RoomType.Monster);
                break;
            case RoomType.Rest:
                StartRest();
                break;
            case RoomType.Chest:
                StartChest();
                break;
            case RoomType.Event:
                EventManager.instance.EventDisplay();
                break;
            case RoomType.Boss:
                BattleManager.instance.StartBattle(RoomType.Boss);
                //currentSize++;
                break;
            case RoomType.MiniBoss:
                BattleManager.instance.StartBattle(RoomType.MiniBoss);
                //currentSize++;
                break;
            case RoomType.End:
                UIManager.instance.endPanel.SetActive(true);
                break;
            case RoomType.Empty:
                CheckRoomEnd();
                break;
        }
    }

    private void StartChest()
    {
        UIManager.instance.chestPanel.SetActive(true);
        chestGoldAmount = Random.Range(1, 50);
        UIManager.instance.chestMoneyText.text = "Gain: " + chestGoldAmount.ToString();
    }

    ///////////////////////////////////
    //EndChest called when Chest finish
    ///////////////////////////////////
    public void EndChest()
    {
        LevelManager.instance.tempMoneyCollected += chestGoldAmount;
        UIManager.instance.chestPanel.SetActive(false);
        CheckRoomEnd();
    }

    //////////////
    /// StartRest
    /////////////
    private void StartRest()
    {
        restPanelText.text = restStrings[Random.Range(0, restStrings.Length)];
        UIManager.instance.restPanel.SetActive(true);
    }

    /////////////////////////////////
    //EndRest called when Rest finish
    /////////////////////////////////
    public void EndRest()
    {
        player.TakeHeal((int)(player.maxHp * 0.25f),1);
        UIManager.instance.playerStatPanel.SetActive(false);
        UIManager.instance.restPanel.SetActive(false);
        CheckRoomEnd();
    }

    ///////////////////////////////////
    //EndEvent called when Event finish
    ///////////////////////////////////
    public void EndEvent()
    {
        EventManager.instance.eventPanel.SetActive(false);
        CheckRoomEnd();
    }
    
    //////////////////////////////////////
    //Check if everything in room is shown
    //////////////////////////////////////
    public void CheckRoomEnd()
    {
        if (currentSize < _roomSize - 1)
        {
            currentSize++;
            StartCoroutine(Fade(selectedRoom));
        }
        else
        {
            LevelManager.instance.StatisticUpdate();
            LevelManager.instance.StatisticClear();
            RoomGen();
        }
    }

    private int RandomRoomNumber()
    {
        int x = Random.Range(0, 10);
        switch (x)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                {
                    x = 0;
                    break;
                }
            case 6:
                {
                    x = Random.Range(1, 4);
                    break;
                }
            case 7:
                {
                    x = 1;
                    break;
                }
            case 8:
                {
                    x = 2;
                    break;
                }
            case 9:
                {
                    x = 3;
                    break;
                }
        }
        return x;
    }

    ////////////////////////////////
    //Start fading into another Room
    ////////////////////////////////
    //Pass in selected room index///
    //(top =0, left =1, right =2)///
    ////////////////////////////////
    private IEnumerator Fade(int idx)
    {
        StartUIManager.instance.StartFade();

        yield return new WaitForSeconds(transitionTime);

        Enemy.instance.gameObject.SetActive(false);
        roomNumberText.text = "Room: " + currentRoomLevel;

        switch(idx)
        {
            case 0:
                CheckRoomType(leftRoom[currentSize]);
                break;
            case 1:
                CheckRoomType(midRoom[currentSize]);
                break;
            case 2:
                CheckRoomType(rightRoom[currentSize]);
                break;
        }
    }
}

public enum RoomType
{
    Monster,
    Rest,
    Chest,
    Event,
    MiniBoss,
    Boss,

    End = 98,
    Empty = 99
}