using System.Collections;
using UnityEngine;
using Core;

public class Skill_MagicBullet : BaseSkill
{
    [HideInInspector] public SO_MagicBulletData skillData;
    [SerializeField] private int bulletPrefabNum;
    private GameObject targetPos;
    private float lastAttackTime;
    [Header("ºÐ¿­")]
    public int splitAngle;
    public AttackInfo splitAtkInfo;
    [Header("Æø¹ßÅº")]
    public float boomRadius;
    public float boomDamage;
    [Header("Àú°Ý")]
    public float addDamageAmount;

    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_MagicBulletData))
        {
            skillData = (SO_MagicBulletData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Attack()
    {
        GameObject GO = GameManager.Instance.poolManager.Get(bulletPrefabNum);
        GO.transform.position = transform.position;
        if (reinforcedNum == 3)
        {
            GO.GetComponentInChildren<GunBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, skillData.attackInfoByLevel[itemLevel], this);
        }
        else if (reinforcedNum == 2)
        {
            GO.GetComponentInChildren<GunBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, skillData.attackInfoByLevel[itemLevel], this);
            GO.GetComponentInChildren<GunBullet>().SetBoomEffect(boomRadius, boomDamage);
        }
        else
        {
            GO.GetComponentInChildren<GunBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, skillData.attackInfoByLevel[itemLevel], this);
        }
    }
    private void AttackSided()
    {
        Vector2 direction = (targetPos.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x);

        float angleLeft = angle + Mathf.Deg2Rad * splitAngle;
        float angleRight = angle - Mathf.Deg2Rad * splitAngle;

        Vector2 leftVector = new Vector2(Mathf.Cos(angleLeft), Mathf.Sin(angleLeft)).normalized;
        Vector2 rightVector = new Vector2(Mathf.Cos(angleRight), Mathf.Sin(angleRight)).normalized;
        GameObject GO1 = GameManager.Instance.poolManager.Get(13);
        GO1.GetComponentInChildren<RandomArrowBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime,splitAtkInfo,leftVector,0,this);
        GameObject GO2 = GameManager.Instance.poolManager.Get(13);
        GO2.GetComponentInChildren<RandomArrowBullet>().SetBullet(skillData.arrowSpeedByLevel[itemLevel], skillData.arrowLifeTime, splitAtkInfo, rightVector, 0,this);
    }
    private void Update()
    {
        targetPos = GameManager.Instance.scanner.GetNearestEnemy();
        if (CanAttack() && targetPos != null)
        {
            Attack();
            if (reinforcedNum == 1)
            {
                AttackSided();
            }
            lastAttackTime = Time.time;
        }
    }
    private bool CanAttack()
    {
        float coolTime= skillData.coolTimeByLevel[itemLevel];
        if (GameManager.Instance.playerTypeManager.BT.typeCode == 3)
        {
            coolTime *= 1.5f / GameManager.Instance.player.playerFinalSpeed;
        }
        coolTime *= GameManager.Instance.SM.GetFinalValue("WeaponCoolReduce");
        return Time.time > coolTime + lastAttackTime;
    }
}
