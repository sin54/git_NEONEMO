using UnityEngine;

public class GunBullet : BaseGuided
{
    private float damageAddAmount;
    private float currentDmgAdd;
    [SerializeField] private GameObject boomObj;
    private float boomRad;
    private float boomDmg;
    private PlayerTypeManager PT;

    protected override void AttackEnemy(BaseEnemy target, AttackInfo aInfo)
    {
        AttackInfo newAInfo = new AttackInfo(aInfo.damage + (BS.reinforcedNum==3?currentDmgAdd:0), aInfo.knockbackPower);
        base.AttackEnemy(target, newAInfo);
        if (PT.BT.typeCode == 1)
        {
            target.eSS.AddFireStack(1);
        }
        else if (PT.BT.typeCode == 2)
        {
            target.eSS.MulSpeedScale(0.9f);
        }
        if (BS.reinforcedNum == 2)
        {
            GameObject GO = GameManager.instance.poolManager.Get(14);
            GO.transform.GetChild(0).GetComponent<BoomEffect>().SetExplosion(target.gameObject.transform.localScale.x * boomRad, new AttackInfo(boomDmg,0), target.gameObject.transform.position);
            GO.transform.GetChild(0).GetComponent<BoomEffect>().IgnoreEnemy(target);
        }

    }
    public void SetBoomEffect(float radius,float dmg)
    {
        boomRad = radius;
        boomDmg = dmg;
    }
    protected override void Awake()
    {
        base.Awake();
        PT = GameManager.instance.playerTypeManager;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        currentDmgAdd += damageAddAmount;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        damageAddAmount = 0f;
        currentDmgAdd = 0f;
        SR.color = PT.Colors[PT.BT.typeCode];
        var Ps = PS.colorOverLifetime;
        Ps.color=PT.Gradients[PT.BT.typeCode];
    }
    protected override void Update()
    {
        base.Update();

    }

    public override void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, BaseSkill baseSkill, Transform sourceTF = null)
    {
        base.SetBullet(bSpeed, bLifeTime, aInfo, baseSkill);
        boomRad = ((Skill_MagicBullet)baseSkill).boomRadius;
        boomDmg= ((Skill_MagicBullet)baseSkill).boomDamage;
        damageAddAmount = ((Skill_MagicBullet)baseSkill).addDamageAmount;
    }
}
