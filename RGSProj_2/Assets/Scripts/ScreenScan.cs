using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScreenScan : MonoBehaviour
{
    public HashSet<BaseEnemy> enemiesInRange = new HashSet<BaseEnemy>();

    public void AttackAllEnemy(float damage,AttackType AT, AttackAttr AA)
    {
        foreach (BaseEnemy enemy in enemiesInRange.ToList())
        {
            GameManager.instance.AtkEnemy(enemy, damage, AT,AA);
        }
    }
    public void DebuffAllEnemy(float amount)
    {
        foreach(BaseEnemy enemy in enemiesInRange.ToList())
        {
            enemy.eSS.MulAttackScale(amount);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
