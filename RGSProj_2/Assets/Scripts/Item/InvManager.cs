using InventorySystem;
using UnityEngine;
using Core;

public class InvManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryObj;
    [SerializeField] private PlayerMove PM;
    private void Start()
    {
        InventoryController.instance.AddItem("Inventory", "Ruby", 2);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowPausePanel();
        }
    }
    public void ShowPausePanel()
    {
        inventoryObj.SetActive(!inventoryObj.activeSelf);
        if (inventoryObj.activeSelf)
        {
            TimeScaleManager.Instance.TimeStopStackPlus();
        }
        else
        {
            TimeScaleManager.Instance.TimeStopStackMinus();
        }
    }
}
