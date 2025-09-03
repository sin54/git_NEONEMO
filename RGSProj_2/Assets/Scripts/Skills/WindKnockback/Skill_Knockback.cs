using UnityEngine;

public class Skill_Knockback : BaseSkill
{
    [HideInInspector] public SO_WindKnockbackData skillData;
    private float lastAttackTime;
    [SerializeField] private GameObject windKnockBackGO;
    [Header("실프")]
    public float skillCool_rf2;
    [Header("고기압")]
    public float def_rf3;
    public float atk_rf3;
    public float buffTime_rf3;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_WindKnockbackData))
        {
            skillData = (SO_WindKnockbackData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Update()
    {
        if (CanAttack())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    private void Attack()
    {
        windKnockBackGO.SetActive(true);
        GameManager.instance.SM.AddModifier("PlayerSpeed", multiplier: skillData.speedMul, duration: 1f);
    }

    private bool CanAttack()
    {
        float coolTime;
        if (reinforcedNum != 2)
        {
            coolTime = skillData.coolTimeByLevel[itemLevel];
        }
        else
        {
            coolTime = skillCool_rf2;
        }
        coolTime *= GameManager.instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.instance.SM.GetFinalValue("W_Cool");
        return Time.time > coolTime + lastAttackTime;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        windKnockBackGO.transform.localScale = new Vector3(skillData.radiusByLevel[itemLevel], skillData.radiusByLevel[itemLevel], skillData.radiusByLevel[itemLevel]);
    }
}
