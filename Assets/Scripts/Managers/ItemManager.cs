using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public Player player;

    public GameObject equipmentPanel;

    public Equipment newEquipment;
    public Equipment currentEquipment;

    public List<Equipment> equipmentsDrop;

    public Text currentDescriptionText;
    public Text currentItemNameText;
    public Text newDescriptionText;
    public Text newItemNameText;
    public Image currentItemIcon;
    public Image newItemIcon;

    public Image artworkImage;
    public int displayOrder;

    //Item DB
    public EquipmentArray[] t1Equipment;
    public EquipmentArray[] t2Equipment;
    public EquipmentArray[] t3Equipment;
    public EquipmentArray[] t4Equipment;
    public EquipmentArray[] t5Equipment;

    public Equipment eventItem;
    
    public float rate = 0.15f;
    public float GrowthRate 
    {
        get 
        {
            GameManager.instance.LoadData();
            return (1 + (GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot].roomProgress / 5) * rate); 
        }
    }
    void Awake()
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

    public void EquipItem()
    {
        if (displayOrder < equipmentsDrop.Count)
        {
            player.EquipItem(newEquipment);
            NextItemDetermine();
        }
    }

    public void DiscardItem()
    {
        if (displayOrder < equipmentsDrop.Count)
        {
            NextItemDetermine();
        }
    }

    public List<Equipment> EnemyEquipmentGenerator()
    {
        List<Equipment> equipment = new List<Equipment>();
        for (int i = 0; i < t1Equipment.Length; i++)
        {
            float tierPointer = Random.Range(0, 1.00f);
            int roll = Random.Range(0, 2);
            if (roll == 0)
                continue;
            if (tierPointer < 0.35)
            {
                int tempPointer = Random.Range(0, t1Equipment[i].equipment.Length);
                equipment.Add(t1Equipment[i].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.6)
            {
                int tempPointer = Random.Range(0, t2Equipment[i].equipment.Length);
                equipment.Add(t2Equipment[i].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.8)
            {
                int tempPointer = Random.Range(0, t3Equipment[i].equipment.Length);
                equipment.Add(t3Equipment[i].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.95)
            {
                int tempPointer = Random.Range(0, t4Equipment[i].equipment.Length);
                equipment.Add(t4Equipment[i].equipment[tempPointer].DeepCopy());
            }
            else
            {
                int tempPointer = Random.Range(0, t5Equipment[i].equipment.Length);
                equipment.Add(t5Equipment[i].equipment[tempPointer].DeepCopy());
            }
        }
        if (equipment.Count <= 0)
        {
            float tierPointer = Random.Range(0, 1.00f);
            int equipmentPointer = Random.Range(0, 5);
            if (tierPointer < 0.35)
            {
                int tempPointer = Random.Range(0, t1Equipment[equipmentPointer].equipment.Length);
                equipment.Add(t1Equipment[equipmentPointer].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.6)
            {
                int tempPointer = Random.Range(0, t2Equipment[equipmentPointer].equipment.Length);
                equipment.Add(t2Equipment[equipmentPointer].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.8)
            {
                int tempPointer = Random.Range(0, t3Equipment[equipmentPointer].equipment.Length);
                equipment.Add(t3Equipment[equipmentPointer].equipment[tempPointer].DeepCopy());
            }
            else if (tierPointer < 0.95)
            {
                int tempPointer = Random.Range(0, t4Equipment[equipmentPointer].equipment.Length);
                equipment.Add(t4Equipment[equipmentPointer].equipment[tempPointer].DeepCopy());
            }
            else
            {
                int tempPointer = Random.Range(0, t5Equipment[equipmentPointer].equipment.Length);
                equipment.Add(t5Equipment[equipmentPointer].equipment[tempPointer].DeepCopy());
            }
        }
        return equipment;
    }

    public void EventItemDeliver()
    {
        int equipmentPointer = Random.Range(0, 5);
        float tierPointer = Random.Range(0, 1.00f);
        if (tierPointer < 0.35)
        {
            int tempPointer = Random.Range(0, t1Equipment[equipmentPointer].equipment.Length);
            eventItem = t1Equipment[equipmentPointer].equipment[tempPointer].DeepCopy();
        }
        else if (tierPointer < 0.6)
        {
            int tempPointer = Random.Range(0, t2Equipment[equipmentPointer].equipment.Length);
            eventItem = t2Equipment[equipmentPointer].equipment[tempPointer].DeepCopy();
        }
        else if (tierPointer < 0.8)
        {
            int tempPointer = Random.Range(0, t3Equipment[equipmentPointer].equipment.Length);
            eventItem = t3Equipment[equipmentPointer].equipment[tempPointer].DeepCopy();
        }
        else if (tierPointer < 0.95)
        {
            int tempPointer = Random.Range(0, t4Equipment[equipmentPointer].equipment.Length);
            eventItem = t4Equipment[equipmentPointer].equipment[tempPointer].DeepCopy();
        }
        else
        {
            int tempPointer = Random.Range(0, t5Equipment[equipmentPointer].equipment.Length);
            eventItem = t5Equipment[equipmentPointer].equipment[tempPointer].DeepCopy();
        }
        List<Equipment> tempItems = new List<Equipment>{eventItem};
        Display(tempItems);
        EventManager.instance.eventItem = eventItem;
    }

    public void Display(List<Equipment> items)
    {
        equipmentsDrop = items;
        displayOrder = 0;
        StatRefresh();
        equipmentPanel.SetActive(true);
    }

    private void StatRefresh()
    {
        if (player == null)
            player = Player.instance;

        if (displayOrder < equipmentsDrop.Count)
        {
            newEquipment = equipmentsDrop[displayOrder];
            newItemNameText.text = newEquipment.itemName;
            currentItemNameText.text = "";
            newItemIcon.sprite = newEquipment.artWork;
            newDescriptionText.text = newEquipment.equipmentAttributes.ToString() + " " + newEquipment.attributeValue;

            for (int i = 0; i < newEquipment.minorAttributes.Count; i++)
            {
                newDescriptionText.text += "\n" + newEquipment.minorAttributes[i].minorAttributeType.ToString() + " " + newEquipment.minorAttributes[i].value; 
            }

            currentItemIcon.gameObject.SetActive(false);
            currentDescriptionText.text = "";
            bool truthValue = false;
            switch (newEquipment.equipmentType)
            {
                case Type.Helmat:
                    if (player.helmat != null)
                    {
                        currentEquipment = player.helmat;
                        truthValue = true;
                    }
                    break;
                case Type.Chestplate:
                    if (player.chestplate != null)
                    {
                        currentEquipment = player.chestplate;
                        truthValue = true;
                    }
                    break;
                case Type.Gauntlet:
                    if (player.gauntlet != null)
                    {
                        currentEquipment = player.gauntlet;
                        truthValue = true;
                    }
                    break;
                case Type.Greaves:
                    if (player.greaves != null)
                    {
                        currentEquipment = player.greaves;
                        truthValue = true;
                    }
                    break;
                case Type.Axe: case Type.Hammer: case Type.Sword:
                    {
                        if (player.weapon != null)
                        {
                            currentEquipment = player.weapon;
                            truthValue = true;
                        }
                        break;
                    }
                default:
                    {
                        Debug.Log("Failure");
                        break;
                    }
            }
            if (truthValue)
            {
                currentItemNameText.text = currentEquipment.itemName;
                currentItemIcon.gameObject.SetActive(true);
                currentItemIcon.sprite = currentEquipment.artWork;
                currentDescriptionText.text = currentEquipment.equipmentAttributes.ToString() + " " + currentEquipment.attributeValue;
                for (int i = 0; i < currentEquipment.minorAttributes.Count; i++)
                {
                    currentDescriptionText.text += "\n" + currentEquipment.minorAttributes[i].minorAttributeType.ToString() + " " + currentEquipment.minorAttributes[i].value + "%";
                }
            }
        }
    }

    private void NextItemDetermine()
    {
        displayOrder++;
        StatRefresh();
        if (displayOrder == equipmentsDrop.Count)
        {
            //handled all items
            displayOrder = 0;
            equipmentPanel.SetActive(false);

            //i.e. go to next room
            RoomManager.instance.CheckRoomEnd();
        }
    }
}
