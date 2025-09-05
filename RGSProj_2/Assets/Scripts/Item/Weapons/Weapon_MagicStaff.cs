using UnityEngine;
using InventorySystem;
[CreateAssetMenu(fileName = "Weapon_MagicStaff", menuName = "Inventory/Weapon/MagicStaff")]
public class Weapon_MagicStaff : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
    }
}
