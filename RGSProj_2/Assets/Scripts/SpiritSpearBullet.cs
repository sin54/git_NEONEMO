using UnityEngine;
using Core;

public class SpiritSpearBullet : BaseBullet
{
    private Skill_FireSpirit SF;
    public void SetBullet(float bSpeed, float bLifeTime, Vector2 ang,Skill_FireSpirit sf)
    {
        bulletLifeTime = bLifeTime;
        RB.linearVelocity = bSpeed * ang.normalized;
        SF = sf;
    }


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.GetComponent<BaseEnemy>().eSS.AddFireStack(SF.spearFireAmount);
            GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),SF.spearDamage,AttackType.PhysicAttack, AttackAttr.Fire); 
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        float angle = Mathf.Atan2(RB.linearVelocityY, RB.linearVelocityX) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0, 0, angle);
    }
}
