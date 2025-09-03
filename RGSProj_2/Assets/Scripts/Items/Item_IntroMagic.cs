using InventorySystem;
using UnityEngine;
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
            GameManager.instance.SM.RemoveModifiersByTag(ID);
            staffSyn = newSyn;
            if (staffSyn) addAmount += 0.2f;
            GameManager.instance.SM.AddModifier("M_AtkMul", additive: addAmount, tag: ID);
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        staffSyn = false;
        GameManager.instance.SM.AddModifier("M_AtkMul", additive: 0.2f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}