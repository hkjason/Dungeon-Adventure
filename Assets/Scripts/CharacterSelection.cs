using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection instance;
    public CharacterDataBase[] characterData;

    [Header("Stat Panel")]
    public GameObject statPanel;
    public Text characterNameText;
    public Text maxhpText;
    public Text strengthText;
    public Text intelligenceText;
    public Text dexterityText;

    public Text armorText;
    public Text magicResistentText;
    //public Text dodgeRateText;
    //public Text baseDamageText;
    public Text critChanceText;
    //public Text luckText;
    public Text faithText;

    //To be put into UIManager
    [Header("Buttons")]
    public GameObject returnButton;
    public GameObject goButton;

    [Header("Character selection wheel variables")]
    public int characterNumber;
    public GameObject[] gameObjects;
    public CanvasGroup[] canvasGroup;
    public Transform[] positionHolder;
    private Vector3[] positionVectors;

    [Space]
    public Button leftButton;
    public Button rightButton;
    public float tweenTime;

    public int idx;

    private void Awake()
    {
        if(instance==null)
            instance =this;
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        positionVectors = new Vector3[characterNumber*3+1];
        int i=0;
        foreach(Transform transform in positionHolder)
        {
            positionVectors[i]=transform.position;
            i++;
        }
        positionVectors[characterNumber*3] = positionHolder[0].position;
        //StartSelection();
    }

    public void StartSelection()
    {
        Check();
        statPanel.SetActive(true);
        //PanelInOut(Vector3.one);
        StatRefresh();

        StartUIManager.instance.StartButtonActive(false);
        ButtonActive(true);
    }

    //true if return, false if go
    public void EndSelection(bool tf)
    {
        statPanel.SetActive(false);
        StartUIManager.instance.StartButtonActive(tf);
        ButtonActive(false);
        //PanelInOut(Vector3.zero);
    }

    private void ButtonActive(bool tf)
    {
        returnButton.SetActive(tf);
        goButton.SetActive(tf);
    }
    public void PanelInOut(Vector3 vec)
    {
        LeanTween.scale(statPanel, vec, 0.5f).setEase(LeanTweenType.linear);
    }

    public void Left()
    {
        TweenRight();
        idx--;
        Check();
        StartCoroutine(ButtonCheck());
        StatRefresh();
    }

    public void Right()
    {
        TweenLeft();
        idx++;
        Check();
        StartCoroutine(ButtonCheck());
        StatRefresh();
    }

    private void StatRefresh()
    {
        characterNameText.text = characterData[idx].characterName.ToString();
        maxhpText.text = characterData[idx].maxhp.ToString();
        strengthText.text = characterData[idx].strength.ToString();
        intelligenceText.text = characterData[idx].intelligence.ToString();
        dexterityText.text = characterData[idx].Dexterity.ToString();
        armorText.text = characterData[idx].armor.ToString();
        magicResistentText.text = characterData[idx].magicResistent.ToString();
        //dodgeRateText.text = characterData[idx].dodgeRate.ToString();
        //baseDamageText.text = characterData[idx].baseDamage.ToString();
        critChanceText.text = characterData[idx].critChance.ToString();
        //luckText.text = characterData[idx].luck.ToString();
        faithText.text = characterData[idx].faith.ToString();
    }

    private void Check()
    {
        if(idx<0)
        {
            idx = characterNumber-1;
        }
        if(idx > characterNumber-1)
        {
            idx = 0;
        }

        //int i = 1;
        int id = idx;
        canvasGroup[id].alpha = 1;
        if(id-1 < 0)
            canvasGroup[characterNumber-1].alpha = 0.3f;
        else
            canvasGroup[id-1].alpha = 0.6f;
        if(id+1 >= characterNumber)
            canvasGroup[0].alpha = 0.3f;
        else
            canvasGroup[id+1].alpha = 0.6f;       
        if(id-2 < 0)
            canvasGroup[characterNumber-1].alpha = 0.6f;
        else
            canvasGroup[id-2].alpha = 0.3f;
        if(id+2 >= characterNumber)
            canvasGroup[0].alpha = 0.6f;
        else
            canvasGroup[id+2].alpha = 0.3f;
    }

    private IEnumerator ButtonCheck()
    {
        leftButton.interactable = false;
        rightButton.interactable = false;

        yield return new WaitForSeconds(tweenTime);

        leftButton.interactable = true;
        rightButton.interactable = true;
    }

    private void TweenLeft()
    {
        int x = 0;
        int tweenIdx = idx;
        while (x<characterNumber)
        {
            if(tweenIdx >= characterNumber)
                tweenIdx = 0;
            LeanTween.move(gameObjects[x], new Vector3[] {positionVectors[tweenIdx*3], positionVectors[tweenIdx*3+1],positionVectors[tweenIdx*3+2], positionVectors[tweenIdx*3+3]}, tweenTime).setEase(LeanTweenType.easeOutQuad).setOrientToPath(false);
            x++;
            tweenIdx++;
        }
    }

    private void TweenRight()
    {
        int x = 0;
        int y = 0;
        int tweenIdx = idx;
        while (y<5)
        {
            if(tweenIdx <= 0)
                tweenIdx = characterNumber;
            if(x < 0)
                x = characterNumber-1;
            LeanTween.move(gameObjects[x], new Vector3[] {positionVectors[tweenIdx*3], positionVectors[tweenIdx*3-1],positionVectors[tweenIdx*3-2], positionVectors[tweenIdx*3-3]}, tweenTime).setEase(LeanTweenType.easeOutQuad).setOrientToPath(false);
            x--;
            tweenIdx--;
            y++;
        }
    }
}
