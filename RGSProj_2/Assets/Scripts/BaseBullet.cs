using UnityEngine;
using System.Collections;
public class BaseBullet : MonoBehaviour
{
    protected SpriteRenderer SR;
    protected Rigidbody2D RB;
    public Transform bulletParent;
    protected BaseSkill BS;
    public bool isTrail;
    public ParticleSystem PS;

    protected float bulletSpeed;
    protected float bulletLifeTime;
    protected float spawnTime;
    protected AttackInfo attackInfo;
    protected Vector2 bulletDirection;
    
    public AttackType AttackType;
    public AttackAttr AttackAttr;

    private bool isDisabling = false;


    protected virtual void Awake()
    {
        RB = GetComponentInParent<Rigidbody2D>();
        SR=GetComponent<SpriteRenderer>();
    }
    protected virtual void Start()
    {
    }
    protected virtual void OnEnable()
    {
        isDisabling = false;
        spawnTime = Time.time;
        if (isTrail)
        {
            SR.enabled = true;
            gameObject.layer = 0;
            var pEmit = PS.emission;
            pEmit.enabled = true;
        }

    }
    protected virtual void Update()
    {
        if (Time.time > spawnTime + bulletLifeTime)
        {
            DisableBullet();
        }
    }

    protected virtual void DisableBullet()
    {
        if (isDisabling) return;
        isDisabling = true;

        if (isTrail)
        {
            SR.enabled = false;
            gameObject.layer = 17;
            var pEmit = PS.emission;
            pEmit.enabled = false;
            RB.linearVelocity = Vector2.zero;
            StartCoroutine(DelayedDisableByTrail());
        }
        else
        {
            DisableParent();
        }
    }

    private IEnumerator DelayedDisableByTrail()
    {
        PS.Stop();
        while (PS.IsAlive(true))
        {
            if (!gameObject.activeInHierarchy || !PS.gameObject.activeInHierarchy)
                yield break;
            yield return null;
        }

        DisableParent();
    }
    private void DisableParent()
    {
        bulletParent.gameObject.SetActive(false);
    }
    protected virtual void AttackEnemy(BaseEnemy target,AttackInfo aInfo)
    {
        GameManager.instance.AtkEnemy(target, aInfo,AttackType,AttackAttr ,target.transform.position - transform.position);
    }
}
