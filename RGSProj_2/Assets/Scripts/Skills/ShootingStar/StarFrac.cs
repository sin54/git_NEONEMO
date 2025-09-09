using UnityEngine;
using Core;

public class StarFrac : MonoBehaviour
{
    private float healAmt;
    private void OnEnable()
    {
        
    }

    public void SetStarFrac(float dur,float healAmount)
    {
        Invoke("DisActive", dur);
        healAmt= healAmount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.player.IncreaseHealth(healAmt);
            gameObject.SetActive(false);
        }
    }
    public void DisActive()
    {
        gameObject.SetActive(false);
    }
}
