using UnityEngine;

public class MagicBallPrefab : MonoBehaviour
{
    private float speed;
    private float damage;
    private float size;
    private float radius;
    private int bouncedTime;
    private float attackDamageAddAmount;
    private Vector2 direction = Vector2.right;
    private Camera cam;
    private Skill_MagicBall SM;

    public void Init(float scale, float spd, float dmg, Vector2 dir)
    {
        speed = spd;
        size = scale;
        radius = size * 0.5f;
        damage = dmg;
        direction = dir.normalized;
        attackDamageAddAmount = 0;
        bouncedTime = 0;
        transform.localScale = new Vector3(size, size);
        cam = Camera.main;
        SM=GetComponentInParent<Skill_MagicBall>(); 
    }

    private void Update()
    {
        transform.localPosition += (Vector3)(direction * speed * Time.deltaTime);

        Vector3 worldMin = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 worldMax = cam.ViewportToWorldPoint(Vector3.one);

        Vector3 localMin = transform.parent.InverseTransformPoint(worldMin);
        Vector3 localMax = transform.parent.InverseTransformPoint(worldMax);

        Vector3 localPos = transform.localPosition;
        bool bounced = false;

        if (localPos.x - radius < localMin.x || localPos.x + radius > localMax.x)
        {
            direction.x *= -1;
            bounced = true;
        }

        if (localPos.y - radius < localMin.y || localPos.y + radius > localMax.y)
        {
            direction.y *= -1;
            bounced = true;
        }

        if (bounced)
        {
            localPos.x = Mathf.Clamp(localPos.x, localMin.x + radius, localMax.x - radius);
            localPos.y = Mathf.Clamp(localPos.y, localMin.y + radius, localMax.y - radius);
            transform.localPosition = localPos;
            bouncedTime++;
            if (bouncedTime >= SM.initBouncedNum)
            {
                bouncedTime = 0;
                attackDamageAddAmount = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.instance.AtkEnemy(collision.GetComponent<BaseEnemy>(), SM.reinforcedNum == 2 ? damage + attackDamageAddAmount : damage,AttackType.PhysicAttack, AttackAttr.Normal);
            attackDamageAddAmount += SM.damageAddAmount;
        }
    }
}
