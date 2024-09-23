using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRoomDisplay : MonoBehaviour
{
    public static NextRoomDisplay instance;
    public GameObject[] Img;

    public int currentIdx=0;

    public Sprite[] roomIcon;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(this);
        }
    }

    void Start()
    {
        currentIdx = 0;
    }

    /*
    public void CheckDisplay(RoomType room)
    {
        switch(room)
        {
            case RoomType.Enemy:
                Img[currentIdx].GetComponent<SpriteRenderer>().sprite = roomIcon[0];
                break;
            case RoomType.Boss:
                Img[currentIdx].GetComponent<SpriteRenderer>().sprite = roomIcon[1];
                break;
            case RoomType.Event:
                Img[currentIdx].GetComponent<SpriteRenderer>().sprite = roomIcon[2];
                break;
            case RoomType.Rest:
                Img[currentIdx].GetComponent<SpriteRenderer>().sprite = roomIcon[3];
                break;
        }
        currentIdx++;
        if(currentIdx>=4)
            currentIdx=0;
    }

    */
}
