using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

public class WindBlownObj : MonoBehaviour
{
    private float damageTimer = 0f;
    public List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    [SerializeField] private Skill_WindBlown SW;
    private void Update()
    {
        damageTimer += Time.deltaTime;


        if (damageTimer >= 0.2f)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                    if (SW.reinforcedNum == 2)
                    {
                        GameManager.Instance.AtkEnemy(enemy, new AttackInfo(SW.skillData.damageByLevel[SW.itemLevel].damage * (1.5f / GameManager.Instance.player.playerFinalSpeed) * SW.damageMulAmount, SW.skillData.damageByLevel[SW.itemLevel].knockbackPower), AttackType.PhysicAttack, AttackAttr.Wind,enemy.gameObject.transform.position - GameManager.Instance.player.transform.position);
                    }
                    else if (SW.reinforcedNum == 3)
                    {
                        bool isDead = GameManager.Instance.AtkEnemy(enemy, SW.skillData.damageByLevel[SW.itemLevel], AttackType.PhysicAttack, AttackAttr.Wind,enemy.gameObject.transform.position - GameManager.Instance.player.transform.position);
                        if (isDead)
                        {
                            SW.reduceCool += SW.reduceAmount;
                        }
                    }
                    else
                    {
                        GameManager.Instance.AtkEnemy(enemy, SW.skillData.damageByLevel[SW.itemLevel], AttackType.PhysicAttack, AttackAttr.Wind,enemy.gameObject.transform.position - GameManager.Instance.player.transform.position);
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
