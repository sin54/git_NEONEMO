using UnityEngine;
using System.Collections;
using Core;
public class Skill_FireBall : BaseSkill
{
    [HideInInspector] public SO_FireBallData skillData;
    private float lastAttackTime;
    [Header("»ø¸®¸à´õ")]
    public int ballAmount = 6;
    public float splitAngle = 5f;
    [Header("Çª¸¥ ºÒ²É")]
    public int givingFire;
    public int lightAddFire;
    public Gradient BlueFire;
    [Header("ÀÏÃ¼È­")]
    public float Lsize;
    public float Ldamage;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_FireBallData))
        {
            skillData = (SO_FireBallData)baseSkillData;
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
            StartCoroutine(Attack());
            lastAttackTime = Time.time;
        }
    }

    private bool CanAttack()
    {
        float coolTime = 0f;
        coolTime = skillData.coolTimeByLevel[itemLevel];
        coolTime *= GameManager.Instance.SM.GetFinalValue("CoolReduce")* GameManager.Instance.SM.GetFinalValue("F_Cool");
        return Time.time > coolTime + lastAttackTime;
    }
    private IEnumerator Attack()
    {
        if (reinforcedNum == 1)
        {
            Vector2 targetVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            for (int i = 0; i < ballAmount; i++)
            {
                GameObject GO = GameManager.Instance.poolManager.Get(19);
                Vector2 direction = (targetVec + (Vector2)(Quaternion.Euler(0, 0, i * splitAngle) * targetVec)).normalized;
                GO.GetComponentInChildren<FireBallBullet>().SetBullet(skillData.fireBallSpeed, skillData.fireBallTime, direction, 0, skillData.explosionRadiusByLevel[itemLevel], skillData.damageByLevel[itemLevel],2);
                yield return new WaitForSeconds(0.1f);
            }

        }
        else if (reinforcedNum == 3)
        {
            GameObject GO = GameManager.Instance.poolManager.Get(19);
            GO.GetComponentInChildren<FireBallBullet>().SetBullet(skillData.fireBallSpeed, skillData.fireBallTime, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, 0, Lsize, Ldamage, 10);
            GO.transform.GetChild(1).localScale = new Vector3(Lsize, Lsize, Lsize);
            GO.transform.GetChild(0).localScale = new Vector3(Lsize/4, Lsize/4, Lsize/4);
        }
        else
        {
            for (int i = 0; i < skillData.fireBallAmountByLevel[itemLevel]; i++)
            {
                GameObject GO = GameManager.Instance.poolManager.Get(19);
                if (reinforcedNum == 2)
                {
                    if (GameManager.Instance.player.typeList.Contains(skillData.levelDatas[1].synerge[0]))
                    {
                        GO.GetComponentInChildren<FireBallBullet>().SetBullet(skillData.fireBallSpeed, skillData.fireBallTime, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, 0, skillData.explosionRadiusByLevel[itemLevel], 0, givingFire+lightAddFire);
                        var PSMain = GO.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                        PSMain.color= BlueFire;
                    }
                    else
                    {
                        GO.GetComponentInChildren<FireBallBullet>().SetBullet(skillData.fireBallSpeed, skillData.fireBallTime, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, 0, skillData.explosionRadiusByLevel[itemLevel], 0, givingFire);
                    }

                }
                else
                {
                    GO.GetComponentInChildren<FireBallBullet>().SetBullet(skillData.fireBallSpeed, skillData.fireBallTime, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized, 0, skillData.explosionRadiusByLevel[itemLevel], skillData.damageByLevel[itemLevel], 4);

                }
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}
