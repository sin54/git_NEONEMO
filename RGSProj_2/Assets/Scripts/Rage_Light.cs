using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rage_Light : BaseRage
{
    [SerializeField] private ParticleSystem lightField;
    [SerializeField] private GameObject lightWave;
    [SerializeField] private Image gaugeImg;
    [SerializeField] private float[] durations = new float[5];
    [SerializeField] private int[] numOfRings = new int[5];
    public AttackInfo knockBackInfo;
    public float damage;

    private LightFieldParticle lFP;
    public float reduceSpeedMul;
    public float increaseHealthMul;
    private float rageMax;
    private bool isRaging;
    private float lastRageTime;

    private Coroutine slowCoroutine;

    private void Awake()
    {
        lFP = GetComponentInChildren<LightFieldParticle>();
    }

    public override void RageStart()
    {
        base.RageStart();

        var PSmain = lightField.main;
        var PSsub = lightField.collision;
        var PSsub2 = lightField.trigger;
        PSmain.duration = durations[type.typeActiveLevel-1];
        if (type.typeActiveLevel >= 2)
        {
            PSsub.enabled = true;
            PSsub2.enabled = true;
        }
        else
        {
            PSsub.enabled = false;
            PSsub2.enabled = false;
        }
        lightField.Play();

        isRaging = true;
        lastRageTime = durations[type.typeActiveLevel-1];
        rageMax = durations[type.typeActiveLevel - 1];

        canAddGauge = false;

        // Coroutine 시작: 0.5초마다 ApplySlow 호출
        if (lFP != null)
            slowCoroutine = StartCoroutine(SlowRoutine());

        // Rings 스폰 Coroutine 시작
        StartCoroutine(SpawnRingsRoutine());
    }

    private IEnumerator SpawnRingsRoutine()
    {
        int rings = numOfRings[type.typeActiveLevel-1];
        if (rings <= 0) yield break;

        float duration = durations[type.typeActiveLevel - 1];

        if (rings == 1)
        {
            // 한 개만 스폰: 시작 시
            SpawnRings();
            yield break;
        }

        // 균등 간격 계산 (첫/마지막은 시작/종료)
        float interval = duration / (rings - 1);

        for (int i = 0; i < rings; i++)
        {
            SpawnRings();
            if (i < rings - 1)
                yield return new WaitForSeconds(interval);
        }
    }


    private void Update()
    {
        if (isRaging)
        {
            lastRageTime -= Time.deltaTime;
            gaugeImg.fillAmount = lastRageTime / rageMax;
            if (lastRageTime < 0)
            {
                isRaging = false;
                canAddGauge = true;
                lightField.Stop();

                // Rage 끝나면 Coroutine 멈춤
                if (slowCoroutine != null)
                {
                    StopCoroutine(slowCoroutine);
                    slowCoroutine = null;
                }
            }
        }
    }

    private IEnumerator SlowRoutine()
    {
        while (true)
        {
            lFP.ApplySlow();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnRings()
    {
        if (type.typeActiveLevel < 3) return;
        GameObject GO=Instantiate(lightWave,transform.position,Quaternion.identity);
        GO.GetComponent<LightWave>().Init(this);
    }
}
