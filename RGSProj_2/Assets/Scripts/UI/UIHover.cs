using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject[] toolTipObj;
    [SerializeField] private GameObject[] nonToolTipObj;

    private void SetActiveArray(GameObject[] array, bool isActive)
    {
        foreach (GameObject obj in array)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveArray(toolTipObj, true);
        SetActiveArray(nonToolTipObj, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetActiveArray(toolTipObj, false);
        SetActiveArray(nonToolTipObj, true);
    }
}
