using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour
{
    public static SlotPanel instance;
    private GameManager gm;

    public Sprite[] characterSprites;
    public Slot[] slots;
    public GameObject panel;
    public int actionNumber;
    //0 for new game;
    //1 for load save;

    [System.Serializable]
    public class Slot
    {
        public Image icon;
        public Button button;
        public Text roomNumber;
    }

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
        gm = GameManager.instance;
    }

    private void PanelRefresh()
    {
        for (int i=0;i<slots.Length;i++)
        {
            if(gm.saveData.saveSlot[i].characterName != null && gm.saveData.saveSlot[i].characterName != "")
            {
                slots[i].roomNumber.text = "Room: " + gm.saveData.saveSlot[i].roomProgress.ToString();
                slots[i].icon.gameObject.SetActive(true);
            }
            else
            {
                switch(actionNumber)
                {
                    case 0:
                        slots[i].button.interactable = true;
                        break;
                    case 1:
                        slots[i].button.interactable = false;
                        break;
                }
                slots[i].roomNumber.text = "Empty Slot";
            }
            slots[i].icon.gameObject.SetActive(true);
            switch (gm.saveData.saveSlot[i].characterName)
            {
                case "Swordsman":
                    slots[i].icon.sprite = characterSprites[0];
                    break;
                case "Archer":
                    slots[i].icon.sprite = characterSprites[1];
                    break;
                case "Magician":
                    slots[i].icon.sprite = characterSprites[2];
                    break;
                case "Thief":
                    slots[i].icon.sprite = characterSprites[3];
                    break;
                case "Priest":
                    slots[i].icon.sprite = characterSprites[4];
                    break;
                case "Warden":
                    slots[i].icon.sprite = characterSprites[5];
                    break;
                default:
                    slots[i].icon.gameObject.SetActive(false);
                    //slots[i].icon.sprite = characterSprites[6];
                    break;
             }
        }
    }

    public void OnClick(int buttonIdx)
    {
        panel.SetActive(false);
        switch(actionNumber)
        {
            case 0:
                gm.saveData.loadType = LoadType.NewGame;
                gm.SaveData();
                gm.LoadNewGame(buttonIdx);
                
                break;
            case 1:
                gm.saveData.loadType = LoadType.LoadGame;
                gm.SaveData();
                gm.LoadGame(buttonIdx);
                break;
            default:
                Debug.Log("failure");
                break;
        }
    }

    public void OpenPanel(int actionIdx)
    {
        actionNumber = actionIdx;
        PanelRefresh();
        panel.SetActive(true);
        StartUIManager.instance.StartButtonActive(false);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        StartUIManager.instance.StartButtonActive(true);
    }
}
