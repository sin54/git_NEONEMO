using UnityEngine;
using static Unity.VisualScripting.Member;
using Core;

public class Basepenetration : BaseBullet
{
    protected int maxPenetrationLimit;
    protected int currentPenetration = 0;
    protected GameObject target;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        currentPenetration = 0;
        target = GameManager.Instance.scanner.GetNearestEnemy();
        if (target == null)
        {
            bulletDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        else
        {
            bulletDirection = new Vector2(target.transform.position.x - GameManager.Instance.player.transform.position.x, target.transform.position.y -GameManager.Instance.player.transform.position.y).normalized;
        }

        float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
    protected virtual void FixedUpdate()
    {
        RB.linearVelocity = bulletDirection * bulletSpeed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AttackEnemy(collision.GetComponent<BaseEnemy>(), attackInfo);
            currentPenetration++;
            if (currentPenetration > maxPenetrationLimit)
            {
                DisableBullet();
            }

        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
    }
    public virtual void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, int maxP, BaseSkill baseSkill, Transform sourceTF = null)
    {
        currentPenetration = 0;
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        BS = baseSkill;
        attackInfo = aInfo;
        maxPenetrationLimit = maxP;
        float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        if (sourceTF != null)
        {
            bulletParent.position = sourceTF.transform.position;
        }
        else
        {
            bulletParent.position = GameManager.Instance.player.transform.position;
        }
    }

}
