using InventorySystem;
using TMPro;
using UnityEngine;

public class SetToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameTxt;
    [SerializeField] private TMP_Text itemDescTxt;
    [SerializeField] private TMP_Text itemSynergeTxt;
    [SerializeField] private TMP_Text itemTagTxt;
    [SerializeField] private TMP_Text itemRarityTxt;
    [SerializeField] private GameObject dojingObj;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void ShowToolTip(Transform pos, InventoryItem IISE)
    {
        // 텍스트 세팅
        itemNameTxt.text = IISE.GetSkillEvent().itemName;
        itemDescTxt.text = IISE.GetSkillEvent().itemAbility;
        itemSynergeTxt.text = IISE.GetSkillEvent().synergeString;
        itemTagTxt.text = UtilClass.ToLocalizedString(IISE.GetTags());
        itemRarityTxt.text = UtilClass.ToLocalizedString(IISE.GetRarity());

        // 오브젝트 위치를 캔버스 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Camera.main.WorldToScreenPoint(pos.position),
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // offset 적용 (살짝 위로 띄우기)
        localPoint += new Vector2(-20f, 20f);

        // 툴팁 크기 / 캔버스 크기
        Vector2 tooltipSize = rectTransform.sizeDelta;
        Vector2 canvasSize = (canvas.transform as RectTransform).sizeDelta;

        float halfH = canvasSize.y / 2;

        Debug.Log(localPoint.y + tooltipSize.y + " " + halfH);
        // y 클램프
        if (localPoint.y + tooltipSize.y > halfH)
        {
            localPoint.y = halfH - tooltipSize.y;
        }

        // 최종 적용
        rectTransform.localPosition = localPoint;

        dojingObj.SetActive(true);
    }




    public void HideToolTip()
    {
        dojingObj.SetActive(false);
    }
}