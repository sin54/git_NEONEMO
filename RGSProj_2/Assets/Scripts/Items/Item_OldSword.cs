using UnityEngine;
using InventorySystem;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "Item_OldSword", menuName = "Inventory/SkillEvent/Normal/OldSword")]
public class Item_OldSword : InventoryItemSkillEvent
{
    private bool synerge_Center;
    private bool synerge_Old;
    /*
    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, itemName);

        GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: 1.05f, tag: ID);

        // 초기 조건 기록
        synerge_Center = (slotNum == 4);
        synerge_Old = activeInv.GetTagCount(ItemTag.Old) >= 2;
    }

    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, itemName);

        float baseMul = 1.05f;

        bool newSynerge = (slotNum == 4);
        bool oldSynerge = activeInv.GetTagCount(ItemTag.Old) >= 2;

        // 조건 변화 시만 재적용
        if (newSynerge != synerge_Center || oldSynerge != synerge_Old)
        {
            GameManager.instance.SM.RemoveModifiersByTag(ID);

            synerge_Center = newSynerge;
            synerge_Old = oldSynerge;

            if (synerge_Center) baseMul += 0.02f;
            if (synerge_Old) baseMul += 0.03f;

            GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: baseMul, tag: ID);
        }
    }
    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, itemName);

        // 해당 슬롯의 버프만 제거
        GameManager.instance.SM.RemoveModifiersByTag(ID);
    }
    */
}
