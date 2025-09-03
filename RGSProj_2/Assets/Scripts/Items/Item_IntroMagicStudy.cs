using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_IntroMagicStudy", menuName = "Inventory/SkillEvent/A/Item_IntroMagicStudy")]
public class Item_IntroMagicStudy : InventoryItemSkillEvent
{
    private bool staffSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = activeInv.GetTagCount(ItemTag.Staff) >= 1;
        float addAmount = 0.5f;
        if (newSyn != staffSyn)
        {
            GameManager.instance.SM.RemoveModifiersByTag(ID);
            staffSyn = newSyn;
            if (staffSyn) addAmount += 0.5f;
            GameManager.instance.SM.AddModifier("M_AtkMul", additive: addAmount, tag: ID);
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        staffSyn = false;
        GameManager.instance.SM.AddModifier("M_AtkMul", additive: 0.5f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
}