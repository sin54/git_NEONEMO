using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_MetalOfGiantFlame", menuName = "Inventory/SkillEvent/A/Item_MetalOfGiantFlame")]
public class Item_MetalOfGiantFlame : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("CriticalPercent", additive: 0.20f, tag: ID);
        GameManager.instance.SM.AddModifier("P_AtkMul", additive: 0.1f, tag: ID);
        GameManager.instance.SM.AddModifier("Fire_AtkMul", multiplier: 1.3f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}