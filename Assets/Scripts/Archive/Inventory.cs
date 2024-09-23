using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(this.gameObject);
        }
    }

    public List<Equipment> items = new List<Equipment>();

    public void AddItem(Equipment item)
    {
        Debug.Log("add item");
        items.Add(item);
    }
    
}
