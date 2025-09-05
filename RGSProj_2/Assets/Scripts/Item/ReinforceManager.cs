using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class ReinforceManager : MonoBehaviour
{
    [SerializeField] private int numOfToken;
    [SerializeField] private TMP_Text leftTokenNum;
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button FinishButton;

    private int[] reinforceDataDict=new int[8];

    public int LeftToken { get; private set; }  
    public void StartReinforce(int[] data)
    {
        FinishButton.interactable = false;
        LeftToken = numOfToken;
        UpdateTokenNumTxt();
        reinforceDataDict = (int[])data.Clone();
        InitText();
    }
    public void EndReinforce()
    {
        GameManager.Instance.EndReinforce(reinforceDataDict);
    }
    public void UpdateValue(int index)
    {
        LeftToken--;
        UpdateTokenNumTxt();
        if (index == 0)
        {
            GameManager.Instance.player.IncreaseMaxHealth(15);
        }
        else if (index == 1)
        {
            GameManager.Instance.SM.AddModifier("AtkMul",additive:0.08f,tag:"Reinforce_Attack");
        }
        else if (index == 2)
        {
            GameManager.Instance.SM.AddModifier("NaturalHeal", additive: 0.1f, tag: "Reinforce_Heal");
        }
        else if (index == 3)
        {
            GameManager.Instance.SM.RemoveModifiersByTag("Reinforce_Dodge");
            GameManager.Instance.SM.AddModifier("dodgeMul", multiplier: 1 - 0.05f * (reinforceDataDict[3]+1), tag: "Reinforce_Dodge");
        }
        else if (index == 4)
        {
            GameManager.Instance.SM.RemoveModifiersByTag("Reinforce_Defence");
            GameManager.Instance.SM.AddModifier("defenceRate", multiplier: 1 - (0.05f * (reinforceDataDict[3] + 1)), tag: "Reinforce_Defence");
        }
        else if (index == 5)
        {
            GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.05f, tag: "Reinforce_Speed");
        }
        else if (index == 6)
        {
            GameManager.Instance.SM.AddModifier("ItemRange", additive: 0.1f, tag: "Reinforce_ItemR");
        }
        else if (index == 7)
        {
            GameManager.Instance.SM.AddModifier("xpMul", additive: 0.08f, tag: "Reinforce_ItemX");
        }
        if (LeftToken <= 0)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            FinishButton.interactable = true;
        }
        reinforceDataDict[index]++;
        InitText();
    }
    private void InitText()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "LV "+reinforceDataDict[i].ToString();
            if (reinforceDataDict[i] >= 5)
            {
                buttons[i].interactable = false;
                texts[i].color = Color.red;
                texts[i].text = "MAX";
            }
        }
        
    }
    private void UpdateTokenNumTxt()
    {
        leftTokenNum.text=LeftToken.ToString();
    }
}
