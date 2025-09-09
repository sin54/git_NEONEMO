using UnityEngine;
using UnityEngine.UI;
using Core;
public class Rage_Normal : BaseRage
{
    [SerializeField] private GameObject rageNormalEffect;
    [SerializeField] private Image gaugeImg;
    private bool isRaging;
    private float lastRageTime;
    private float rageMax;
    [SerializeField] private float[] durations = new float[5];
    [SerializeField] private float[] percents = new float[5];
    public override void RageStart()
    {
        base.RageStart();
        isRaging = true;
        lastRageTime = durations[type.typeActiveLevel-1];
        rageMax = durations[type.typeActiveLevel - 1];
        rageNormalEffect.SetActive(true);
        canAddGauge = false;
        GameManager.Instance.SM.AddModifier("WeaponCoolReduce", multiplier: percents[type.typeActiveLevel-1], duration: durations[type.typeActiveLevel - 1]);
        for(int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0; i < type.typeActiveLevel; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (isRaging)
        {
            lastRageTime -= Time.deltaTime;
            gaugeImg.fillAmount=lastRageTime/rageMax;
            if(lastRageTime < 0)
            {
                isRaging = false;
                rageNormalEffect.SetActive(false);
                canAddGauge = true;
            }
        }
    }

}
