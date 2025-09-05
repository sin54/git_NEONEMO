using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_Neconomicron", menuName = "Inventory/SkillEvent/S/Item_Neconomicron")]
public class Item_Neconomicron : InventoryItemSkillEvent
{
    private bool staffSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = activeInv.GetTagCount(ItemTag.Staff) >= 1;
        float addAmount = 1.5f;
        if (newSyn != staffSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID);
            staffSyn = newSyn;
            if (staffSyn) addAmount += 1f;
            GameManager.Instance.SM.AddModifier("M_AtkMul", additive: addAmount, tag: ID);
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        staffSyn = false;
        GameManager.Instance.SM.AddModifier("M_AtkMul", additive: 0.5f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}