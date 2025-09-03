using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Skill_WindCharge : BaseSkill
{
    [HideInInspector] public SO_WindChargeData skillData;
    private float lastAttackTime;
    private GameObject targetPos;
    [Header("¹«¿ªÇ³")]
    public AttackInfo dmg_rf1;
    public float size_rf1;
    [Header("Æí¼­Ç³")]
    public float sideSpeed_rf2;
    public float angle_rf2;
    public float size_rf2;  
    public AttackInfo dmg_rf2;
    [Header("±Øµ¿Ç³")]
    public int num_rf3;
    public float size_rf3;
    public AttackInfo dmg_rf3;
    public float rangeAngle_rf3;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_WindChargeData))
        {
            skillData = (SO_WindChargeData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Update()
    {
        targetPos = GameManager.instance.scanner.GetNearestEnemy();
        if (CanAttack()&&targetPos!=null)
        {
            Attack();
            if (reinforcedNum == 2)
            {
                AttackSided();
            }
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        if (reinforcedNum == 1)
        {
            GameObject windBullet = GameManager.instance.poolManager.Get(27);
            
            windBullet.transform.GetChild(0).GetComponent<WindCharge_Bullet>().SetBullet(skillData.bulletSpeed, 15f, dmg_rf1, 100, this);
            float size = size_rf1;
            windBullet.transform.localScale = new Vector3(size,size,size);
            windBullet.transform.GetChild(1).transform.localScale = new Vector3(size / 2, size / 2);
        }
        else if (reinforcedNum == 2) {
            GameObject windBullet = GameManager.instance.poolManager.Get(27);
            windBullet.transform.GetChild(0).GetComponent<WindCharge_Bullet>().SetBullet(skillData.bulletSpeed, 15f, skillData.atkInfoByLevel[itemLevel], 100, this);
            windBullet.transform.localScale = new Vector3(skillData.atkRadByLevel[itemLevel], skillData.atkRadByLevel[itemLevel], skillData.atkRadByLevel[itemLevel]);
            windBullet.transform.GetChild(1).transform.localScale = new Vector3(skillData.atkRadByLevel[itemLevel] / 2, skillData.atkRadByLevel[itemLevel] / 2);
        }
        else if (reinforcedNum == 3)
        {
            StartCoroutine(PolarWind());
        }
        else
        {
            GameObject windBullet = GameManager.instance.poolManager.Get(27);
            windBullet.transform.GetChild(0).GetComponent<WindCharge_Bullet>().SetBullet(skillData.bulletSpeed, 15f, skillData.atkInfoByLevel[itemLevel], 100, this);
            windBullet.transform.localScale = new Vector3(skillData.atkRadByLevel[itemLevel], skillData.atkRadByLevel[itemLevel], skillData.atkRadByLevel[itemLevel]);
            windBullet.transform.GetChild(1).transform.localScale = new Vector3(skillData.atkRadByLevel[itemLevel] / 2, skillData.atkRadByLevel[itemLevel] / 2);
        }

    }
    private void AttackSided()
    {

        Vector2 direction = (targetPos.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x);

        float angleLeft = angle + Mathf.Deg2Rad * angle_rf2;
        float angleRight = angle - Mathf.Deg2Rad * angle_rf2;

        Vector2 leftVector = new Vector2(Mathf.Cos(angleLeft), Mathf.Sin(angleLeft)).normalized;
        Vector2 rightVector = new Vector2(Mathf.Cos(angleRight), Mathf.Sin(angleRight)).normalized;

        GameObject GO1 = GameManager.instance.poolManager.Get(39);
        GO1.transform.GetChild(0).GetComponent<WindCharge_NoTargetBullet>().SetBullet(sideSpeed_rf2, 15f, dmg_rf2,leftVector,100, this);
        float size = size_rf2;
        GO1.transform.localScale = new Vector3(size, size, size);
        GO1.transform.GetChild(1).transform.localScale = new Vector3(size / 2, size / 2);
        GameObject GO2 = GameManager.instance.poolManager.Get(39);
        GO2.transform.GetChild(0).GetComponent<WindCharge_NoTargetBullet>().SetBullet(sideSpeed_rf2, 15f, dmg_rf2, rightVector, 100,this);
        GO2.transform.localScale = new Vector3(size, size, size);
        GO2.transform.GetChild(1).transform.localScale = new Vector3(size / 2, size / 2);
    }
    private IEnumerator PolarWind()
    {
        Vector2 pivotAng = targetPos.transform.position - transform.position;
        for (int i = 0; i < num_rf3; i++)
        {
            GameObject GO = GameManager.instance.poolManager.Get(39);
            Vector2 randomAng = UtilClass.RotateVector2(pivotAng, Random.Range(-rangeAngle_rf3, rangeAngle_rf3)).normalized;
            GO.transform.GetChild(0).GetComponent<WindCharge_NoTargetBullet>().SetBullet(skillData.bulletSpeed, 15f, dmg_rf3, randomAng, 100,this);
            float size = size_rf3;
            GO.transform.localScale = new Vector3(size, size, size);
            GO.transform.GetChild(1).transform.localScale = new Vector3(size / 2, size / 2);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private bool CanAttack()
    {
        return Time.time > skillData.atkCoolByLevel[itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("W_Cool") + lastAttackTime;
    }
}
