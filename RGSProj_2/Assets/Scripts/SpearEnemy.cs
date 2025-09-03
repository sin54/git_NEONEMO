using System.Collections;
using UnityEngine;

public class SpearEnemy : BaseEnemy
{
    [SerializeField] private GameObject trail;

    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackCool;
    public float spearAttackCool;
    private float lastAttackTime;

    public float spearDamage;
    [SerializeField] private float spearCool;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    private bool isDashing;
    private float lastSpearAttackTime;


    protected override void Update()
    {
        base.Update();
        if (!isDashing)
        {
            SetDirection();
        }
        if (CanChargeAttack() && eSS.GetStunTime() <= 0f)
        {
            lastSpearAttackTime = Time.time;
            trail.SetActive(true);
            isDashing=true;
            unStoppable = true;
            StartCoroutine(Dash());
        }
    }
    private void SetDirection()
    {
        Vector2 dirVec = (Vector2)target.transform.position - RB.position;
        direction = dirVec.normalized * speed * Time.fixedDeltaTime;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
    private IEnumerator Dash()
    {
        SetVelocity(dashSpeed * eSS.GetSpeedScale(), direction);
        yield return new WaitForSeconds(dashTime);
        SetVelocityZero();

        isDashing = false;
        unStoppable = false;
        trail.SetActive(false);
    }

    private bool CanChargeAttack()
    {
        return Time.time > lastSpearAttackTime + spearCool;
    }
    private bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCool;
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
}
