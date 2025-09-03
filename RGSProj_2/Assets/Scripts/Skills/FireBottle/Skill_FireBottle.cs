using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Skill_FireBottle : BaseSkill
{
    [HideInInspector] public SO_FireBottleData skillData;
    private float lastAttackTime;
    [Header("¸¸À¯ÀÎ·Â")]
    public float centripetalForce;
    [Header("È­¿°ÆøÇ³")]
    public float increasingAmount;
    [Header("ÀÌÄ÷¶óÀÌÀú")]
    public float equalRadius;
    public int equalNum;
    public float equalDistance;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_FireBottleData))
        {
            skillData = (SO_FireBottleData)baseSkillData;
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
            if (reinforcedNum == 3)
            {
                StartCoroutine(EqualAttack());
                lastAttackTime = Time.time;
            }
            else
            {
                StartCoroutine(Attack());
                lastAttackTime = Time.time;
            }

        }
    }

    private IEnumerator Attack()
    {
        List<Vector2> attackPoints = GetRandomPoints(skillData.numOfIncen[itemLevel]);
        for (int i = 0; i < attackPoints.Count; i++)
        {
            GameObject incenBullet = GameManager.instance.poolManager.Get(25);
            incenBullet.transform.position = transform.position;
            incenBullet.GetComponent<FireBottleBullet>().Attack(attackPoints[i], skillData, itemLevel, this);
            yield return new WaitForSeconds(0.4f / skillData.numOfIncen[itemLevel]);
        }
    }
    private IEnumerator EqualAttack()
    {
        List<Vector2> attackPoints = GetEqualPoints(equalNum);
        for (int i = 0; i < attackPoints.Count; i++)
        {
            GameObject incenBullet = GameManager.instance.poolManager.Get(25);
            incenBullet.transform.position = transform.position;
            incenBullet.GetComponent<FireBottleBullet>().Attack(attackPoints[i], skillData, itemLevel, this);
            yield return new WaitForSeconds(0.16f);
        }
    }

    private List<Vector2> GetEqualPoints(int number)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 newVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        for (int i = 0; i < number; i++)
        {
            points.Add((Vector2)transform.position+newVec * (i + 0.5f)*equalDistance);
        }
        return points;
    }
    private List<Vector2> GetRandomPoints(int number)
    {
        List<Vector2> points = new List<Vector2>();
        for(int i = 0; i < number; i++)
        {
            points.Add(new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f))+(Vector2)transform.position);
        }
        return points;
    }
    private bool CanAttack()
    {
        return Time.time > skillData.attackCool[itemLevel]*GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("F_Cool") + lastAttackTime;
    }
}
