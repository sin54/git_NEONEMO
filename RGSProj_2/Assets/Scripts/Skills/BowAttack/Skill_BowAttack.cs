using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Core;

public class Skill_BowAttack : BaseSkill
{
    [HideInInspector] public SO_BowAttackData skillData;
    [SerializeField]private int bulletPrefabNum;
    private float lastAttackTime;
    [Header("맹인")]
    public int numofArrow;
    [Header("절정")]
    public float addDamageAmount;
    [Header("무언영창")]
    public float arrowCoolDown;

    public override void Upgrade()
    {
        base.Upgrade();
    }

    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_BowAttackData))
        {
            skillData = (SO_BowAttackData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }

    private void Update()
    {
        GameObject targetPos = GameManager.Instance.scanner.GetNearestEnemy();
        if (CanAttack() && targetPos != null)
        {
            if (reinforcedNum == 1)
            {
                StartCoroutine(Attack_nonGuided());
            }
            else
            {
                Attack();
            }
            lastAttackTime = Time.time;
        }
    }
    private bool CanAttack()
    {
        float coolTime = 0f;
        if (reinforcedNum != 3)
        {
            coolTime = skillData.coolTimeByLevel[itemLevel];
        }
        else
        {
            coolTime = arrowCoolDown;
        }
        coolTime *= GameManager.Instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.Instance.SM.GetFinalValue("N_Cool");
        return Time.time > coolTime + lastAttackTime;
    }

    private void Attack()
    {
        GameObject GO = GameManager.Instance.poolManager.Get(bulletPrefabNum);
        GO.GetComponentInChildren<ArrowBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, skillData.attackInfoByLevel[itemLevel], skillData.maxPenetrationLimit[itemLevel], this);
    }

    private IEnumerator Attack_nonGuided()
    {
        for (int i = 0; i < numofArrow; i++)
        {
            GameObject GO = GameManager.Instance.poolManager.Get(12);
            GO.GetComponentInChildren<RandomArrowBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, skillData.attackInfoByLevel[itemLevel],new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized ,skillData.maxPenetrationLimit[itemLevel],this);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
