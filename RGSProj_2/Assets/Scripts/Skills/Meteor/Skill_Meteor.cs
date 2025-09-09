using System.Collections;
using UnityEngine;
using Core;

public class Skill_Meteor : BaseSkill
{
    [HideInInspector] public SO_MeteorData skillData;
    [SerializeField] private Transform warningSaver;
    [SerializeField] private GameObject meteorWarning;
    private float lastAttackTime;
    [Header("Ãæ°ÝÆÄ")]
    public float allDamageAmount;
    [Header("µå·ÓÅ±")]
    public float stunTime;
    [Header("´ë¸êÁ¾")]
    public float damagePercent;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_MeteorData))
        {
            skillData = (SO_MeteorData)baseSkillData;
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
        coolTime = skillData.coolTimeByLevel[itemLevel];
        coolTime *= GameManager.Instance.SM.GetFinalValue("CoolReduce");
        coolTime *= GameManager.Instance.SM.GetFinalValue("F_Cool");
        return Time.time > coolTime + lastAttackTime;
    }
    private void Attack()
    {
        Vector3[] positions = SetMeteorPosition();
        for (int i = 0; i < skillData.meteorNumByLevel[itemLevel]; i++)
        {
            GameObject GO = GameManager.Instance.poolManager.Get(23);
            GO.GetComponent<Meteorite>().SetMeteor(positions[i], 0.12f, skillData.explosionRadiusByLevel[itemLevel], skillData.attackInfoByLevel[itemLevel],this);
            GO.transform.position=positions[i]+new Vector3(3,10);
        }
        if (reinforcedNum == 1)
        {
            Invoke("ShakeAttack",1.75f);
        }

    }
    private Vector3[] SetMeteorPosition()
    {
        Vector3[] targetPos = new Vector3[skillData.meteorNumByLevel[itemLevel]];
        for (int i = 0; i < skillData.meteorNumByLevel[itemLevel]; i++)
        {
            GameObject GO;
            if (i < warningSaver.childCount)
            {
                GO=warningSaver.GetChild(i).gameObject;
            }
            else
            {
                GO = Instantiate(meteorWarning, transform.position, Quaternion.identity);
            }
            GO.transform.parent = warningSaver;
            GO.SetActive(true);
            StartCoroutine(ActiveFalse(GO));
            Vector3 randTransform = new Vector3(Random.Range(-4f, 4f), Random.Range(-3f, 3f))+GameManager.Instance.player.transform.position;
            GO.transform.position = randTransform;
            float scale = skillData.explosionRadiusByLevel[itemLevel] * GameManager.Instance.SM.GetFinalValue("explosionRad");
            GO.transform.localScale = new Vector3(scale,scale,scale);
            targetPos[i]=randTransform;
        }
        return targetPos;
    }
    void ShakeAttack()
    {
        GameManager.Instance.CM.Shake(0.3f, 0.3f, 0.05f);
        GameManager.Instance.screenScan.AttackAllEnemy(allDamageAmount,AttackType.PhysicAttack,AttackAttr.Fire);
    }
    private IEnumerator ActiveFalse(GameObject GO)
    {
        yield return new WaitForSeconds(1.85f);
        GO.SetActive(false);
    }
}
