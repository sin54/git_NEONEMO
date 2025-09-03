using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Shinai", menuName = "Inventory/SkillEvent/E/Item_Shinai")]
public class Item_Shinai : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: 1.05f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}