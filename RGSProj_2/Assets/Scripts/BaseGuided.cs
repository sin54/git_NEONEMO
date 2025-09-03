using UnityEngine;
using static Unity.VisualScripting.Member;

public class BaseGuided : BaseBullet
{
    protected bool isTargetDestroyed = false;
    protected GameObject target;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        isTargetDestroyed = false;
        target = GameManager.instance.scanner.GetNearestEnemy();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AttackEnemy(collision.GetComponent<BaseEnemy>(), attackInfo);
            DisableBullet();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isTargetDestroyed)
        {
            if (target == null || !target.activeSelf||target.layer!=8)
            {
                isTargetDestroyed = true;
            }
            bulletDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;
            float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        }

    }
    protected virtual void FixedUpdate()
    {
        RB.linearVelocity = bulletDirection * bulletSpeed;
    }

    public virtual void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, BaseSkill baseSkill,Transform sourceTF = null)
    {
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        attackInfo = aInfo;
        BS = baseSkill;
        if (sourceTF != null)
        {
            bulletParent.position = sourceTF.transform.position;
        }
        else
        {
            bulletParent.position = GameManager.instance.player.transform.position;
        }
    }
}
