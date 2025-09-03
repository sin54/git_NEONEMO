using System.Collections;
using UnityEngine;

public class NonTargetMeleeEnemy : BaseEnemy
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackCool;
    [SerializeField] private float destroyTime;
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

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Invoke("SetDirection", 0.04f);
    }
    private void SetDirection()
    {
        Vector2 dirVec = (Vector2)target.transform.position - RB.position;
        direction = dirVec.normalized * speed * Time.fixedDeltaTime;
    }

    protected override void SetVelocity(float speed, Vector2 angle)
    {
        base.SetVelocity(speed, angle);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time > spawnTime + destroyTime)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanAttack() && eSS.GetStunTime() <= 0f)
        {
            float attackDmg = meleeDamage * eSS.GetAttackScale();
            target.DecreaseHealth(attackDmg);
            lastAttackTime = Time.time;
        }
    }
    private bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCool;
    }
}
