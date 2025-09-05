using InventorySystem;
using UnityEngine;
using Core;

namespace Item
{
    /// <summary>
    /// 인벤토리 패널의 열기/닫기와 
    /// 게임 타임스케일(일시정지) 제어를 담당합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class InvManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// 토글할 인벤토리 UI 패널 오브젝트
        /// </summary>
        [Tooltip("인벤토리 UI 패널을 드래그하여 할당하세요.")]
        [SerializeField] private GameObject inventoryObj;

        /// <summary>
        /// 인벤토리 열기 시 플레이어 이동을 비활성화하기 위한 컴포넌트
        /// </summary>
        [Tooltip("PlayerMove 컴포넌트를 드래그하여 할당하세요.")]
        [SerializeField] private PlayerMove PM;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 게임 시작 시 기본 아이템을 인벤토리에 추가합니다.
        /// InventoryController 인스턴스가 없으면 오류를 출력합니다.
        /// </summary>
        private void Start()
        {
            InventoryController.instance.AddItem("Inventory", "Ruby", 2);
        }

        /// <summary>
        /// 매 프레임 "I" 키 입력을 확인하고 인벤토리 토글을 호출합니다.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowPausePanel();
            }
        }

        /// <summary>
        /// 인벤토리 패널을 열거나 닫고, TimeScaleManager로 게임 일시정지/재개를 처리합니다.
        /// </summary>
        public void ShowPausePanel()
        {
            inventoryObj.SetActive(!inventoryObj.activeSelf);
            if (inventoryObj.activeSelf)
            {
                TimeScaleManager.Instance.TimeStopStackPlus();
            }
            else
            {
                TimeScaleManager.Instance.TimeStopStackMinus();
            }
        }

        #endregion
    }
}