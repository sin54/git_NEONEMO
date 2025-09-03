using InventorySystem;
using UnityEngine;
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
            GameManager.instance.SM.RemoveModifiersByTag(ID+"_Syn");
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.2f;
            GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID+"_Syn");
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        centerSyn = false;
        GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: 1.75f, tag: ID+"_Syn");
        GameManager.instance.SM.AddModifier("PlayerSpeed", additive: 0.2f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID+"_Syn");
    }
}