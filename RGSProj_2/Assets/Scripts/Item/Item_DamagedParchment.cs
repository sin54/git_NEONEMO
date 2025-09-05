using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_DamagedParchment", menuName = "Inventory/SkillEvent/E/Item_DamagedParchment")]
public class Item_DamagedParchment : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.Instance.SM.AddModifier("M_AtkMul", additive: 0.05f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}