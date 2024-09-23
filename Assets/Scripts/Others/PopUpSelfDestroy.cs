using UnityEngine;

public class PopUpSelfDestroy : MonoBehaviour
{
    public void SelfDestroy()
    {
        //destroy pop up parent
        Destroy(gameObject.transform.parent.gameObject);
    }
}
