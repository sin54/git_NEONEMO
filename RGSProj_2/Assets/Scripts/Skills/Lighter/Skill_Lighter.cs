using UnityEngine;
using System.Collections;

public class Skill_Lighter : BaseSkill
{
    [HideInInspector] public SO_LighterData skillData;
    private float lastAttackTime;
    [Header("±â¸§Åë")]
    public float attackCool;
    public float windRedAttackCool;
    [Header("ºÒ¾¾")]
    public int targetNum;
    public int fireAmount;
    
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_LighterData))
        {
            skillData = (SO_LighterData)baseSkillData;
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

    private bool CanAttack()
    {
        float coolTime = 0f;
        if (reinforcedNum == 1)
        {
            if (GameManager.instance.player.typeList.Contains(skillData.levelDatas[0].synerge[0]))
            {
                coolTime = attackCool - windRedAttackCool;
            }
            coolTime = attackCool;
        }
        else
        {
            coolTime = skillData.coolTimeByLevel[itemLevel];
        }
        coolTime *= GameManager.instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.instance.SM.GetFinalValue("F_Cool");
        return Time.time > coolTime + lastAttackTime;
    }
    private void Attack()
    {
        if (reinforcedNum == 2)
        {
            GameObject[] targets = GameManager.instance.scanner.FindNearestEnemies(targetNum);
            for (int i = 0; i < Mathf.Min(targets.Length, targetNum); i++)
            {
                if (targets[i] == null) break;
                GameObject GO = GameManager.instance.poolManager.Get(21);
                GO.transform.GetChild(0).gameObject.GetComponent<LighterBullet>().SetBullet(10, 10, new AttackInfo(0, 0),this);
                GO.transform.GetChild(0).gameObject.GetComponent<LighterBullet>().SetTarget(targets[i]);
            }
        }
        else
        {
            GameObject[] targets = GameManager.instance.scanner.FindNearestEnemies(skillData.targetNumByLevel[itemLevel]);
            for (int i = 0; i < Mathf.Min(targets.Length,skillData.targetNumByLevel[itemLevel]); i++)
            {
                GameObject GO = GameManager.instance.poolManager.Get(21);
                GO.transform.GetChild(0).gameObject.GetComponent<LighterBullet>().SetBullet(10, 10, new AttackInfo(0, 0),this);
                GO.transform.GetChild(0).gameObject.GetComponent<LighterBullet>().SetTarget(targets[i]);
            }
        }

    }
}
