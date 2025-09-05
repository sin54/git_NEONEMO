using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Item
{
    /// <summary>
    /// 강화 미니게임 UI 및 데이터 처리를 담당합니다.
    /// 토큰 사용을 통해 다양한 능력치를 증가시키고,
    /// 완료 시 GameManager에 결과를 전달합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class ReinforceManager : MonoBehaviour
    {
        #region Fields

        /// <summary>강화에 사용할 초기 토큰 개수</summary>
        [SerializeField] private int numOfToken;

        /// <summary>남은 토큰 개수를 표시할 텍스트</summary>
        [SerializeField] private TMP_Text leftTokenNum;

        /// <summary>강화 레벨을 표시할 텍스트 배열</summary>
        [SerializeField] private TMP_Text[] texts;

        /// <summary>각 강화 항목 버튼 배열</summary>
        [SerializeField] private Button[] buttons;

        /// <summary>강화를 완료할 때 활성화할 버튼</summary>
        [SerializeField] private Button FinishButton;

        /// <summary>각 항목별 강화 횟수를 저장하는 배열</summary>
        private int[] reinforceDataDict = new int[8];

        #endregion

        #region Properties

        /// <summary>현재 남은 토큰 개수</summary>
        public int LeftToken { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// 강화 씬이 시작될 때 호출됩니다.
        /// 초기 토큰 설정, 텍스트 초기화, 이전 강화 데이터 로드가 수행됩니다.
        /// </summary>
        public void StartReinforce(int[] data)
        {
            FinishButton.interactable = false;
            LeftToken = numOfToken;
            UpdateTokenNumTxt();
            reinforceDataDict = (int[])data.Clone();
            InitText();
        }

        /// <summary>
        /// 강화가 완료되면 호출됩니다.
        /// GameManager에 결과 데이터를 전달하고 씬을 종료합니다.
        /// </summary>
        public void EndReinforce()
        {
            GameManager.Instance.EndReinforce(reinforceDataDict);
        }

        /// <summary>
        /// 인덱스에 해당하는 강화 항목을 1 레벨 올리고,
        /// 토큰을 차감하며 UI, 스탯 수정자를 업데이트합니다.
        /// </summary>
        /// <param name="index">강화 항목 인덱스 (0~7)</param>
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

        #endregion

        #region Private Methods

        /// <summary>
        /// 강화 레벨 텍스트를 현재 데이터에 맞춰 갱신하고,
        /// 최대 레벨(5) 달성 시 버튼 비활성화 및 색상 변경을 처리합니다.
        /// </summary>
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

        /// <summary>
        /// 남은 토큰 개수 텍스트를 갱신합니다.
        /// </summary>
        private void UpdateTokenNumTxt()
        {
            leftTokenNum.text=LeftToken.ToString();
        }

        #endregion
    }
}