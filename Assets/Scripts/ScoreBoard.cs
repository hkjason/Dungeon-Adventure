using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    [Header("Texts")]
    public Text roomText;
    public Text killCountText;
    public Text bossKillCountText;
    public Text damageDealtText;
    public Text highestDamageDealtText;
    public Text damageTakenText;
    public Text moneyCollectedText;
    public Text scoreText;

    [Header("Tween Settings")]
    public float tweenTime;
    public float scoreBoardTime;
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

    //////////////////////////////
    //DisplayScore called when end
    //////////////////////////////
    public void DisplayScore()
    {
        LeanTween.scale(this.gameObject, new Vector3(1,1,1), scoreBoardTime).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>Tween());
    }

    ///////////////////////////////////////////
    //Tween all score in scoreboard, one by one
    ///////////////////////////////////////////
    private void Tween()
    {
        SaveData.SaveSlot saveSlot = GameManager.instance.saveData.saveSlot[GameManager.instance.saveData.lastPlayedSlot];
        LeanTween.value(gameObject,0f, RoomManager.instance.currentRoomLevel, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val)=>
        {
            roomText.text = "Room: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(()=>
        {
        LeanTween.value(gameObject,0f, saveSlot.killCount, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val)=>
        {
            killCountText.text = "Killed: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(()=>
        {
        LeanTween.value(gameObject,0f, saveSlot.bossKillCount, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val)=>
        {
            bossKillCountText.text = "Boss Killed: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(() =>
        {
        LeanTween.value(gameObject, 0f, saveSlot.damageDealt, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val) =>
        {
            damageDealtText.text = "Damage Dealt: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(() =>
        {
        LeanTween.value(gameObject, 0f, saveSlot.highestDamageDealt, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val) =>
        {
            highestDamageDealtText.text = "Highest Damage Dealt: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(() =>
        {
        LeanTween.value(gameObject, 0f, saveSlot.damageTaken, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val) =>
        {
            damageTakenText.text = "Damage Taken: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(() =>
        {
        LeanTween.value(gameObject, 0f, saveSlot.moneyCollected, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val) =>
        {
            moneyCollectedText.text = "Money Collected: " + Mathf.RoundToInt(val).ToString();
        }).setOnComplete(() =>
        {
        LeanTween.value(gameObject, 0f, saveSlot.score, tweenTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float val) =>
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(val).ToString();
        });
        });
        });
        });
        });
        });
        });
        });
    }

    ////////////////////////////////////
    //Tween one more value in scoreBoard
    ////////////////////////////////////
    //Remove the ";" in the first "});"
    //Then add the code behind
    ////////////////////////////////////
     //.setOnComplete(()=>
     //{
     //LeanTween.value(gameObject,0f, InsertFinalValue, InsertTweenTime).setEase(InsertTweenType).setOnUpdate((float val)=>
     //{
     //    InsertTextObject.text = Mathf.RoundToInt(val).ToString();
     //});
     //});
    ///////////////////////////////////
}
