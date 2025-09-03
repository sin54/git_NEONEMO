using UnityEngine;

public class Skill_FireSpirit : BaseSkill
{
    [HideInInspector]public SO_SpiritData skillData;
    [SerializeField] private GameObject spiritPrefab;
    [Header("ºÒ¾¾")]
    public float spearDamage;
    public float spearCool;
    public int spearFireAmount;
    [Header("È­¸¶")]
    public float numOfAttack;
    public float explodeCool;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_SpiritData))
        {
            skillData = (SO_SpiritData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        Batch();
    }
    private void Batch()
    {
        int count = skillData.countByLevel[itemLevel];
        float radius = reinforcedNum == 3 ? 1.75f : 0.35f;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < count; i++)
        {
            Transform blade;
            if (i < transform.childCount)
            {
                blade = transform.GetChild(i);
                blade.gameObject.SetActive(true);
            }
            else
            {
                blade = Instantiate(spiritPrefab, transform).transform;
            }

            float angle = 360f * i / count;

            blade.GetComponent<FireSpirit>().SF = this;

            var spin = blade.GetComponent<SpinAround>();
            spin.radius = radius;
            spin.SetInitialAngle(angle);
        }
    }
    private void Update()
    {
    }
}
