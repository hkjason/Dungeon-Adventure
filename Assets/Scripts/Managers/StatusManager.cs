using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager instance;
    public Image[] playerStatusBar;
    public Image[] enemyStatusBar;

    public Status[] statusDataBase;

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

    public void ActivateStatus(Status s, Character target)
    {
        bool exist = false;
        //Check If Exist, refresh round if
        foreach(Status stat in target.status)
        {
            if(s.statusName == stat.statusName)
            {
                exist = true;
                if(s.remainedClick >= stat.remainedClick)
                {
                    stat.remainedClick = s.remainedClick;
                }
            }
        }
        if(!exist)
            target.status.Add(s.Deepcopy());

        StatusDisplay(target);
    }

    public void CheckStatus(Character target)
    {
        List<Status> temp = new List<Status>();
        foreach(Status s in target.status)
        {
            if(s.remainedClick>0)
                s.StatusFunctioning(target);
            s.remainedClick--;
            if(s.remainedClick<=0)
            {
                if (s.GetType() == typeof(ValueStatus))
                {
                    s.StatusFunctioning(target);
                }
                temp.Add(s);
            }
        }
        foreach(Status s in temp)
        {
            target.status.Remove(s);
        }

        StatusDisplay(target);
    }

    public void StatusDisplay(Character target)
    {
        int temp = 0;
        foreach(Status s in target.status)
        {
            switch (target.name)
            {
                case "Player":
                    playerStatusBar[temp].sprite = s.artWork;
                    playerStatusBar[temp].enabled = true;
                    break;
                case "Enemy":
                    enemyStatusBar[temp].sprite = s.artWork;
                    enemyStatusBar[temp].enabled = true;
                    break;
                default:
                    Debug.Log("failure");
                    break;
            }
            temp++;
        }

        switch (target.name)
        {
            case "Player":
                for(int i=temp; i<playerStatusBar.Length; i++)
                {
                    playerStatusBar[i].enabled = false;
                }
                break;
            case "Enemy":
                for(int i=temp; i<enemyStatusBar.Length; i++)
                {
                    enemyStatusBar[i].enabled = false;
                }
                break;
            default:
                Debug.Log("failure");
                break;
        }
    }

    public void ClearAll()
    {
        ClearStatus(Player.instance);
        ClearStatus(Enemy.instance);
    }

    private void ClearStatus(Character target)
    {
        target.status.Clear();
        StatusDisplay(target);
    }
}
