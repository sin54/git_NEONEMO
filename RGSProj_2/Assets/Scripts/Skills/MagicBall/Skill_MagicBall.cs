using UnityEngine;

public class Skill_MagicBall : BaseSkill
{
    [HideInInspector] public SO_MagicBallData skillData;
    public GameObject ballPrefab;
    [Header("≈ ≈ ∫º")]
    public int ballAmount = 7;
    public float ballRadius;
    [Header("∞•∏¡")]
    public float damageAddAmount = 1f;
    public int initBouncedNum = 3;
    [Header("≈∏≈∞ø¬")]
    public float tachyonSpeed;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_MagicBallData))
        {
            skillData = (SO_MagicBallData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        batch();
    }
    private void batch()
    {
        if (reinforcedNum != 3)
        {
            for (int i = 0; i < (reinforcedNum == 1 ? ballAmount : skillData.numOfBall[itemLevel]); i++)
            {
                GameObject ball;
                if (i < transform.childCount)
                {
                    ball = transform.GetChild(i).gameObject;
                }
                else
                {
                    ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                }
                ball.transform.parent = transform;
                ball.GetComponent<MagicBallPrefab>().Init(reinforcedNum == 1 ? ballRadius : skillData.ballRadius[itemLevel], skillData.ballSpeed[itemLevel], skillData.attackDamage[itemLevel], new Vector2(UtilClass.GetPercent(0.5f) ? Random.Range(0.3f, 0.7f) : -1 * Random.Range(0.3f, 0.7f), UtilClass.GetPercent(0.5f) ? Random.Range(0.3f, 0.7f) : -1 * Random.Range(0.3f, 0.7f)).normalized);
            }
        }
        else
        {
            GameObject ball;
            ball = transform.GetChild(0).gameObject;
            ball.transform.parent = transform;
            Destroy(transform.GetChild(1).gameObject);
            ball.GetComponent<MagicBallPrefab>().Init(skillData.ballRadius[itemLevel], tachyonSpeed, skillData.attackDamage[itemLevel], new Vector2(UtilClass.GetPercent(0.5f) ? Random.Range(0.3f, 0.7f): -1*Random.Range(0.3f, 0.7f), UtilClass.GetPercent(0.5f) ? Random.Range(0.3f, 0.7f) : -1 * Random.Range(0.3f, 0.7f)).normalized);
        }
    }
}
