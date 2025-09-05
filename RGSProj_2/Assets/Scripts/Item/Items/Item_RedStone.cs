using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_RedStone", menuName = "Inventory/SkillEvent/E/Item_RedStone")]
public class Item_RedStone : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        GameManager.Instance.player.IncreaseMaxHealth(5);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.player.DecreaseMaxHealth(5);
    }
}