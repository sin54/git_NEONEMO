using Core;
using UnityEngine;

public class Rage_Fire : BaseRage
{
    [SerializeField] private Gradient attackColor;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private float maxChargeTime;

    public int[] fireAmounts;
    public float[] damages;
    public int chargeAmount;
    private bool isCharging = false;
    private float chargeTime = 0f;

    public override void RageStart()
    {
        base.RageStart();
        chargeAmount = 0;
        attackObj.SetActive(false);
        isCharging = true;
        chargeTime = 0.25f;
        attackRange.transform.localScale = Vector3.one * chargeTime;
        attackRange.SetActive(true);
        canAddGauge = false;
        GameManager.Instance.SM.AddModifier("PlayerSpeed", multiplier: 0.5f, tag: "Rage_Fire");
    }

    private void Update()
    {
        if (isCharging)
        {
            // �ð� ���� (�ִ� maxChargeTime������)
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime*0.3f, maxChargeTime);

            // ũ�� = chargeTime (0.25�� �⺻ ũ���� +�ص� ��)
            float size = chargeTime;
            attackRange.transform.localScale = Vector3.one * size;

            // ���� ��ȭ (0.5�ʺ��� maxChargeTime����)
            float t = Mathf.InverseLerp(0.5f, maxChargeTime, chargeTime);
            Color currentColor = attackColor.Evaluate(Mathf.Clamp01(t));

            SpriteRenderer sr = attackRange.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = currentColor;
        }

        if (Input.GetKeyUp(KeyCode.R)&&isCharging)
        {
            chargeAmount = Mathf.RoundToInt(chargeTime / 0.2f);
            isCharging = false;
            GameManager.Instance.SM.RemoveModifiersByTag("Rage_Fire");
            attackRange.SetActive(false);
            attackObj.transform.localScale= Vector3.one * chargeTime;
            attackObj.SetActive(true);
            canAddGauge = true;
        }
    }
}
