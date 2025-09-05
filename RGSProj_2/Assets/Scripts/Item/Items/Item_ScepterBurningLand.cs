using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_ScepterBurningLand", menuName = "Inventory/SkillEvent/S+/Item_ScepterBurningLand")]
public class Item_ScepterBurningLand : InventoryItemSkillEvent
{
    private bool isFireSynerge_Act;
    private bool isFireSynerge_Pas;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        float f_emp = 1.8f;
        float f_cool = 0.8f;

        bool fireSyn_A = GameManager.Instance.playerTypeManager.Types[1].typeActiveLevel == 5;
        bool fireSyn_P = GameManager.Instance.playerTypeManager.Types[1].typePassiveLevel == 4;

        if (fireSyn_A != isFireSynerge_Act)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID+"_SynA");
            isFireSynerge_Act = fireSyn_A;
            if (isFireSynerge_Act) f_emp += 0.4f;
            GameManager.Instance.SM.AddModifier("Fire_AtkMul", multiplier: f_emp, tag: ID + "_SynA");
        }
        if (fireSyn_P != isFireSynerge_Pas) {
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_SynP");
            isFireSynerge_Pas=fireSyn_P;
            if (isFireSynerge_Pas) f_cool -= 0.15f;
            GameManager.Instance.SM.AddModifier("F_Cool", multiplier: f_cool, tag: ID + "_SynP");
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        isFireSynerge_Act = false;
        isFireSynerge_Pas = false;
        GameManager.Instance.SM.AddModifier("M_AtkMul", multiplier: 1.3f, tag: ID);
        GameManager.Instance.SM.AddModifier("Fire_AtkMul", multiplier: 1.8f, tag: ID + "_SynA");
        GameManager.Instance.SM.AddModifier("F_Cool", multiplier: 0.8f, tag: ID+"_SynP");
        GameManager.Instance.AddExeCondition(ID, e => e.eSS.GetFireStack() >= 35);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID+"_SynA");
        GameManager.Instance.SM.RemoveModifiersByTag(ID + "_SynP");
        GameManager.Instance.RemoveExeCondition(ID);
    }
}