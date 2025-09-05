using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_temp", menuName = "Inventory/SkillEvent/F/Item_temp")]
public class Item_temp : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
    }
}