using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public float ghostDelay;
    public float ghostDestroyTime;
    private float ghostDelayTime;
    private SpriteRenderer SR;
    public GameObject ghost;
    public bool makeGhost;

    private void Awake()
    {
        SR= GetComponent<SpriteRenderer>(); 
    }
    void Start()
    {
        this.ghostDelayTime = this.ghostDelay;
    }

    void FixedUpdate()
    {
        if (this.makeGhost)
        {
            if (this.ghostDelayTime > 0)
            {
                this.ghostDelayTime -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(this.ghost, this.transform.position, this.transform.rotation);
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().material.color=SR.material.color;
                this.ghostDelayTime = this.ghostDelay;
                Destroy(currentGhost, ghostDestroyTime);
            }
        }
    }
}
