using Core;
using UnityEngine;

public class Skill_Laser : BaseSkill
{
    [HideInInspector] public SO_LaserData skillData;
    private float lastAttackTime;
    private float distance = 0.25f; // 원하는 거리
    [Header("냉각포")]
    public float laserWidth;
    [Header("적염포")]
    public float laserTime;
    [Header("회전포")]
    public float angularVelocity;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_LaserData))
        {
            skillData = (SO_LaserData)baseSkillData;
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
        if (reinforcedNum == 3)
        {
            Vector2 spawnPos = GetRandomPosition(transform);
            for (int i = 0; i < 4; i++)
            {
                float ang = Mathf.Atan2(spawnPos.y - transform.position.y, spawnPos.x - transform.position.x);
                GameObject GO = GameManager.Instance.poolManager.Get(29);
                GO.transform.position = GameManager.Instance.player.transform.position;
                GO.transform.rotation = Quaternion.Euler(0, 0, ang * Mathf.Rad2Deg+90f*i);
                if (reinforcedNum == 2)
                {
                    GO.transform.GetChild(0).GetComponent<Laser>().SetLaser(skillData.damageByLevel[itemLevel], laserTime, this);
                }
                else
                {
                    GO.transform.GetChild(0).GetComponent<Laser>().SetLaser(skillData.damageByLevel[itemLevel], 1, this);
                }
                if (reinforcedNum == 1)
                {
                    GO.transform.localScale = new Vector3(laserWidth, laserWidth, laserWidth);
                }
                else
                {
                    GO.transform.localScale = new Vector3(skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel]);
                }

            }
        }
        else
        {
            for (int i = 0; i < skillData.laserNumByLevel[itemLevel]; i++)
            {
                Vector2 spawnPos = GetRandomPosition(transform);
                float ang = Mathf.Atan2(spawnPos.y - transform.position.y, spawnPos.x - transform.position.x);
                GameObject GO = GameManager.Instance.poolManager.Get(29);
                GO.transform.position = spawnPos;
                GO.transform.rotation = Quaternion.Euler(0, 0, ang * Mathf.Rad2Deg);
                if (reinforcedNum == 2)
                {
                    GO.transform.GetChild(0).GetComponent<Laser>().SetLaser(skillData.damageByLevel[itemLevel], laserTime, this);
                }
                else
                {
                    GO.transform.GetChild(0).GetComponent<Laser>().SetLaser(skillData.damageByLevel[itemLevel], 1, this);
                }
                if (reinforcedNum == 1)
                {
                    GO.transform.localScale = new Vector3(laserWidth, laserWidth, laserWidth);
                }
                else
                {
                    GO.transform.localScale = new Vector3(skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel]);
                }

            }
        }

        
    }
    private bool CanAttack()
    {
        return Time.time > skillData.coolTimeByLevel[itemLevel]* GameManager.Instance.SM.GetFinalValue("CoolReduce") * GameManager.Instance.SM.GetFinalValue("L_Cool") + lastAttackTime;
    }


    public Vector2 GetRandomPosition(Transform baseTransform)
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * distance;

        Vector3 randomPosition = baseTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        return randomPosition;
    }
}
