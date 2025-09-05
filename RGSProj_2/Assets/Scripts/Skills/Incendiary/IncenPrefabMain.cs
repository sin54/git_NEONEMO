using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Core;

public class IncenPrefabMain : MonoBehaviour
{
    private float spawnTime = 0f;
    private float damage;
    private float radius;
    private float duration;
    private Skill_Incendiary SI;

    private float damageTimer = 0f;
    public List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();

    private void OnEnable()
    {
        spawnTime = Time.time;
    }
    void Update()
    {
        if (Time.time > spawnTime + duration)
        {
            gameObject.SetActive(false);
        }

        damageTimer += Time.deltaTime;

        
        if (damageTimer >=0.4f)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                    if (enemy.gameObject.activeSelf)
                    {
                        GameManager.Instance.AtkEnemy(enemy, damage, AttackType.PhysicAttack, AttackAttr.Normal);
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
    public void Init(float dur, float dmg, float size,Skill_Incendiary si)
    {
        duration= dur*GameManager.Instance.SM.GetFinalValue("SkillDurationMul");
        damage = dmg;
        radius = size*GameManager.Instance.SM.GetFinalValue("AoESize");
        transform.localScale = new Vector3(radius, radius, radius);
        SI= si;
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                if (SI.reinforcedNum == 2)
                {
                    enemy.eSS.MulSpeedScale(SI.slowDownAmount);
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
                if (SI.reinforcedNum == 2)
                {
                    enemy.eSS.MulSpeedScale(1.0f/SI.slowDownAmount);
                }
            }
        }
    }
}
