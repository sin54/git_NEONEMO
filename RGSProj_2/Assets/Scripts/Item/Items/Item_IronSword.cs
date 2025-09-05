using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_IronSword", menuName = "Inventory/SkillEvent/C/IronSword")]
public class Item_IronSword : InventoryItemSkillEvent
{
    private bool centerSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, itemName);
        bool newSyn = (slotNum == 4);
        float atkMul = 1.15f;
        if (newSyn != centerSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID);
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.05f;
            GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID);
        }

    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, itemName);
        centerSyn = false;
        GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: 1.15f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, itemName);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}
