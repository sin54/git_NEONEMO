using UnityEngine;
using System.Collections;

public class Skill_ShootingStar : BaseSkill
{
    [HideInInspector] public SO_ShootingStarData skillData;
    private float lastAttackTime;
    [Header("별 헤는 밤")]
    public int numofStar;
    [Header("별조각")]
    public float healAmount;
    public float starLifeTime;
    public float fracPercent;
    [Header("피날레")]
    public AttackInfo lastDamage;
    public float lastRadius;
    public float lastStarSize;
    public Gradient lastStarColor;

    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_ShootingStarData))
        {
            skillData = (SO_ShootingStarData)baseSkillData;
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
            lastAttackTime= Time.time;
        }
    }

    private IEnumerator Attack()
    {
        for (int i = 0; i < (reinforcedNum==1?numofStar:skillData.numOfStar[itemLevel]); i++) {
            Vector3 randTransform = new Vector3(Random.Range(-4f, 4f), Random.Range(-3.5f, 3.5f)) + GameManager.instance.player.transform.position;
            GameObject GO = GameManager.instance.poolManager.Get(30);
            GO.GetComponent<StarObj>().SetStar(randTransform, 0.08f, skillData.explosionRadiusByLevel[itemLevel], skillData.explosionDamageByLevel[itemLevel],this,false);
            GO.transform.position = randTransform + new Vector3(-5, 10);
            yield return new WaitForSeconds(0.25f);
        }
        if (reinforcedNum == 3)
        {
            Vector3 randTransform = new Vector3(Random.Range(-4f, 4f), Random.Range(-3.5f, 3.5f)) + GameManager.instance.player.transform.position;
            GameObject GO = GameManager.instance.poolManager.Get(30);
            GO.GetComponent<StarObj>().SetStar(randTransform, 0.06f, lastRadius, lastDamage, this,true);
            GO.transform.localScale = new Vector3(lastStarSize, lastStarSize, lastStarSize);
            var PS = GO.GetComponent<ParticleSystem>().colorOverLifetime;
            var PS2=GO.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
            PS.color = lastStarColor;
            PS2.color = lastStarColor;
            GO.transform.position = randTransform + new Vector3(-5, 10);
            GO.transform.GetChild(0).localScale= new Vector3(lastStarSize, lastStarSize, lastStarSize);
        }

    }
    private bool CanAttack()
    {
        return Time.time > skillData.coolTimeByLevel[itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("L_Cool") + lastAttackTime;
    }
}
