using System.Collections;
using TMPro;
using UnityEngine;
using Core;

public class ExplodeEnemy : BaseEnemy
{
    [SerializeField] private float explodeDamage;
    [SerializeField] private float explodeRadius;
    [SerializeField] private TMP_Text countDown;
    [SerializeField] private Gradient Grad;
    [SerializeField] private float explosionTime;
    [SerializeField] private float scaleMul;
    [SerializeField] private bool isElite;
    [SerializeField] private float shieldAmount;

    private GameObject explodeRange;

    private bool exploding;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isKnockbacking && eSS.GetStunTime() <= 0f&&(!exploding||isElite))
        {
            SetVelocity(speed * eSS.GetSpeedScale(), direction);
        }

    }

    protected override void OnDeath()
    {
        if (!exploding)
        {
            Explode();
        }
        else
        {
            if (isElite)
            {
                GameObject DeathParticle = GameManager.Instance.poolManager.Get(4, new Vector3(transform.localScale.x / 0.35f, transform.localScale.x / 0.35f));
                DeathParticle.transform.position = transform.position;
                var mainPS = DeathParticle.GetComponent<ParticleSystem>().main;
                mainPS.startColor = enemyColor;
                GameManager.Instance.killedEnemy++;
                DropXP();
                ExplodeWithNoCool();
            }
        }

    }


    protected override void Update()
    {
        base.Update();
        Vector2 dirVec = (Vector2)target.transform.position - RB.position;
        direction = dirVec.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ExplodeWithNoCool();
        }
    }

    private void Explode()
    {

        exploding = true;
        if (isElite)
        {
            eSS.MulSpeedScale(1.3f);
            eSS.AddShield(shieldAmount);
            currentHealth = 1;
        }
        else
        {
            gameObject.layer = 0;
            noDamage = true;
        }
        StartCoroutine(DelayExplode());
    }

    private void ExplodeWithNoCool()
    {
        GameObject GO2 = GameManager.Instance.poolManager.Get(48);
        GO2.transform.GetChild(0).GetComponent<EnemyExplodeEffect>().SetExplosion(explodeRadius, explodeDamage, transform.position);
        countDown.text = "";
        isDeath = true;
        currentHealth = maxHealth;
        if (explodeRange != null)
        {
            explodeRange.transform.SetParent(GameManager.Instance.poolManager.transform);
            explodeRange.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    protected override bool IsMassLocked()
    {
        return base.IsMassLocked()||(exploding&&!isElite);
    }
    private IEnumerator DelayExplode()
    {
        explodeRange = GameManager.Instance.poolManager.Get(46);
        explodeRange.transform.SetParent(transform);
        explodeRange.transform.position = transform.position;
        explodeRange.transform.localScale = new Vector3(explodeRadius*scaleMul, explodeRadius*scaleMul, explodeRadius * scaleMul);
        explodeRange.GetComponent<EnemyBeforeExplode>().Play(Grad, explosionTime);
        for (int i = 0; i < Mathf.CeilToInt(explosionTime / 0.1f); i++)
        {
            countDown.text = (explosionTime - i/10.0f).ToString("F1");
            yield return new WaitForSeconds(0.1f);
        }
        countDown.text = "";
        GameManager.Instance.killedEnemy++;
        isDeath = true;
        GameObject DeathParticle = GameManager.Instance.poolManager.Get(4, new Vector3(transform.localScale.x / 0.35f, transform.localScale.x / 0.35f));
        DeathParticle.transform.position = transform.position;
        var mainPS = DeathParticle.GetComponent<ParticleSystem>().main;
        mainPS.startColor = enemyColor;
        DropXP();
        currentHealth = maxHealth;
        explodeRange.transform.SetParent(GameManager.Instance.poolManager.transform);
        explodeRange.SetActive(false);
        GameObject GO2 = GameManager.Instance.poolManager.Get(48);
        GO2.transform.GetChild(0).GetComponent<EnemyExplodeEffect>().SetExplosion(explodeRadius, explodeDamage, transform.position);
        gameObject.SetActive(false);

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RB.mass = 1;
        countDown.text = "";
        gameObject.layer = 8;
        exploding = false;
    }
}
