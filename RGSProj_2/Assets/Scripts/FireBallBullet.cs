using UnityEngine;

public class FireBallBullet : BaseAngled
{
    [SerializeField] private GameObject fireExplosion;
    private float radius;
    private float damage;
    private int fireStack;

    public void SetBullet(float bSpeed, float bLifeTime, Vector2 ang, int extraValue,float explosionRadius,float explosionDamage,int fireAmount)
    {
        bulletSpeed = bSpeed;
        bulletLifeTime = bLifeTime;
        bulletDirection = ang;
        maxPenetrationLimit = extraValue;
        float rotationZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        radius = explosionRadius;
        damage=explosionDamage;
        fireStack = fireAmount;
        bulletParent.position = GameManager.instance.player.transform.position;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject GO = GameManager.instance.poolManager.Get(20);
            GO.GetComponent<FireExplosion>().SetExplosion(radius, damage,fireStack,0.1f);
            GO.transform.position = collision.gameObject.transform.position;
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
}
