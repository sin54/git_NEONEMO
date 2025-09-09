using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Type;

public class Skill_Rage : MonoBehaviour
{
    [SerializeField] private TMP_Text rageCoolTxt;
    [SerializeField] private Image rageCoolImg;
    [SerializeField] private Image rageCoolBackImg;
    [SerializeField] private GameObject rageCoolInd;
    [SerializeField] private BaseRage[] BR; // Ÿ�Ժ� ���� ��ũ��Ʈ (typeCode �������)
    [SerializeField] private PlayerTypeManager PT;
    [SerializeField] private TMP_Text rageLvlTxt;

    private float gaugeTickTime = 1f; // 1�ʸ��� ������ +1
    private float lastGaugeUpdateTime;

    private void Update()
    {
        // ���� Ÿ�� ��������
        int currentType = PT.BT.typeCode;
        if (PT.Types[currentType].typeActiveLevel <= 0)
        {
            rageCoolInd.SetActive(false);
            return;
        }
        else
        {
            rageCoolInd.SetActive(true);
        }
        BaseRage currentRage = BR[currentType];
        if (currentRage == null) return;

        if (Time.time >= lastGaugeUpdateTime + gaugeTickTime&&currentRage.canAddGauge&&!GameManager.Instance.isDay)
        {
            lastGaugeUpdateTime = Time.time;
            currentRage.currentGauge = Mathf.Min(currentRage.currentGauge + 1f, currentRage.needGauge);
        }

        if (currentRage.canAddGauge)
        {
            rageCoolImg.fillAmount = currentRage.currentGauge / currentRage.needGauge;
            rageCoolTxt.text = currentRage.currentGauge >= currentRage.needGauge ? "R" : $"{currentRage.needGauge - currentRage.currentGauge:0}";
        }


        rageCoolImg.color = new Color(PT.Colors[currentType].r, PT.Colors[currentType].g, PT.Colors[currentType].b,1f);
        rageCoolBackImg.color= new Color(PT.Colors[currentType].r, PT.Colors[currentType].g, PT.Colors[currentType].b,0.06666f);
        rageLvlTxt.text = "LV"+PT.Types[currentType].typeActiveLevel.ToString();

        if (Input.GetKeyDown(KeyCode.R) && currentRage.currentGauge >= currentRage.needGauge)
        {
            currentRage.RageStart();
            currentRage.currentGauge = 0;
        }
    }
}
