using System.Collections.Generic;
using UnityEngine;
using Core;


public class BaseExplosion2 : MonoBehaviour
{
    public Transform boomParent;
    protected float radius;
    protected AttackInfo damageInfo;
    protected HashSet<BaseEnemy> enemiesInRange = new HashSet<BaseEnemy>();
    public void DisableExplode()
    {
        boomParent.gameObject.SetActive(false);
    }
    protected virtual void OnEnable()
    {
        enemiesInRange.Clear();
    }
    public void IgnoreEnemy(BaseEnemy enemy)
    {
        enemiesInRange.Add(enemy);
    }
    public virtual void SetExplosion(float rad, AttackInfo dmg,Vector3 boomPos)
    {
        radius = rad*GameManager.Instance.SM.GetFinalValue("explosionRad");
        damageInfo = dmg;
        gameObject.transform.parent.localScale = new Vector3(radius, radius, radius);
        transform.position = boomPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (!enemiesInRange.Contains(enemy))
            {
                AttackEnemy(enemy);
                enemiesInRange.Add(enemy);
            }

        }
    }
    protected virtual void AttackEnemy(BaseEnemy BE)
    {

    }
}
