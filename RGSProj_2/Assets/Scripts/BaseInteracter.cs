using UnityEngine;

public class BaseInteracter : MonoBehaviour,IInteractable
{
    public bool isInteracting;
    [SerializeField] private GameObject interactObj;
    private Animator anim;

    private void Awake()
    {
        anim=interactObj.GetComponentInChildren<Animator>();
    }

    public virtual void HoverIn() {
        interactObj.SetActive(true);
    }
    public virtual void HoverOut()
    {
        anim.SetTrigger("Disable");
    }

    public virtual void Interact()
    {
    }
}
