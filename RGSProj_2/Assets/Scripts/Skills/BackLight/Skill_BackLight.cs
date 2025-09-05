using System.Collections;
using Core;
using UnityEngine;

public class Skill_BackLight : BaseSkill
{
    [HideInInspector] public SO_BackLightData skillData;
    [SerializeField] private GameObject PSm;
    [SerializeField] private ParticleSystem[] PSs;
    [SerializeField] private Player player;
    private float lastAttackTime;
    private bool isSkilling;
    private float multiAmount;
    private float thisTurnDuration;
    [Header("µÐÈ­Áö´ë")]
    public float slowDownAmount;
    [Header("Èí¼ö")]
    public float increaseNum;
    private float increasingAmount;
    private int nowKillEnemy;
    [Header("ÀüÀÇ»ó½Ç")]
    public float critMul;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_BackLightData))
        {
            skillData = (SO_BackLightData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Start()
    {
        PSm.SetActive(false);
    }
    private void Update()
    {
        if (CanAttack()&&!isSkilling)
        {
            increasingAmount = 0;
            nowKillEnemy=GameManager.Instance.killedEnemy;
            lastAttackTime = Time.time;
            isSkilling = true;
            multiAmount= skillData.damageMultiplier[itemLevel] * (1 + (player.playerCurrentHealth * 0.5f) / player.playerMaxHealth);
            GameManager.Instance.SM.AddModifier("AtkMul", multiplier: multiAmount, tag: "BackLight");
            thisTurnDuration = skillData.durationByLevel[itemLevel] * GameManager.Instance.SM.GetFinalValue("SkillDurationMul");
            foreach (ParticleSystem p in PSs) {
                var PSMain = p.main;
                PSMain.duration = thisTurnDuration;
                PSMain.startLifetime = thisTurnDuration;
            }
            if (reinforcedNum == 3)
            {
                GameManager.Instance.SM.AddModifier("CriticalPercent", multiplier: critMul, tag: "BL_3");
            }
            PSm.SetActive(true);
        }
        if(Time.time > lastAttackTime + thisTurnDuration && isSkilling)
        {
            PSm.SetActive(false);
            isSkilling = false;
            GameManager.Instance.SM.RemoveModifiersByTag("BackLight");
            GameManager.Instance.SM.RemoveModifiersByTag("BL_3");
        }
    }
    private void FixedUpdate()
    {
        if (isSkilling)
        {
            increasingAmount = (GameManager.Instance.killedEnemy - nowKillEnemy) * increaseNum;
        }

    }
    private bool CanAttack()
    {
        float coolTime;
        if (reinforcedNum == 2)
        {
            coolTime = skillData.coolTimeByLevel[itemLevel]-increasingAmount;
        }
        else
        {
            coolTime = skillData.coolTimeByLevel[itemLevel];
        }
        coolTime *= GameManager.Instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.Instance.SM.GetFinalValue("L_Cool");
        return Time.time > coolTime + lastAttackTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (reinforcedNum == 1)
            {
                collision.GetComponent<BaseEnemy>().eSS.MulSpeedScale(slowDownAmount);
            }
        }
    }
}
