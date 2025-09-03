using UnityEngine;

public class SideSword : MonoBehaviour
{
    [SerializeField]private Skill_Ghost SG;
    public Transform centerTarget;  // 중심이 될 대상, 즉 SG.transform
    public float radius = 0.5f;       // 중심으로부터 거리

    [SerializeField]private float angle = 0f;

    private void FixedUpdate()
    {
        if (centerTarget == null) return;

        // 원하는 방향으로 회전 (시계든 반시계든 가능)
        angle += SG.rotationSpeedOfSword* Time.fixedDeltaTime; // or -= for 반시계
        float rad = angle * Mathf.Deg2Rad;

        // 위치 계산
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.position = centerTarget.position + offset;
        Vector3 directionToCenter = (centerTarget.position - transform.position).normalized;
        float angleDeg = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),SG.damageOfSword,AttackType.PhysicAttack,AttackAttr.Wind);
        }
    }
}
