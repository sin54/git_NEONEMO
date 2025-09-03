using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ReinforceTooltip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] toolTipObj;
    [SerializeField] private GameObject[] nonToolTipObj;

    [Header("허용된 마우스 버튼들 (복수 선택 가능)")]
    [SerializeField] private List<PointerEventData.InputButton> allowedButtons;

    private bool isTooltipActive = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (allowedButtons.Contains(eventData.button))
        {
            isTooltipActive = !isTooltipActive;

            SetActiveArray(toolTipObj, isTooltipActive);
            SetActiveArray(nonToolTipObj, !isTooltipActive);
        }
    }

    private void SetActiveArray(GameObject[] array, bool isActive)
    {
        foreach (GameObject obj in array)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }
}
