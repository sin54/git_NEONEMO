using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashEnemy : BaseEnemy
{
    [SerializeField] private bool isElite;
    [SerializeField] private GameObject dashGO;
    [SerializeField] private ParticleSystem PS;
    private DashEnemy_DashAttack DDA;
    private SpriteRenderer dashSR;
    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackCool;

    private bool isRoaring;
    private float lastAttackTime;

    public float dashDamage;
    [SerializeField] private float dashRange;
    [SerializeField] private float dashCool;
    [SerializeField] private float dashChargeTime;
    [SerializeField] private float shieldAmount;

    private float lastDashTime;
    private bool isDashing;
    private float distFromEnemy;
    private Coroutine checkCoroutine;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isKnockbacking && eSS.GetStunTime() <= 0f&&!isDashing)
        {
            SetVelocity(speed * eSS.GetSpeedScale(), direction);
        }
    }

    protected override void Update()
    {
        base.Update();
        Vector2 dirVec = (Vector2)target.transform.position - RB.position;
        direction = dirVec.normalized;
        if (!isDashing && distFromEnemy < dashRange&&Time.time>lastDashTime+dashCool)
        {

            DashAttack();
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanAttack() && eSS.GetStunTime() <= 0f&&!isRoaring)
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

    private void DashAttack()
    {
        unStoppable = true;
        isDashing = true;
        dashGO.SetActive(true);
        Vector3 dG = dashGO.transform.localScale;
        
        dG.y = isElite?dashRange:dashRange * 2;
        dashGO.transform.localScale = dG;
        dashSR.material.SetFloat("_Cutoff", 0);
        SetDashObjAlpha(0.2f);
        StartCoroutine(showDashRange());
    }

    private IEnumerator showDashRange()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dashGO.transform.rotation = Quaternion.Euler(0f, 0f, angle+90f);
        Vector2 targetPos = (Vector2)transform.position + dashRange * direction.normalized;
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            dashSR.material.SetFloat("_Cutoff", t);
            yield return null;
        }

        dashSR.material.SetFloat("_Cutoff", 1f);
        yield return new WaitForSeconds(0.25f);
        duration = dashChargeTime;
        float startAlpha = 0.2f;
        float endAlpha = 1.0f;
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            SetDashObjAlpha(alpha);
            yield return null;
        }

        SetDashObjAlpha(endAlpha); // 마지막 보정
        yield return new WaitForSeconds(0.15f);
        Vector2 startPos = transform.position;
        isRoaring = true;
        elapsed = 0f;
        duration = 0.1f;

        dashSR.material.SetFloat("_Cutoff", 0);
        SetDashObjAlpha(0f);
        DDA.Attack(dashDamage);
        PS.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }
        if (isElite)
        {
            eSS.AddShield(shieldAmount);
        }
        PS.Stop();
        isRoaring = false;
        transform.position = targetPos;
        unStoppable = false;
        isDashing = false;
        lastDashTime = Time.time;
        dashGO.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isDashing = false;
        dashGO.SetActive(false);
        dashSR.material.SetFloat("_Cutoff", 0);
        SetDashObjAlpha(0f);
        checkCoroutine = StartCoroutine(CheckDistanceRoutine());
        isRoaring = false;
        PS.Stop();
    }
    protected override void OnDisable()
    {
        if (checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
        }
    }
    private IEnumerator CheckDistanceRoutine()
    {
        while (true)
        {
            if (target != null)
            {
                distFromEnemy = Vector2.Distance(target.transform.position, transform.position);

            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected override bool IsMassLocked()
    {
        return isDashing||base.IsMassLocked();
    }

    protected override void Awake()
    {
        base.Awake();
        dashSR = dashGO.GetComponent<SpriteRenderer>();
        DDA=GetComponentInChildren<DashEnemy_DashAttack>();
    }

    private void SetDashObjAlpha(float alpha)
    {
        Color dC = dashSR.color;
        dC.a = alpha;
        dashSR.color = dC;
    }

}
