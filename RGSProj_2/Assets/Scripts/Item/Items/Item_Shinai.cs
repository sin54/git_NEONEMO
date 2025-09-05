using InventorySystem;
using UnityEngine;
using Core;
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
        GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: 1.05f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}