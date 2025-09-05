using UnityEngine;
using InventorySystem;

namespace InventorySystem
{
    /// <summary>
    /// 슬롯에 연결된 아이템을 초기화하고 관리합니다.
    /// Slot 컴포넌트를 캐싱하여 슬롯 관련 로직을 수행할 수 있도록 준비합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class SlotItemManager : MonoBehaviour
    {
        #region Fields


        /// <summary>
        /// Slot 컴포넌트 참조
        /// </summary>
        private Slot slot;

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake 시 Slot 컴포넌트를 가져와 캐싱합니다.
        /// Slot 컴포넌트가 없는 경우 에러를 출력합니다.
        /// </summary>
        private void Awake()
        {
            slot = GetComponent<Slot>();
            if (slot == null)
            {
                Debug.LogError($"Slot 컴포넌트를 찾을 수 없습니다. GameObject: {gameObject.name}", this);
            }

        }

        #endregion
    }
}

