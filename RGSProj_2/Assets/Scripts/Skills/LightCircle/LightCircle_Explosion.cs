using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core;

public class LightCircle_Explosion : MonoBehaviour
{
    private AttackInfo damageInfo;
    private HashSet<BaseEnemy> enemiesInRange = new HashSet<BaseEnemy>();
    private Skill_LightCircle SM;
    public void SetExplosion( AttackInfo dmg, Skill_LightCircle sm)
    {
        damageInfo = dmg;
        SM = sm;
    }

    private void OnEnable()
    {
        enemiesInRange.Clear();
        Invoke("BoomAttack", 0.02f);
    }
    public void BoomAttack()
    {
        foreach (BaseEnemy enemy in enemiesInRange.ToList())
        {
            if (enemy != null)
            {
                GameManager.Instance.AtkEnemy(enemy, damageInfo, AttackType.MagicAttack, AttackAttr.Light,enemy.gameObject.transform.position - transform.position);
            }
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
}
