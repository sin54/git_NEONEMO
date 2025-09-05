using Unity.Mathematics;
using UnityEngine;
using Core;

public class LightCircle : BaseAngled
{
    private float speedMulAmount;
    private float sizeIncSpeed;
    private Skill_LightCircle SL;
    public void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, Vector2 ang, int extraValue, float spd, float sizeInc,Skill_LightCircle sl)
    {
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        attackInfo = aInfo;
        bulletDirection = ang;
        maxPenetrationLimit = extraValue;
        transform.rotation = Quaternion.Euler(90, 0, 180);
        speedMulAmount = spd;
        sizeIncSpeed = sizeInc;
        SL = sl;
        bulletParent.position = GameManager.Instance.player.transform.position;
    }

    protected override void AttackEnemy(BaseEnemy target, AttackInfo aInfo)
    {
        GameManager.Instance.AtkEnemy(target, attackInfo.damage, AttackType.MagicAttack, AttackAttr.Light);
        target.eSS.MulSpeedScale(speedMulAmount);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float sizeIncSpd = sizeIncSpeed * GameManager.Instance.SM.GetFinalValue("AoESize");
        transform.localScale += new Vector3(sizeIncSpd,sizeIncSpd,sizeIncSpd);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localScale = Vector3.zero;
    }
    public void OnDisable()
    {
        if (SL!=null&&SL.reinforcedNum == 1)
        {
            GameObject GO = GameManager.Instance.poolManager.Get(38);
            GO.transform.position = transform.position;
            GO.GetComponent<LightCircle_Explosion>().SetExplosion(SL.finalAtk, SL);
        }
    }
    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.AtkEnemy(collision.gameObject.GetComponent<BaseEnemy>(), new AttackInfo(0, attackInfo.knockbackPower), AttackType.StaticAttack, AttackAttr.None,transform.position - collision.gameObject.transform.position);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(SL.reinforcedNum == 2)
            {
                float incNum = SL.increaseNum * GameManager.Instance.SM.GetFinalValue("AoESize");
                transform.localScale += new Vector3(incNum,incNum,incNum);
            }
            if (SL.reinforcedNum == 3)
            {
                collision.GetComponent<BaseEnemy>().eSS.MulDefenceScale(SL.lowDmgAmount);
            }
        }
    }
}
