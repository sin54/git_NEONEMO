using TMPro;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    private Rigidbody2D RB;
    private Vector3 targetPos;
    private float meteorSpeed;
    private float radius;
    private AttackInfo damageInfo;
    private Skill_Meteor SM;
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    public void SetMeteor(Vector3 FinalPos,float speed,float rad,AttackInfo dmg,Skill_Meteor sm)
    {
        targetPos = FinalPos;
        meteorSpeed = speed;
        radius = rad;
        damageInfo = dmg;
        SM=sm;
    }
    private void Update()
    {

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            GameObject GO = GameManager.instance.poolManager.Get(24);
            GO.GetComponent<Meteorite_Explosion>().SetExplode(radius, damageInfo,0.2f,SM);
            GO.transform.position=transform.position;
            gameObject.SetActive(false);

        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, meteorSpeed);
    }
}
