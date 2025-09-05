using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_Excalibur", menuName = "Inventory/SkillEvent/X/Item_Excalibur")]
public class Item_Excalibur : InventoryItemSkillEvent
{
    private bool centerSyn;
    private bool aloneSyn;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = (slotNum == 4);
        float atkMul = 2.5f;
        if (newSyn != centerSyn)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.5f;
            GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID + "_Syn");
        }
        //처형컨디션 추가 ㄱㄱ
        bool newExeSyn=InventoryController.instance.GetInventory("Active").GetOccupiedSlotCount()==1;
        float exePercent = 0.25f;
        if (newExeSyn != aloneSyn)
        {
            GameManager.Instance.RemoveExeCondition(ID);
            aloneSyn= newExeSyn;
            if (aloneSyn) exePercent += 0.5f;
            GameManager.Instance.AddExeCondition(ID, e => e.currentHealth < e.maxHealth *exePercent);

        }
    
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        centerSyn = false;
        aloneSyn = false;
        GameManager.Instance.SM.AddModifier("P_AtkMul", multiplier: 2.5f, tag: ID + "_Syn");
        GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.2f, tag: ID);
        GameManager.Instance.AddExeCondition(ID, e => e.currentHealth< e.maxHealth*0.25f);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
        GameManager.Instance.RemoveExeCondition(ID);
    }
}