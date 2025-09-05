using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_Katana", menuName = "Inventory/SkillEvent/S/Item_Katana")]
public class Item_Katana : InventoryItemSkillEvent
{
    private bool centerSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = (slotNum == 4);
        float atkMul = 1.75f;
        if (newSyn != centerSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID+"_Syn");
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.2f;
            GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID+"_Syn");
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        centerSyn = false;
        GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: 1.75f, tag: ID+"_Syn");
        GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.2f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID+"_Syn");
    }
}