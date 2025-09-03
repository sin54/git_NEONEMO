using InventorySystem;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private Slot slot;
    private string slotID;


    private void Awake()
    {
        slot = GetComponent<Slot>();
        slotID = "Weapon " + slot.GetPosition().ToString();
    }
    public void OnEnter(InventoryItem item)
    {
        //Debug.Log("ENTER "+item.GetItemType()+ " "+slot.GetPosition());
        item.GetSkillEvent().OnEnter(item, slotID);
    }
    public void OnExit(InventoryItem item)
    {
        //Debug.Log("Exit " + item.GetItemType() + " " + slot.GetPosition());
        item.GetSkillEvent().OnExit(item, slotID);
    }
    public void OnActivate(InventoryItem item)
    {
        //Debug.Log("ACT " + item.GetItemType() + " " + slot.GetPosition());
        item.GetSkillEvent().OnActivate(item, slotID);
    }
}
