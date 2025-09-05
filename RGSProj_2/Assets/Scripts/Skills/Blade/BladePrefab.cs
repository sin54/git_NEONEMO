using UnityEngine;
using Core;

public class BladePrefab : MonoBehaviour
{
    public AttackInfo attackInfo { private get; set; }
    private void Awake()
    {
    }
    private void Start()
    {
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),attackInfo,AttackType.PhysicAttack, AttackAttr.Normal,transform.position - GameManager.Instance.player.gameObject.transform.position);
        }
    }
}
