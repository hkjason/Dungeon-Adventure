using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    /*

    public static DungeonGenerator instance;

    public Tilemap tileMap;
    public int wallSize;
    public Tile[] wall;
    public Tile door;
    public int floorSize;
    public Tile[] floor;

    public float transitionTime;

    [Header("Size")]
    [SerializeField]
    private int _tileMapX;
    [SerializeField]
    private int _tileMapY;

    [Header("Offset")]
    [SerializeField]
    private float _originalX;
    [SerializeField]
    private float _originalY;

    [Header("DoorCollider")]
    public Collider2D[] colliders;

    private int _currentRoomX = 0;
    private int _currentRoomY = 0;

    public bool fromBot;
    public bool fromTop;
    public bool fromLeft;
    public bool fromRight;

    public int roomTypeNumber;
    public RoomType roomLeft;
    public RoomType roomRight;
    public RoomType roomTop;
    public RoomType roomBot;

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
        GenerateRoom();
    }

    void GenerateRoom()
    {
        //if(fromTop)
        //    _currentRoomY--;
        //if(fromBot)
        //    _currentRoomY++;
        //if(fromLeft)
        //    _currentRoomX++;
        //if(fromRight)
        //    _currentRoomX--;
        //tileMap.transform.position = new Vector2(_originalX + _tileMapX*_currentRoomX, _originalY + _tileMapY*_currentRoomY);
        GenerateWall();
        GenerateFloor();
        GenerateRoomType();
        NextRoomDisplay.instance.CheckDisplay(roomTop);
        NextRoomDisplay.instance.CheckDisplay(roomBot);
        NextRoomDisplay.instance.CheckDisplay(roomLeft);
        NextRoomDisplay.instance.CheckDisplay(roomRight);
    }

    void GenerateRoomType()
    {
        int i = 0;
        i = Random.Range(0,roomTypeNumber);
        roomLeft = (RoomType)i;
        i = Random.Range(0,roomTypeNumber);
        roomRight = (RoomType)i;
        i = Random.Range(0,roomTypeNumber);
        roomTop = (RoomType)i;
        i = Random.Range(0,roomTypeNumber);
        roomBot = (RoomType)i;
    }

    void GenerateWall()
    {
        //bot
        for(int i=0;i<_tileMapX;i++)
        {
            int j = Random.Range(0,wallSize);
            tileMap.SetTile(new Vector3Int(i,0,0), wall[j]);
        }
        //top
        for(int i=0;i<_tileMapX;i++)
        {
            int j = Random.Range(0,wallSize);
            tileMap.SetTile(new Vector3Int(i,_tileMapY-1,0), wall[j]);
        }
        //left
        for(int i=1;i<_tileMapY-1;i++)
        {
            int j = Random.Range(0,wallSize);
            tileMap.SetTile(new Vector3Int(0,i,0), wall[j]);
        }
        //right
        for(int i=1;i<_tileMapY-1;i++)
        {
            int j = Random.Range(0,wallSize);
            tileMap.SetTile(new Vector3Int(_tileMapX-1,i,0), wall[j]);
        }
        GenerateDoor();
    }

    void GenerateDoor()
    {
        foreach(GameObject img in NextRoomDisplay.instance.Img)
        {
            img.SetActive(false);
        }
        foreach(Collider2D col in colliders)
        {
            col.enabled = false;
        }
        //top
        int x = Random.Range(0,10);
        if(x<7 && !fromTop)
            TopDoor();
        //right
        int y = Random.Range(0,10);
        if(y<7 && !fromRight)
            RightDoor();
        //bot
        int z = Random.Range(0,10);
        if(z<7 && !fromBot)
            BotDoor();
        //left
        int w = Random.Range(0,10);
        if(w<7 && !fromLeft)
            LeftDoor();

        //x<7 but fromTop makes atleastonedoor not trigger fix
        if(fromTop)
            x=7;
        if(fromRight)
            y=7;
        if(fromBot)
            z=7;
        if(fromLeft)
            w=7;

        //atLeastOneDoor
        if(x>=7&&y>=7&&z>=7&&w>=7)
        {
            int toggle=0;
            if(fromLeft)
                toggle=0;
            if(fromRight)
                toggle=1;
            if(fromTop)
                toggle=2;
            if(fromBot)
                toggle=3;
            AtLeastOneDoor(toggle);
        }

        fromBot = false;
        fromTop = false;
        fromLeft = false;
        fromRight = false;
    }

    void LeftDoor()
    {
        NextRoomDisplay.instance.Img[2].SetActive(true);
        colliders[0].enabled = true;
        for(int i=_tileMapY/2-1;i<_tileMapY/2+1;i++)
            tileMap.SetTile(new Vector3Int(0,i,0), door);
    }

    void RightDoor()
    {
        NextRoomDisplay.instance.Img[3].SetActive(true);
        colliders[1].enabled = true;
        for(int i=_tileMapY/2-1;i<_tileMapY/2+1;i++)
            tileMap.SetTile(new Vector3Int(_tileMapX-1,i,0), door);
    }

    void TopDoor()
    {
        NextRoomDisplay.instance.Img[0].SetActive(true);
        colliders[2].enabled = true;
        for(int i=_tileMapX/2-1;i<_tileMapX/2+1;i++)
            tileMap.SetTile(new Vector3Int(i,_tileMapY-1,0), door);
    }

    void BotDoor()
    {
        NextRoomDisplay.instance.Img[1].SetActive(true);
        colliders[3].enabled = true;
        for(int i=_tileMapX/2-1;i<_tileMapX/2+1;i++)
            tileMap.SetTile(new Vector3Int(i,0,0), door);
    }

    void AtLeastOneDoor(int idx)
    {
        int i;
        do 
        {
            i = Random.Range(0,4);
        } while (i==idx);

        switch(i)
        {
            case 0:
                LeftDoor();
                break;
            case 1:
                RightDoor();
                break;
            case 2:
                TopDoor();
                break;
            case 3:
                BotDoor();
                break;
        }
    }

    void GenerateFloor()
    {
        for(int i=1;i<_tileMapY-1;i++)
        {
            for(int j=1;j<_tileMapX-1;j++)
            {
                int k = Random.Range(0,floorSize);
                tileMap.SetTile(new Vector3Int(j,i,0), floor[k]);
            }
        } 
    }

    public IEnumerator Fade(int idx)
    {
        UIManager.instance.transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        GenerateRoom();
        switch(idx)
        {
            case 0:
                Player.instance.transform.position = new Vector2(-7,-1);
                break;
            case 1:
                Player.instance.transform.position = new Vector2(0,-4);
                break;
            case 2:
                Player.instance.transform.position = new Vector2(0,1.5f);
                break;
            case 3:
                Player.instance.transform.position = new Vector2(7,-1);
                break;
        }
    }
    */
}

/*
public enum RoomType
{
    Enemy,
    Boss,
    Event,
    Rest
}
*/