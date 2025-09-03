using UnityEngine;

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
            GameManager.instance.AtkEnemy(collision.GetComponent<BaseEnemy>(),attackInfo,AttackType.PhysicAttack, AttackAttr.Normal,transform.position - GameManager.instance.player.gameObject.transform.position);
        }
    }
}
