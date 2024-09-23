using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    public static StartUIManager instance;

    public GameObject startCanvas;

    [Header("StartGameButtons")]
    public GameObject continueButton;
    public Button continueButtonUI;
    public GameObject loadGameButton;
    public GameObject newGameButton;


    public CanvasGroup fadeImage;

    void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }   
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }
    }

    public void StartButtonActive(bool tf)
    {
        continueButton.SetActive(tf);
        loadGameButton.SetActive(tf);
        newGameButton.SetActive(tf);
    }

    //Fade screen
    public void StartFade()
    {
        LeanTween.value(gameObject,0f,1f,1f).setOnUpdate((float val)=>
        {
            fadeImage.alpha = val;
        }).setOnComplete(TweenAlpha);
    }

    private void TweenAlpha()
    {
        LeanTween.value(gameObject,1f,0f,1f).setOnUpdate((float val)=>
        {
            fadeImage.alpha = val;
        });
    }

}