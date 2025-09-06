using UnityEngine;

public class UIController : MonoBehaviour
{
    public void AnimationFinishTrigger()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
