using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public Image image;
    public Text statusText;
    public int number;
    public float size;

    public CharacterType characterType;

    private void Start()
    {
        statusText = panel.GetComponent<Text>();
        image = this.gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.transform.position = image.transform.position + new Vector3(0,-size,0);
        if(image.enabled == true)
        {
            switch(characterType)
            {
                case CharacterType.Player:
                    statusText.text = Player.instance.status[number].statusName;
                    break;
                case CharacterType.Enemy:
                    statusText.text = Enemy.instance.status[number].statusName;
                    break;
            }
            panel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }

    public enum CharacterType
    {
        Player,
        Enemy
    }
}
