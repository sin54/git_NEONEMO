using InventorySystem;
using UnityEngine;
using Core;
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
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
            meleeSyn = newSyn;
            if (meleeSyn)
            {
                GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.3f, tag: ID + "_Syn");
                GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.45f, tag: ID + "_Syn");
            }
            else
            {
                GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID + "_Syn");
                GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.3f, tag: ID + "_Syn");
            }
        }
        if (newSyn2 != slotSyn) {
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn2");
            slotSyn = newSyn2;
            if (slotSyn)
            {
                GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.1f, tag: ID+"_Syn2");
            }
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        meleeSyn = false;
        slotSyn = false;
        GameManager.Instance.SM.AddModifier("dodgeMul", multiplier: 0.7f, tag: ID);
        GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.15f, tag: ID + "_Syn");
        GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.3f, tag: ID + "_Syn");
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
        GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn2");
    }
}