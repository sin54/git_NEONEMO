using InventorySystem;
using UnityEngine;
using Core;

[CreateAssetMenu(fileName = "Item_RedRedStone", menuName = "Inventory/SkillEvent/C/Item_RedRedStone")]
public class Item_RedRedStone : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.Instance.player.IncreaseMaxHealth(10);
        GameManager.Instance.SM.AddModifier("F_Cool", multiplier: 0.95f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.player.DecreaseMaxHealth(10);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}