using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseExplosion : MonoBehaviour
{
    protected float radius;
    protected float timeToExplode;
    protected BaseSkill baseSkill;
    public Transform boomParent;
    protected AttackInfo damageInfo;

    protected HashSet<BaseEnemy> enemiesInRange = new HashSet<BaseEnemy>();

    public virtual void SetExplode(float rad, AttackInfo dmg, float time, BaseSkill BS)
    {
        radius = rad*GameManager.instance.SM.GetFinalValue("explosionRad");
        damageInfo = dmg;
        timeToExplode = time;
        baseSkill = BS;
        gameObject.transform.localScale = new Vector3(radius, radius, radius);
        Invoke("BoomAttack", timeToExplode);
    }
    protected void OnEnable()
    {
        enemiesInRange.Clear();
    }
    public void BoomAttack()
    {
        foreach (BaseEnemy enemy in enemiesInRange.ToList())
        {
            if (enemy != null)
            {
                AttackEnemy(enemy);
            }
        }
    }
    public virtual void AttackEnemy(BaseEnemy BE)
    {
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
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }
}
