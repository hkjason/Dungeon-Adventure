using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 backToScale;
    public RectTransform targetButton;
    private void Start()
    {
        backToScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(targetButton);
        transform.localScale = backToScale;
        LeanTween.scale(targetButton, transform.localScale*1.2f, 0.4f).setEase(LeanTweenType.easeOutElastic);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(targetButton, backToScale, 0.4f).setEase(LeanTweenType.easeOutQuint);
    }
}
