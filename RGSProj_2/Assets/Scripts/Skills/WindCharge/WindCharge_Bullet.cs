using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

public class WindCharge_Bullet : Basepenetration
{
    private HashSet<BaseEnemy> baseEnemySet=new HashSet<BaseEnemy>();
    private float damageInterval = 0.2f; // 공격 주기
    private float damageTimer;
    protected override void AttackEnemy(BaseEnemy target, AttackInfo aInfo)
    {
        GameManager.Instance.AtkEnemy(target, aInfo.damage, AttackType.PhysicAttack, AttackAttr.Wind);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy newEnemy = collision.GetComponent<BaseEnemy>();
            if (!baseEnemySet.Contains(newEnemy))
            {
                baseEnemySet.Add(newEnemy);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy newEnemy = collision.GetComponent<BaseEnemy>();
            GameManager.Instance.AtkEnemy(newEnemy, new AttackInfo(0, attackInfo.knockbackPower), AttackType.StaticAttack, AttackAttr.None, newEnemy.transform.position - transform.position);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy newEnemy = collision.GetComponent<BaseEnemy>();
            if (baseEnemySet.Contains(newEnemy))
            {
                baseEnemySet.Remove(newEnemy);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;

            foreach (var enemy in baseEnemySet.ToArray())
            {
                if (enemy != null && enemy.isActiveAndEnabled)
                {
                    AttackEnemy(enemy, attackInfo);
                }
            }
        }
    }
}
