using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_DoubleSword", menuName = "Inventory/SkillEvent/B+/Item_DoubleSword")]
public class Item_DoubleSword : InventoryItemSkillEvent
{
    private bool centerSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = (slotNum == 4);
        float atkMul = 1.3f;
        if (newSyn != centerSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID);
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.1f;
            GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID);
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        centerSyn = false;
        GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: 1.3f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}