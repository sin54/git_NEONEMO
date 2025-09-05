using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_EmberFangDagger", menuName = "Inventory/SkillEvent/B/Item_EmberFangDagger")]
public class Item_EmberFangDagger : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID);
        GameManager.Instance.SM.AddModifier("P_AtkMul", additive: 0.05f, tag: ID);


    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}