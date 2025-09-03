using System.Collections;
using TMPro;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    protected Rigidbody2D RB;
    protected SpriteRenderer SR;
    protected Player target;
    public EnemyStateSetter eSS;

    protected Vector2 direction;

    public float speed;
    protected bool isKnockbacking;
    protected float spawnTime;
    private float knockbackStartTime;
    [SerializeField]protected bool isDeath = false;

    protected bool unStoppable = false;
    protected bool noDamage = false;

    [SerializeField] private float knockbackResistance;

    [SerializeField] protected Color enemyColor;
    [SerializeField] private Transform floatingTxtPos;
    [SerializeField] private XPDropInfo XPDrop;
    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SR=GetComponent<SpriteRenderer>();
        eSS=GetComponentInChildren<EnemyStateSetter>(); 
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        target = GameManager.instance.player;

    }
    protected virtual void OnEnable()
    {
        unStoppable = false;
        noDamage = false;

        currentHealth = maxHealth;
        isKnockbacking = false;
        SR.color=enemyColor;

        spawnTime = Time.time;
        isDeath = false;
        Spawner.OnMassEnemyDeath += EndWave;
        StartCoroutine(TickDamage());
    }
    protected virtual void OnDisable()
    {
        Spawner.OnMassEnemyDeath -= EndWave;
    }
    protected virtual void Update()
    {
        if (isKnockbacking)
        {
            if (Time.time > knockbackStartTime + 0.1f)
            {
                isKnockbacking = false;
                SetVelocityZero();
            }
        }

        UpdateMass();

    }
    protected virtual void FixedUpdate()
    {
    }

    public virtual bool HasAttacked(AttackInfo attackinfo,Vector2 knockbackDirection,AttackType attacktype,bool isC)
    {
        if (isDeath||noDamage) return false;
        float finalDmg = ReduceHealth(attackinfo.damage);
        SpawnFloatingTxt(finalDmg, isC, attacktype);
        OnHit(attackinfo,knockbackDirection.normalized);
        if (currentHealth <= 0&&!isDeath) {
            OnDeath();
            return true;
        }
        return false;
    }

    public virtual bool HasAttacked(float damage,AttackType attacktype,bool isC)
    {
        if (damage <= 0||isDeath||noDamage) return false;
        float finalDmg=ReduceHealth(damage);
        SpawnFloatingTxt(finalDmg, isC, attacktype);
        if (gameObject.activeSelf)
        {
            StartCoroutine(HitColorChange());
        }
        if (currentHealth <= 0)
        {
            OnDeath();
            return true;
        }
        return false;
    }
    private float ReduceHealth(float damage)
    {
        float finalDmg=damage * eSS.GetDefenceScale();
        float save = finalDmg;
        float currentShield = eSS.GetShield();
        if (currentShield > 0)
        {
            if (currentShield >= finalDmg)
            {
                eSS.ReduceShield(finalDmg);
                finalDmg = 0f;
            }
            else
            {
                finalDmg -= eSS.GetShield();
                eSS.SetShield(0);
            }
        }
        currentHealth -= finalDmg;
        if (currentHealth > 0)
        {
            canExecute();
        }

        return save;
    }
    protected virtual void OnHit(AttackInfo attackinfo,Vector2 knockbackDirection)
    {
        if (gameObject.activeSelf&&attackinfo.damage>0)
        {
            StartCoroutine(HitColorChange());
        }
        if (attackinfo.knockbackPower != 0&&!unStoppable)
        {
            isKnockbacking = true;
            knockbackStartTime = Time.time;
            SetVelocity(attackinfo.knockbackPower*GameManager.instance.SM.GetFinalValue("KnockBackMul")*knockbackResistance, knockbackDirection);
        }
    }

    private void SpawnFloatingTxt(float damage,bool isCrit,AttackType attacktype)
    {
        if (damage <= 0||isDeath) return;
        GameObject floatingTxt=GameManager.instance.poolManager.Get(3);
        floatingTxt.transform.position = floatingTxtPos.position;
        floatingTxt.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        TMP_Text floatingTxt_TMP = floatingTxt.GetComponentInChildren<TextMeshPro>();
        floatingTxt_TMP.text = (Mathf.RoundToInt(damage)).ToString();
        floatingTxt_TMP.fontSize = Mathf.Clamp(damage / 10, 2.5f, 7);
        if (attacktype == AttackType.StaticAttack)
        {
            floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[0];
        }
        else if(attacktype== (AttackType.PhysicAttack | AttackType.MagicAttack))
        {
            if (isCrit)
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[6];
            }
            else
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[5];
            }
        }
        else if (attacktype == AttackType.PhysicAttack)
        {
            if (isCrit)
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[2];
            }
            else
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[1];
            }
        }
        else if (attacktype == AttackType.MagicAttack)
        {
            if (isCrit)
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[4];
            }
            else
            {
                floatingTxt_TMP.color = GameManager.instance.floatingTxtColors[3];
            }
        }
    }

    private IEnumerator HitColorChange()
    {
        SR.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        SR.color = enemyColor;
    }
    protected virtual void OnDeath()
    {
        if (isDeath) return;
        GameManager.instance.killedEnemy++;
        isDeath=true;
        GameObject DeathParticle=GameManager.instance.poolManager.Get(4,new Vector3(transform.localScale.x/0.35f,transform.localScale.x/0.35f));
        DeathParticle.transform.position = transform.position;
        var mainPS = DeathParticle.GetComponent<ParticleSystem>().main;
        mainPS.startColor=enemyColor;
        DropXP();
        currentHealth = maxHealth;
        gameObject.SetActive(false);
    }
    public void canExecute()
    {
        if (GameManager.instance.CanExecute(this))
        {
            if (!isDeath)
            {
                OnExecution();
            }
        }
    }
    protected virtual void OnExecution()
    {
        if (isDeath) return;
        GameManager.instance.killedEnemy++;
        isDeath = true;
        GameObject DeathParticle = GameManager.instance.poolManager.Get(58, new Vector3(transform.localScale.x / 0.35f, transform.localScale.x / 0.35f));
        DeathParticle.transform.position = transform.position;
        GameObject DeathParticle2 = GameManager.instance.poolManager.Get(59, new Vector3(transform.localScale.x/2.6f, transform.localScale.x/2.6f));
        DeathParticle2.transform.position = transform.position;
        DropXP();
        currentHealth = maxHealth;
        gameObject.SetActive(false);
    }
    protected void DropXP()
    {
        for (int i = 0; i < XPDrop.XP1amount; i++)
        {
            float rnd = Random.value;
            if (rnd < XPDrop.percentXP1)
            {
                float gap = (Random.value - 0.5f) / 2;
                GameObject GO=GameManager.instance.poolManager.Get(5);
                GO.transform.position = new Vector3(transform.position.x + gap, transform.position.y + gap, transform.position.z);
                GO.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                GO.GetComponent<BaseItem>().isCollected = false;
            }
        }
        for (int i = 0; i < XPDrop.XP5amount; i++)
        {
            float rnd = Random.value;
            if (rnd<XPDrop.percentXP2)
            {
                float gap = (Random.value - 0.5f) / 2;
                GameObject GO = GameManager.instance.poolManager.Get(6);
                GO.transform.position = new Vector3(transform.position.x + gap, transform.position.y + gap, transform.position.z);
                GO.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                GO.GetComponent<BaseItem>().isCollected = false;
            }
        }
        for (int i = 0; i < XPDrop.XP25amount; i++)
        {
            float rnd = Random.value;
            if (rnd < XPDrop.percentXP3)
            {
                float gap = (Random.value - 0.5f) / 2;
                GameObject GO = GameManager.instance.poolManager.Get(7);
                GO.transform.position = new Vector3(transform.position.x +gap, transform.position.y + gap, transform.position.z);
                GO.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                GO.GetComponent<BaseItem>().isCollected = false;
            }
        }
    }

    protected virtual bool IsMassLocked()
    {
        return eSS.GetStunTime() > 0;
    }
    private void UpdateMass()
    {
        if (IsMassLocked())
        {
            SetVelocityZero();
            if (RB.mass != 10000f)
                RB.mass = 10000f;
        }
        else
        {
            if (RB.mass != 1f)
                RB.mass = 1f;
        }
    }
    #region SetFunc
    protected virtual void SetVelocity(float speed,Vector2 angle)
    {
        if (eSS.GetStunTime() <= 0f)
        {
            RB.linearVelocity = new Vector2(speed * angle.x, speed * angle.y);
        }
    }
    public virtual void SetVelocityZero()
    {
        RB.linearVelocity = Vector2.zero;
    }
    private IEnumerator TickDamage()
    {
        while (true)
        {
            HasAttacked(eSS.GetFireStack(),AttackType.StaticAttack,false);
            yield return new WaitForSeconds(GameManager.instance.enemyFireTick);
        }
    }
    public void EndWave()
    {
        GameObject DeathParticle = GameManager.instance.poolManager.Get(36, new Vector3(transform.localScale.x / 0.35f, transform.localScale.x / 0.35f));
        DeathParticle.transform.position = transform.position;
        var mainPS = DeathParticle.GetComponent<ParticleSystem>().main;
        mainPS.startColor = enemyColor;
        isDeath = true;
        currentHealth = maxHealth;
        gameObject.SetActive(false);
    }
    #endregion
}
