using UnityEngine;
using System.Collections.Generic;
using Core;

public class LightWave : MonoBehaviour
{
    [SerializeField] private float velocity = 2f; // �ʴ� ������ ���� �ӵ�
    private CircleCollider2D circle;
    private Rage_Light RL;
    private float prevRadius = 0f;

    // �̹� ���� ���� ����
    private HashSet<BaseEnemy> damagedEnemies = new HashSet<BaseEnemy>();

    void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        prevRadius = circle.radius;
    }

    public void Init(Rage_Light rl)
    {
        RL = rl;
    }

    void Update()
    {
        float newRadius = circle.radius + velocity * Time.deltaTime;

        // �� ������ �ִ� �� ���� (prevRadius < distance <= newRadius)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, newRadius, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist <= prevRadius) continue; // �̹� ���� ������ ����

            BaseEnemy BE = hit.GetComponent<BaseEnemy>();
            if (BE != null && !damagedEnemies.Contains(BE)) // ó�� �´� ���� ó��
            {
                damagedEnemies.Add(BE); // ���

                GameManager.Instance.AtkEnemy(BE, RL.knockBackInfo, AttackType.StaticAttack, AttackAttr.None, BE.transform.position - transform.position);
                if (RL.type.typeActiveLevel >= 4)
                {
                    GameManager.Instance.AtkEnemy(BE, RL.damage, AttackType.MagicAttack, AttackAttr.Light);
                }
            }
        }

        prevRadius = newRadius;
        circle.radius = newRadius;
    }
}
