using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public float transitionTime = 1f;

    public CanvasGroup fadeImage;
    public GameObject loadingCanvas;
    public Slider loadingBar;
    public Text loadingPercentageText;

    private void Awake()
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

    public void SceneChange(int levelIndex)
    {
        if (levelIndex == 0)
            UIManager.instance.pauseMenu.SetActive(false);
        StartCoroutine(LoadScene(levelIndex));
    }

    private IEnumerator LoadScene(int levelIndex)
    {
        FadeIn(0.5f);

        loadingCanvas.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            loadingPercentageText.text = progress * 100f + "%";
            yield return null;
        }

        if(operation.isDone)
        {
            loadingCanvas.SetActive(false);
            FadeOut(0.5f);
            switch(levelIndex)
            {
                case 0:
                    Debug.Log("load scene 0");
                    StartUIManager.instance.startCanvas.SetActive(true);
                    StartUIManager.instance.StartButtonActive(true);
                    break;
                case 1:
                    break;
            }
        }
    }

    public IEnumerator StartFade(float time)
    {
        Debug.Log("cour fade");
        FadeIn(time);
        yield return new WaitForSeconds(time);
        FadeOut(time);
    }

    private void FadeIn(float time)
    {
        TweenAlpha(time, 0f, 1f);
    }

    private void FadeOut(float time)
    {
        TweenAlpha(time, 1f, 0f);
    }

    private void TweenAlpha(float time, float start, float end)
    {
        LeanTween.value(gameObject, start, end, time).setOnUpdate((float val)=>
        {
            fadeImage.alpha = val;
        });
    }
}
