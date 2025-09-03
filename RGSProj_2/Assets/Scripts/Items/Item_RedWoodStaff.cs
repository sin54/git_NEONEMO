using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_RedWoodStaff", menuName = "Inventory/SkillEvent/C/RedWoodStaff")]
public class Item_RedWoodStaff : InventoryItemSkillEvent
{
    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("M_AtkMul", multiplier: 1.1f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}
