using UnityEngine;

public class Skill_Blade : BaseSkill
{
    private SO_BladeData skillData;
    [SerializeField] private GameObject bladePrefab;
    [Header("가속")]
    public float reinforcedRotationSpeed;
    [Header("실체화")]
    public AttackInfo reinforcedAttackInfo;
    [Header("주피터")]
    public float reinfocedRadius = 2.2f;
    public int reinforcedBladeCount = 7;

    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_BladeData))
        {
            skillData = (SO_BladeData)baseSkillData;
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
        if (reinforcedNum != 3)
        {
            for (int i = 0; i < skillData.countByLevel[itemLevel]; i++)
            {
                Transform blade;
                if (i < transform.childCount)
                {
                    blade = transform.GetChild(i);
                }
                else
                {
                    blade = Instantiate(bladePrefab, transform.position, Quaternion.identity).transform;
                }
                blade.parent = transform;
                blade.localPosition = Vector3.zero;
                blade.localRotation = Quaternion.identity;
                Vector3 rotVec = Vector3.forward * 360 * i / skillData.countByLevel[itemLevel];
                blade.Rotate(rotVec);
                blade.Translate(blade.up * 1.5f, Space.World);
                if (reinforcedNum != 2)
                {
                    blade.gameObject.GetComponent<BladePrefab>().attackInfo = skillData.attackInfoByLevel[itemLevel];
                }
                else
                {
                    blade.gameObject.GetComponent<BladePrefab>().attackInfo = reinforcedAttackInfo;
                }

            }
        }
        else
        {
            for (int i = 0; i < reinforcedBladeCount; i++)
            {
                Transform blade;
                if (i < transform.childCount)
                {
                    blade = transform.GetChild(i);
                }
                else
                {
                    blade = Instantiate(bladePrefab, transform.position, Quaternion.identity).transform;
                }
                blade.parent = transform;
                blade.localPosition = Vector3.zero;
                blade.localRotation = Quaternion.identity;
                Vector3 rotVec = Vector3.forward * 360 * i / reinforcedBladeCount;
                blade.Rotate(rotVec);
                blade.Translate(blade.up * reinfocedRadius, Space.World);
                blade.gameObject.GetComponent<BladePrefab>().attackInfo = skillData.attackInfoByLevel[itemLevel];

            }
        }

    }

    private void Update()
    {
        if (reinforcedNum != 1)
        {
            transform.Rotate(Vector3.forward * skillData.rotateSpeedByLevel[itemLevel] * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * reinforcedRotationSpeed* Time.deltaTime);
        }
    }
}
