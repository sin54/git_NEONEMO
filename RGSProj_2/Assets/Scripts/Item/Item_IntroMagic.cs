using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_IntroMagic", menuName = "Inventory/SkillEvent/B/Item_IntroMagic")]
public class Item_IntroMagic : InventoryItemSkillEvent
{
    private bool staffSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = activeInv.GetTagCount(ItemTag.Staff) >= 1;
        float addAmount = 0.2f;
        if (newSyn != staffSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID);
            staffSyn = newSyn;
            if (staffSyn) addAmount += 0.2f;
            GameManager.Instance.SM.AddModifier("M_AtkMul", additive: addAmount, tag: ID);
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        staffSyn = false;
        GameManager.Instance.SM.AddModifier("M_AtkMul", additive: 0.2f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}