using UnityEngine;

public class FloatingTxt : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.SetTrigger("start");
    }
    public void AnimationFinishTrigger()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
