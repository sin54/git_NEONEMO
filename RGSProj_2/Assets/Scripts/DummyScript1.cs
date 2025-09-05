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
        GameManager.instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID);
        GameManager.instance.SM.AddModifier("M_AtkMul", additive: 0.05f, tag: ID);


    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}