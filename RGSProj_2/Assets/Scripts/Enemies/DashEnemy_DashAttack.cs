using UnityEngine;
using Core;

public class DashEnemy_DashAttack : MonoBehaviour
{

    private bool isPlayerInside = false;

    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    public void Attack(float dmg)
    {
        if (isPlayerInside)
        {
            GameManager.Instance.player.DecreaseHealth(dmg);
        }
    }
}
