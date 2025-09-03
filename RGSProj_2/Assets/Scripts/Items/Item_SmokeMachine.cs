using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_SmokeMachine", menuName = "Inventory/SkillEvent/A+/Item_SmokeMachine")]
public class Item_SmokeMachine : InventoryItemSkillEvent
{
    private bool meleeSyn;
    private bool slotSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = activeInv.GetTagCount(ItemTag.Melee) >= 1;
        bool newSyn2 = (slotNum == 7);
        if (newSyn != meleeSyn)
        {
            GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn");
            meleeSyn = newSyn;
            if (meleeSyn)
            {
                GameManager.instance.SM.AddModifier("CriticalPercent", additive: 0.3f, tag: ID + "_Syn");
                GameManager.instance.SM.AddModifier("CriticalMul", multiplier: 1.45f, tag: ID + "_Syn");
            }
            else
            {
                GameManager.instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID + "_Syn");
                GameManager.instance.SM.AddModifier("CriticalMul", multiplier: 1.3f, tag: ID + "_Syn");
            }
        }
        if (newSyn2 != slotSyn) {
            GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn2");
            slotSyn = newSyn2;
            if (slotSyn)
            {
                GameManager.instance.SM.AddModifier("PlayerSpeed", additive: 0.1f, tag: ID+"_Syn2");
            }
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        meleeSyn = false;
        slotSyn = false;
        GameManager.instance.SM.AddModifier("dodgeMul", multiplier: 0.7f, tag: ID);
        GameManager.instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID + "_Syn");
        GameManager.instance.SM.AddModifier("CriticalMul", multiplier: 1.3f, tag: ID + "_Syn");
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn");
        GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn2");
    }
}