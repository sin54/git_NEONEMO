using InventorySystem;
using UnityEngine;
using Core;

[CreateAssetMenu(fileName = "Item_WhiteSmoke", menuName = "Inventory/SkillEvent/D/Item_WhiteSmoke")]
public class Item_WhiteSmoke : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.Instance.SM.AddModifier("dodgeMul", multiplier: 0.95f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}