using UnityEngine;

public class SpiritBreathBullet : BaseBullet
{
    
    public void SetBullet(float bSpeed, float bLifeTime,Vector2 ang)
    {
        bulletLifeTime = bLifeTime;
        RB.linearVelocity = bSpeed * ang.normalized;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.GetComponent<BaseEnemy>().eSS.AddFireStack(1);
            DisableBullet();
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
