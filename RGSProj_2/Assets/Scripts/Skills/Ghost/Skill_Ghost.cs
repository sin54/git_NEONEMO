using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Ghost : BaseSkill
{
    [HideInInspector] public SO_GhostData skillData;
    private float lastAttackTime;
    private bool isSkilling;
    [SerializeField]private GameObject ghostObj;
    [SerializeField] private GameObject playerTrail;
    [SerializeField] private ParticleSystem runningParticle;
    [SerializeField] private GameObject HPimg;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();
    private float currentDuration;
    [Header("¹Ù¶÷ Ä®³¯")]
    public float damageOfSword;
    public float rotationSpeedOfSword;
    public GameObject firstSword;
    public GameObject lastSword;
    [Header("È­¿°Ç³")]
    private int fireStack_rf2;
    public float transPercent;
    [Header("±Þ°¡¼Ó")]
    public float incSpeedAmount_rf3;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_GhostData))
        {
            skillData = (SO_GhostData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void OnEnable()
    {
        ghostObj.SetActive(false);
        runningParticle.Stop();
        firstSword.SetActive(false);
        lastSword.SetActive(false); 
    }
    private void Update()
    {
        if (CanAttack()&&!isSkilling)
        {
            Attack();
            currentDuration = skillData.durationByLevel[itemLevel] * GameManager.instance.SM.GetFinalValue("SkillDurationMul");
            GameManager.instance.SM.AddModifier("PlayerSpeed", additive: skillData.playerSpeedAddAmount, duration: currentDuration);
            runningParticle.Play();
            lastAttackTime = Time.time;
            isSkilling = true;
            GameManager.instance.player.canDash = false;
            playerTrail.SetActive(false);
            HPimg.SetActive(false);
        }
        if (Time.time > lastAttackTime + currentDuration&&isSkilling)
        {
            isSkilling = false;
            GameManager.instance.player.noDamage = false;
            ghostObj.SetActive(false);
            playerTrail.SetActive(true);
            HPimg.SetActive(true);
            if (reinforcedNum == 1)
            {
                firstSword.SetActive(false);
                lastSword.SetActive(false);
            }
            GameManager.instance.player.GetComponent<Collider2D>().enabled = true;
            GameManager.instance.player.canDash =true;
            GameManager.instance.player.GetComponent<SpriteRenderer>().enabled = true;
            runningParticle.Stop();
        }
    }
    private void Attack()
    {
        fireStack_rf2 = 0;
        GameManager.instance.player.noDamage = true;
        ghostObj.SetActive(true);
        GameManager.instance.player.GetComponent<Collider2D>().enabled = false;
        GameManager.instance.player.GetComponent<SpriteRenderer>().enabled = false;
        if (reinforcedNum == 1)
        {
            firstSword.SetActive(true);
            lastSword.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSkilling&& collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy BE = collision.GetComponent<BaseEnemy>();
            GameManager.instance.AtkEnemy(BE, skillData.damageByLevel[itemLevel], AttackType.PhysicAttack, AttackAttr.Wind);
            if (reinforcedNum == 2)
            {
                if (fireStack_rf2 == 0 && BE.eSS.GetFireStack() > 0)
                {
                    fireStack_rf2 = BE.eSS.GetFireStack();
                }
                if (UtilClass.GetPercent(transPercent))
                {
                    BE.eSS.AddFireStack(fireStack_rf2);
                }

            }
            if (reinforcedNum == 3)
            {
                float dur = skillData.durationByLevel[itemLevel] -(Time.time-lastAttackTime);
                if (dur > 0)
                {
                    GameManager.instance.SM.AddModifier("PlayerSpeed", additive: incSpeedAmount_rf3, duration: dur);
                }

            }
        }
    }
    private bool CanAttack()
    {
        return Time.time > skillData.coolTimeByLevel[itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce")* GameManager.instance.SM.GetFinalValue("W_Cool") + lastAttackTime;
    }
}
