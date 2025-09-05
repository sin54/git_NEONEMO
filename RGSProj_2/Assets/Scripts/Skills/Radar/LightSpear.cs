using UnityEngine;
using Core;

public class LightSpear : BaseBullet
{
    public float nonGuidedTime;
    private Transform targetPos;
    private BaseEnemy targetEnemy;
    private Skill_Radar skillRadar;
    [SerializeField] private float rotationSpeed;


    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time > nonGuidedTime + spawnTime) 
        {
            RB.linearVelocity = new Vector2(0, 0);
            if (!targetPos.gameObject.activeSelf||!(targetPos.gameObject.layer==8))
            {
                transform.parent.gameObject.SetActive(false);
                return;
            }

            // 방향 벡터 계산
            Vector2 direction = (Vector2)targetPos.position - (Vector2)transform.parent.position;
            direction.Normalize();

            // 현재 회전값과 목표 회전값의 차이 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, angle, rotationSpeed * Time.deltaTime);

            // 회전 적용
            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            // 앞으로 이동
            transform.parent.position += transform.right * bulletSpeed * Time.deltaTime;
        }
        else
        {
            RB.linearVelocity = bulletDirection * bulletSpeed;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.parent.position = GameManager.Instance.player.transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.gameObject.GetComponent<BaseEnemy>() == targetEnemy)
            {
                bool isDead=GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),attackInfo,AttackType.MagicAttack, AttackAttr.Light,transform.position - GameManager.Instance.player.transform.position);
                if (skillRadar.reinforcedNum == 2 && isDead)
                {
                    GameManager.Instance.player.IncreaseHealth(skillRadar.healAmount);
                }
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

    }
    public void SetBullet(float bSpeed, float bLifeTime, AttackInfo aInfo, Vector2 ang,BaseEnemy BE,Skill_Radar sr)
    {
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        attackInfo = aInfo;
        bulletDirection = ang;
        float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        targetPos = BE.transform;
        targetEnemy = BE;
        skillRadar = sr;
    }
}
