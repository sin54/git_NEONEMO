using UnityEngine;

public class StarObj : MonoBehaviour
{
    private Vector3 targetPos;
    private float meteorSpeed;
    private float radius;
    private AttackInfo damageInfo;
    private Skill_ShootingStar SS;
    private bool isrf;
    public void SetStar(Vector3 FinalPos,float speed,float rad,AttackInfo dmg,Skill_ShootingStar ss,bool isRF)
    {
        SS = ss;
        targetPos = FinalPos;
        meteorSpeed = speed;
        radius = rad;
        damageInfo = dmg;
        isrf = isRF;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            GameObject GO = GameManager.instance.poolManager.Get(31);
            GO.transform.GetChild(0).GetComponent<StarExplode>().SetExplosion(radius, damageInfo,transform.position);
            if (isrf)
            {
                GO.transform.GetChild(0).GetComponent<SpriteRenderer>().color= Color.red;
            }
            else
            {
                GO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            if (SS.reinforcedNum == 2&&UtilClass.GetPercent(SS.fracPercent))
            {
                GameObject GO2 = GameManager.instance.poolManager.Get(37);
                GO2.GetComponent<StarFrac>().SetStarFrac(SS.starLifeTime, SS.healAmount);
                GO2.transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, meteorSpeed);
    }
}
