using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_SmokeShell", menuName = "Inventory/SkillEvent/A/Item_SmokeShell")]
public class Item_SmokeShell : InventoryItemSkillEvent
{
    private bool meleeSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn= activeInv.GetTagCount(ItemTag.Melee) >= 1;
        if (newSyn != meleeSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
            meleeSyn = newSyn;
            if (meleeSyn)
            {
                GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.2f, tag: ID + "_Syn");
                GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.2f, tag: ID + "_Syn");
            }
            else
            {
                GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.1f, tag: ID + "_Syn");
                GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.1f, tag: ID + "_Syn");
            }
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        meleeSyn = false;
        GameManager.Instance.SM.AddModifier("dodgeMul", multiplier: 0.85f, tag: ID);
        GameManager.Instance.SM.AddModifier("CriticalPercent", additive: 0.1f, tag: ID+"_Syn");
        GameManager.Instance.SM.AddModifier("CriticalMul", multiplier: 1.1f, tag: ID+"_Syn");
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID+"_Syn");
    }
}