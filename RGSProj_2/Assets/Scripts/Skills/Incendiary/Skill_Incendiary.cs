using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Skill_Incendiary : BaseSkill
{
    [SerializeField] private GameObject incenBullet;
    [HideInInspector] public SO_IncenData skillData;
    private float lastAttackTime;
    [Header("수류탄")]
    public float gradeDamage;
    [Header("피로제")]
    public float slowDownAmount;
    [Header("무자비")]
    public int incenAmount;
    public float incenDamage;
    public float incenRadius;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_IncenData))
        {
            skillData = (SO_IncenData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (CanAttack())
        {
            StartCoroutine(Attack());
            lastAttackTime=Time.time;
        }
    }

    private IEnumerator Attack()
    {
        if (reinforcedNum != 3)
        {
            List<Vector2> attackPoints = GetCirclePoints(skillData.numOfIncen[itemLevel]);
            for (int i = 0; i < attackPoints.Count; i++)
            {
                GameObject incenBullet = GameManager.Instance.poolManager.Get(8);
                incenBullet.transform.position = transform.position;
                incenBullet.GetComponent<IncenBullet>().Attack(attackPoints[i], skillData, itemLevel, this);
                yield return new WaitForSeconds(0.4f / skillData.numOfIncen[itemLevel]);
            }
        }
        else
        {
            List<Vector2> attackPoints = GetCirclePoints(incenAmount);
            for (int i = 0; i < attackPoints.Count; i++)
            {
                GameObject incenBullet = GameManager.Instance.poolManager.Get(8);
                incenBullet.transform.position = transform.position;
                incenBullet.GetComponent<IncenBullet>().Attack(attackPoints[i], skillData, itemLevel, this);
                yield return new WaitForSeconds(0.4f / incenAmount);
            }
        }
    }

    public List<Vector2> GetCirclePoints(int number)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 center = transform.position;
        float initangle = Random.Range(0, 360);
        for (int i = 0; i < number; i++)
        {
            float angle = initangle+i * Mathf.PI * 2f / number; // 라디안 기준 각도
            float x = center.x + skillData.attackRadius * Mathf.Cos(angle);
            float y = center.y + skillData.attackRadius * Mathf.Sin(angle);
            points.Add(new Vector2(x, y));
        }

        return points;
    }
    private bool CanAttack()
    {
        return Time.time > skillData.attackCool[itemLevel]* GameManager.Instance.SM.GetFinalValue("CoolReduce")*GameManager.Instance.SM.GetFinalValue("N_Cool") + lastAttackTime;
    }
}
