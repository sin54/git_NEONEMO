using UnityEngine;
using Core;

public class RageFireStigma : MonoBehaviour
{
    private CircleCollider2D CC2D;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Rage_Fire RF;
    [SerializeField] private ParticleSystem particle;
    private void Awake()
    {
        CC2D = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        particle.Stop();
    }
    public void Attack()
    {
        particle.Play();
        // CircleCollider2D의 중심과 반지름
        Vector2 center = CC2D.bounds.center;
        float radius = CC2D.bounds.extents.x;

        // Enemy Layer 안의 모든 Collider2D 탐색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, radius,enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            BaseEnemy BE = enemy.GetComponent<BaseEnemy>();
            BE.eSS.AddFireStack(RF.fireAmounts[RF.type.typeActiveLevel-1]*RF.chargeAmount);
            GameManager.Instance.AtkEnemy(BE, RF.damages[RF.type.typeActiveLevel-1]*RF.chargeAmount, AttackType.PhysicAttack,AttackAttr.Fire);
        }
    }
}