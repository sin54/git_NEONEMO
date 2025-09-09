using UnityEngine;
using Core;

public class SideSword : MonoBehaviour
{
    [SerializeField]private Skill_Ghost SG;
    public Transform centerTarget;  // �߽��� �� ���, �� SG.transform
    public float radius = 0.5f;       // �߽����κ��� �Ÿ�

    [SerializeField]private float angle = 0f;

    private void FixedUpdate()
    {
        if (centerTarget == null) return;

        // ���ϴ� �������� ȸ�� (�ð�� �ݽð�� ����)
        angle += SG.rotationSpeedOfSword* Time.fixedDeltaTime; // or -= for �ݽð�
        float rad = angle * Mathf.Deg2Rad;

        // ��ġ ���
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
            GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),SG.damageOfSword,AttackType.PhysicAttack,AttackAttr.Wind);
        }
    }
}
