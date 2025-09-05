using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

public class Skill_Radar : BaseSkill
{
    [HideInInspector] public SO_RadarData skillData;
    [SerializeField] private LayerMask enemyLayer;
    private float lastAttackTime;
    private List<(BaseEnemy, int)> targetList = new List<(BaseEnemy, int)>();
    [Header("도플러 효과")]
    public float dmgIncAmount;
    [Header("전송 손실")]
    public float healAmount;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_RadarData))
        {
            skillData = (SO_RadarData)baseSkillData;
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
            lastAttackTime= Time.time;
            ReturnSet();
            if (reinforcedNum == 3&&targetList.Count!=0)
            {
                GameManager.Instance.player.noDamage = true;
                Invoke("DefenceUp", targetList.Count * 0.02f);
            }
            StartCoroutine(Attack());
        }
    }
    private bool CanAttack()
    {
        return Time.time > skillData.coolTimeByLevel[itemLevel]* GameManager.Instance.SM.GetFinalValue("CoolReduce") * GameManager.Instance.SM.GetFinalValue("L_Cool") + lastAttackTime;
    }
    private void ReturnSet()
    {
        targetList.Clear();
        Collider2D[] targets_unSelected = Physics2D.OverlapCircleAll(transform.position, skillData.raderRadiusByLevel[itemLevel],enemyLayer);
        List<Collider2D> targets = new List<Collider2D>();
        for(int i = 0; i < targets_unSelected.Length; i++)
        {
            float slowAmount = targets_unSelected[i].GetComponent<BaseEnemy>().eSS.GetSpeedScale();
            int numofArrow = Mathf.CeilToInt((1 - slowAmount) / skillData.divideValueByLevel[itemLevel]);
            for(int j = 0; j < numofArrow; j++)
            {
                targets.Add(targets_unSelected[i]);
            }
        }
        int[] arr=UtilClass.GenerateShuffledArray(targets.Count);
        for(int i=0;i<targets.Count; i++)
        {
            targetList.Add((targets[i].gameObject.GetComponent<BaseEnemy>(), arr[i]));
        }
        targetList.Sort((a, b) => a.Item2.CompareTo(b.Item2));

    }
    private IEnumerator Attack()
    {
        for (int i = 0; i < Mathf.Min(targetList.Count, skillData.maxTargetByLevel[itemLevel]); i++) {
            GameObject GO=GameManager.Instance.poolManager.Get(33);
            Vector2 director = GameManager.Instance.player.transform.position- targetList[i].Item1.transform.position;
            Vector2 spreadDir = Quaternion.Euler(0, 0, Random.Range(-60f, 60f)) * director;
            GO.transform.GetChild(0).GetComponent<LightSpear>().SetBullet(7, 15, reinforcedNum==1?new AttackInfo(skillData.damageByLevel[itemLevel].damage + dmgIncAmount * i, skillData.damageByLevel[itemLevel].knockbackPower):skillData.damageByLevel[itemLevel], spreadDir.normalized, targetList[i].Item1,this);
            yield return new WaitForSeconds(0.02f);
            if (!targetList[i].Item1.gameObject.activeSelf) { break; }
        }

    }
    private void DefenceUp()
    {
        GameManager.Instance.player.noDamage = false;
    }
}
