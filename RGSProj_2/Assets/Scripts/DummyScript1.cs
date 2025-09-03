using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "DummyScript1", menuName = "Inventory/SkillEvent/F/DummyScript1")]
public class DummyScript1 : InventoryItemSkillEvent
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