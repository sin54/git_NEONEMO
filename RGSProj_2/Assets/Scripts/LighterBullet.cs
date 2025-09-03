using UnityEngine;

public class LighterBullet : BaseGuided
{
    private int nowLevel;
    private SO_LighterData Data;
    private int rfNum;
    private bool hasTarget;

    public override void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, BaseSkill baseSkill, Transform sourceTF = null)
    {
        base.SetBullet(bSpeed, bLifeTime, aInfo, baseSkill);
        nowLevel = ((Skill_Lighter)baseSkill).itemLevel;
        rfNum = ((Skill_Lighter)baseSkill).reinforcedNum;
        Data = ((Skill_Lighter)baseSkill).skillData;
    }
    public void SetTarget(GameObject tg)
    {
        hasTarget = true;
        target = tg;
    }

    protected override void AttackEnemy(BaseEnemy target, AttackInfo aInfo)
    {
        base.AttackEnemy(target, aInfo);
        if (rfNum == 1 || rfNum == 2)
        {
            target.eSS.AddFireStack(1);
        }
        else if (rfNum == 3)
        {
            if (target.eSS.GetFireStack() == 0)
            {
                target.eSS.AddFireStack(1);
            }
            else
            {
                target.eSS.MulFireStack(2);
            }
        }
        else
        {
            target.eSS.AddFireStack(Data.fireStackByLevel[nowLevel]);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        if (hasTarget)
        {
            RB.linearVelocity = bulletDirection * bulletSpeed;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hasTarget = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
