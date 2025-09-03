using UnityEngine;

public class SplitEnemy : BaseEnemy
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackCool;
    [SerializeField] private int splitPrefabNum;
    [SerializeField] private int splitNumber;
    [SerializeField] private float spawnRadius;
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

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDeath()
    {
        GameManager.instance.killedEnemy++;
        isDeath = true;
        GameObject DeathParticle = GameManager.instance.poolManager.Get(4, new Vector3(transform.localScale.x / 0.35f, transform.localScale.x / 0.35f));
        DeathParticle.transform.position = transform.position;
        var mainPS = DeathParticle.GetComponent<ParticleSystem>().main;
        mainPS.startColor = enemyColor;
        DropXP();
        currentHealth = maxHealth;
        for (int i = 0; i < splitNumber; i++)
        {
            GameObject enemy = GameManager.instance.poolManager.Get(splitPrefabNum);

            float angle = (360f / splitNumber) * i;
            Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * spawnRadius;

            enemy.transform.position = (Vector2)transform.position + offset;
        }
        gameObject.SetActive(false);
    }
}
