using InventorySystem;
using UnityEngine;
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
            GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn");
            centerSyn = newSyn;
            if (centerSyn) atkMul += 0.5f;
            GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: atkMul, tag: ID + "_Syn");
        }
        //처형컨디션 추가 ㄱㄱ
        bool newExeSyn=InventoryController.instance.GetInventory("Active").GetOccupiedSlotCount()==1;
        float exePercent = 0.25f;
        if (newExeSyn != aloneSyn)
        {
            GameManager.instance.RemoveExeCondition(ID);
            aloneSyn= newExeSyn;
            if (aloneSyn) exePercent += 0.5f;
            GameManager.instance.AddExeCondition(ID, e => e.currentHealth < e.maxHealth *exePercent);

        }
    
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        centerSyn = false;
        aloneSyn = false;
        GameManager.instance.SM.AddModifier("P_AtkMul", multiplier: 2.5f, tag: ID + "_Syn");
        GameManager.instance.SM.AddModifier("PlayerSpeed", additive: 0.2f, tag: ID);
        GameManager.instance.AddExeCondition(ID, e => e.currentHealth< e.maxHealth*0.25f);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID + "_Syn");
        GameManager.instance.RemoveExeCondition(ID);
    }
}