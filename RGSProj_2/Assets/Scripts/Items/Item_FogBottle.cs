using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_FogBottle", menuName = "Inventory/SkillEvent/B/Item_FogBottle")]
public class Item_FogBottle : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("dodgeMul", multiplier: 0.9f, tag: ID);
        GameManager.instance.SM.AddModifier("CriticalPercent", additive:0.05f, tag: ID);
        GameManager.instance.SM.AddModifier("CriticalMul", multiplier: 1.05f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}