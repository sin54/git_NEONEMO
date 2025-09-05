using UnityEngine;
using UnityEngine.UIElements;
using Core;

public class BaseAngled : BaseBullet
{
    protected int maxPenetrationLimit;
    protected int currentPenetration = 0;
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
    public virtual void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, Vector2 ang, int maxP, BaseSkill baseSkill,Transform sourceTF=null)
    {
        currentPenetration = 0;
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        BS = baseSkill;
        attackInfo = aInfo;
        bulletDirection = ang;
        maxPenetrationLimit = maxP;
        float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        if (sourceTF != null)
        {
            bulletParent.position=sourceTF.transform.position;
        }
        else
        {
            bulletParent.position = GameManager.Instance.player.transform.position;
        }
    }
}
