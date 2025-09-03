using InventorySystem;
using UnityEngine;
//[CreateAssetMenu(fileName = "Item_OldArmor", menuName = "Inventory/SkillEvent/Normal/OldArmor")]
public class Item_OldArmor : InventoryItemSkillEvent
{
    /*
    private bool synerge_Center;
    private bool synerge_Old;
    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, itemName);
        GameManager.instance.SM.AddModifier("defenceRate", multiplier: 1.05f, tag: this.name);
    }

    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, itemName);

        float baseMul = 1.05f;

        bool newSynerge = (slotNum == 4);
        bool oldSynerge = activeInv.GetTagCount(ItemTag.Old) >= 2;
        if (newSynerge != synerge_Center || oldSynerge != synerge_Old)
        {
            GameManager.instance.SM.RemoveModifiersByTag(this.name);
            synerge_Center = newSynerge;
            synerge_Old = oldSynerge;

            if (synerge_Center) baseMul += 0.02f;
            if (synerge_Old) baseMul += 0.03f;
            GameManager.instance.SM.AddModifier("defenceRate", multiplier: baseMul, tag: this.name);
        }

    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, itemName);
        GameManager.instance.SM.RemoveModifiersByTag(this.name);
    }
    */
}
