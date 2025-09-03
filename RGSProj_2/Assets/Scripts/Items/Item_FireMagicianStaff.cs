using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_FireMagicianStaff", menuName = "Inventory/SkillEvent/B+/FireMagicianStaff")]
public class Item_FireMagicianStaff : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);

    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.instance.SM.AddModifier("M_AtkMul", multiplier: 1.2f, tag: ID);
        GameManager.instance.SM.AddModifier("Fire_AtkMul", multiplier: 1.15f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}
