using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaAttacker : MonoBehaviour
{
    private Skill_AreaAttacker SA;

    private float damageTimer = 0f;
    public List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    private void Awake()
    {
        SA=GetComponentInParent<Skill_AreaAttacker>();
    }
    private void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= SA.skillData.damageInterval)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                    if (enemy.gameObject.activeSelf)
                    {
                        if (SA.reinforcedNum == 1)
                        {
                            GameManager.instance.AtkEnemy(enemy, SA.baseAttackDmg + 3.5f * Vector2.Distance(transform.position, enemy.transform.position), AttackType.MagicAttack,AttackAttr.Normal);
                        }
                        else if (SA.reinforcedNum == 3)
                        {
                            GameManager.instance.AtkEnemy(enemy, SA.skillData.attackDmgByLevel[SA.itemLevel], AttackType.MagicAttack, AttackAttr.Normal);
                            if (UtilClass.GetPercent(SA.stunPercent))
                            {
                                enemy.eSS.AddStunTime(SA.stunTime);
                            }
                        }
                        else
                        {
                            GameManager.instance.AtkEnemy(enemy, SA.skillData.attackDmgByLevel[SA.itemLevel], AttackType.MagicAttack, AttackAttr.Normal);
                        }
                    }
                    else
                    {
                        enemiesInRange.Remove(enemy);
                    }
                }
            }

            damageTimer = 0f;
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
                if (SA.reinforcedNum == 2)
                {
                    enemy.eSS.MulAttackScale(SA.damageReduceAmount);
                }
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
                if (SA.reinforcedNum == 2)
                {
                    enemy.eSS.MulAttackScale(1.0f/SA.damageReduceAmount);
                }

            }
        }
    }
}
