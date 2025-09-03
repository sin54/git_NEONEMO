using UnityEngine;
using System.Collections;
public class Skill_WindSword :BaseSkill
{
    [HideInInspector] public SO_WindSwordData skillData;
    private float lastAttackTime;
    [Header("2fast4u")]
    public float atkMul_rf1;
    [Header("Ди°Л")]
    public int targetNum_rf2;
    public float damage_rf2;
    public float size_rf2;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_WindSwordData))
        {
            skillData = (SO_WindSwordData)baseSkillData;
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

    private IEnumerator Attack()
    {
        if (reinforcedNum == 2)
        {
            GameObject[] targets = GameManager.instance.scanner.FindNearestEnemies(targetNum_rf2);
            for (int i = 0; i < Mathf.Min(targetNum_rf2, targets.Length); i++)
            {
                GameObject GO = GameManager.instance.poolManager.Get(28);
                GO.GetComponent<WindSwordParticle>().SetParticle(skillData.damageByLevel[itemLevel], this);
                GO.transform.position = targets[i].transform.position;
                GO.transform.localScale = new Vector3(size_rf2,size_rf2,size_rf2);
                GO.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            GameObject[] targets = GameManager.instance.scanner.FindNearestEnemies(skillData.swordNumByLevel[itemLevel]);
            for (int i = 0; i < Mathf.Min(skillData.swordNumByLevel[itemLevel], targets.Length); i++)
            {
                GameObject GO = GameManager.instance.poolManager.Get(28);
                GO.GetComponent<WindSwordParticle>().SetParticle(skillData.damageByLevel[itemLevel], this);
                GO.transform.position = targets[i].transform.position;
                GO.transform.localScale = new Vector3(skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel], skillData.sizeByLevel[itemLevel]);
                GO.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
    private bool CanAttack()
    {
        return Time.time > skillData.atkCoolByLevel[itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("W_Cool") + lastAttackTime;
    }
}
