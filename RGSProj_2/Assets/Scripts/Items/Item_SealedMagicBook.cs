using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_SealedMagicBook", menuName = "Inventory/SkillEvent/C/Item_SealedMagicBook")]
public class Item_SealedMagicBook : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("M_AtkMul", additive: 0.1f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}