using UnityEngine;

public class SpearEnemy_Spear : MonoBehaviour
{
    private SpearEnemy SE;
    private float lastAttackTime;


    private void Awake()
    {
        SE = GetComponentInParent<SpearEnemy>();
    }
    private bool CanAttack()
    {
        return Time.time > lastAttackTime + SE.spearAttackCool;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanAttack())
        {
            float attackDmg = SE.spearDamage;
            GameManager.instance.player.DecreaseHealth(attackDmg);
            lastAttackTime = Time.time;
        }
    }
}
