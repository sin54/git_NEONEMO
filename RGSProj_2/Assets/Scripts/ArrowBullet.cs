using UnityEngine;

public class ArrowBullet : Basepenetration
{
    private float dmgAdd;
    private float currentDmgAmount;

    public override void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, int maxP, BaseSkill baseSkill, Transform sourceTF = null)
    {
        base.SetBullet(bSpeed, bLifeTime, aInfo, maxP, baseSkill);
        dmgAdd = ((Skill_BowAttack)baseSkill).addDamageAmount;
    }

    protected override void AttackEnemy(BaseEnemy target, AttackInfo aInfo)
    {
        AttackInfo newAttackInfo = new AttackInfo(aInfo.damage + currentDmgAmount, aInfo.knockbackPower);
        base.AttackEnemy(target, newAttackInfo);
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
        currentDmgAmount = 0;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (BS.reinforcedNum==2&&collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            currentDmgAmount += dmgAdd;
        }
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
