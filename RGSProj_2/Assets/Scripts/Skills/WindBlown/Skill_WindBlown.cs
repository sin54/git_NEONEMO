using System.Net.NetworkInformation;
using UnityEngine;

public class Skill_WindBlown :BaseSkill
{
    [HideInInspector] public SO_WindBlownData skillData;
    private float lastAttackTime;
    private bool isSkilling;
    [SerializeField] private ParticleSystem skillPS;
    [SerializeField] private Collider2D skillRange;
    [SerializeField] private Transform windBlownTF;
    [Header("À©µå¹Ð")]
    [SerializeField] private ParticleSystem skillPS2;
    [SerializeField] private Transform windBlownTF2;
    [SerializeField] private Collider2D skillRange2;
    [Header("ÁúÇ³")]
    public float damageMulAmount;
    [Header("¿ªÇ³")]
    public float reduceAmount;
    public float reduceCool;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_WindBlownData))
        {
            skillData = (SO_WindBlownData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void OnEnable()
    {
        skillPS.Stop();
        skillPS2.Stop();
        skillRange.enabled = false;
        skillRange2.enabled = false;
    }
    private void Update()
    {
        windBlownTF.rotation = Quaternion.Euler(270 - GameManager.instance.player.transform.rotation.eulerAngles.z, 90, -90);
        windBlownTF2.rotation= Quaternion.Euler(90 - GameManager.instance.player.transform.rotation.eulerAngles.z, 90, -90);
        float AoES = GameManager.instance.SM.GetFinalValue("AoESize");
        var PS = skillPS.main;
        PS.startLifetime = skillData.rangeByLevel[itemLevel] * AoES;
        float rangeSize = skillData.rangeByLevel[itemLevel] * 2 * AoES;
        skillRange.transform.localScale = new Vector3(rangeSize, rangeSize, rangeSize);
        var PS2 = skillPS2.main;
        PS2.startLifetime = skillData.rangeByLevel[itemLevel];
        skillRange2.transform.localScale = new Vector3(rangeSize, rangeSize, rangeSize);
        if (CanAttack())
        {
            Attack();
            GameManager.instance.SM.AddModifier("PlayerSpeed", multiplier: skillData.speedMulAmount, duration: skillData.durationByLevel[itemLevel]);
            skillPS.Play();
            if (reinforcedNum == 1)
            {
                skillPS2.Play();
            }
            lastAttackTime = Time.time;
            isSkilling = true;
        }
        if (Time.time > lastAttackTime + skillData.durationByLevel[itemLevel]*GameManager.instance.SM.GetFinalValue("SkillDurationMul") && isSkilling)
        {
            skillRange.enabled = false;
            isSkilling = false;
            skillPS.Stop();
            if (reinforcedNum == 1)
            {
                skillPS2.Stop();
                skillRange2.enabled = false;
            }
        }
    }
    private void Attack()
    {
        skillRange.enabled = true;
        if (reinforcedNum == 1)
        {
            skillRange2.enabled = true;
        }
    }
    private bool CanAttack()
    {
        float coolTime = skillData.coolTimeByLevel[itemLevel];
        if (reinforcedNum == 3)
        {
            coolTime -= reduceAmount;
        }
        coolTime *= GameManager.instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.instance.SM.GetFinalValue("W_Cool");
        return Time.time > coolTime + lastAttackTime;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        float AoES = GameManager.instance.SM.GetFinalValue("AoESize");
        var PS = skillPS.main;
        PS.startLifetime=skillData.rangeByLevel[itemLevel]*AoES;
        float rangeSize = skillData.rangeByLevel[itemLevel] * 2 * AoES;
        skillRange.transform.localScale = new Vector3(rangeSize,rangeSize,rangeSize);
        var PS2 = skillPS2.main;
        PS2.startLifetime = skillData.rangeByLevel[itemLevel];
        skillRange2.transform.localScale = new Vector3(rangeSize,rangeSize,rangeSize);
    }
}
