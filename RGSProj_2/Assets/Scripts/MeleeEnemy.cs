using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField]private float meleeDamage;
    [SerializeField]private float attackCool;
    [SerializeField] private bool isElite;
    [SerializeField] private float eliteBaseSpeed;
    [SerializeField] private float eliteBaseShield;
    private float lastAttackTime;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isKnockbacking && eSS.GetStunTime() <= 0f)
        {
            SetVelocity(speed * eSS.GetSpeedScale(), direction);
        }
    }

    protected override void Update()
    {
        base.Update();
        Vector2 dirVec = (Vector2)target.transform.position - RB.position;
        direction = dirVec.normalized;

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&CanAttack()&&eSS.GetStunTime()<=0f)
        {
            float attackDmg = meleeDamage * eSS.GetAttackScale();
            target.DecreaseHealth(attackDmg);
            lastAttackTime = Time.time;
        }
    }

    private bool CanAttack()
    {
        return Time.time>lastAttackTime+attackCool;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (isElite)
        {
            eSS.SetShield(eliteBaseShield);
            eSS.MulSpeedScale(eliteBaseSpeed);
        }
    }
}
