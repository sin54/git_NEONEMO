using UnityEngine;

public class Skill_LightCircle : BaseSkill
{
    [HideInInspector] public SO_LightCircleData skillData;
    private float lastAttackTime;
    [Header("ºØ±«")]
    public AttackInfo finalAtk;
    [Header("°úÆ÷È­")]
    public float increaseNum;
    [Header("Ãë¾àÁö´ë")]
    public float lowDmgAmount;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_LightCircleData))
        {
            skillData = (SO_LightCircleData)baseSkillData;
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
            lastAttackTime= Time.time;
            GameObject GO = GameManager.instance.poolManager.Get(32);
            GO.transform.GetChild(0).GetComponent<LightCircle>().SetBullet(reinforcedNum==3?1.2f:1.8f, 10, skillData.damageByLevel[itemLevel], new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, 10000, skillData.speedMulAmount[itemLevel], skillData.increasingSpeed[itemLevel],this);
        }
    }
    private bool CanAttack()
    {
        return Time.time > skillData.coolTimeByLevel[itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("L_Cool") + lastAttackTime;
    }
}
