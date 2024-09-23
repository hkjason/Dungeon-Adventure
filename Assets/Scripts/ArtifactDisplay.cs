using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public Image image;
    public Text artifactText;
    public int number;
    public float size;

    public DisplayType type;

    private void Start()
    {
        artifactText = panel.GetComponent<Text>();
        image = this.gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (type)
        {
            case DisplayType.Battle:
                panel.transform.position = image.transform.position + new Vector3(0, -size, 0);
                break;
            case DisplayType.Normal:
                panel.transform.position = image.transform.position + new Vector3(0, size, 0);
                break;
            default:
                Debug.Log("failure");
                break;
        }
        if (image.enabled == true)
        {
            artifactText.text = Player.instance.artifacts[number].artifactName + "\n" + Player.instance.artifacts[number].artifactDescription;
            panel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }

    public enum DisplayType
    { 
        Battle,
        Normal
    }
}

