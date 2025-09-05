using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// 미리 지정한 프리팹을 풀링하여 재사용하고,
    /// 필요 시 자동으로 PoolTimeTracker를 붙여 재사용 대기 시간을 관리합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class PoolManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// 인덱스별로 풀링할 프리팹 배열 (Inspector에서 할당)  
        /// </summary>
        [Tooltip("풀링용 프리팹을 인덱스 순서대로 할당하세요.")]
        public GameObject[] prefabs;

        /// <summary>
        /// 인덱스별 생성된 오브젝트 리스트</summary>
        List<GameObject>[] pools;

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake 시점에 풀 리스트를 초기화합니다.
        /// </summary>
        private void Awake()
        {
            // prefabs 길이만큼 빈 리스트 생성
            pools = new List<GameObject>[prefabs.Length];
            for (int idx = 0; idx < pools.Length; idx++)
            {
                pools[idx] = new List<GameObject>();
            }
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// 지정한 인덱스의 오브젝트를 반환합니다.
        /// 비활성화된 오브젝트 중 재사용 가능 시간이 지난 첫 번째 아이템을 활성화하여 반환하며,
        /// 없으면 새로 인스턴스화합니다.
        /// </summary>
        /// <param name="index">prefabs 배열의 인덱스</param>
        /// <returns>풀에서 얻은 GameObject 인스턴스</returns>
        public GameObject Get(int index)
        {
            float now = Time.time;
            GameObject select = null;

            foreach (GameObject item in pools[index])
            {
                if (!item.activeSelf)
                {
                    PoolTimeTracker tracker = item.GetComponent<PoolTimeTracker>();
                    if (tracker != null)
                    {
                        if (now - tracker.lastDisabledTime < 1f)
                            continue; // 아직 재사용 금지
                    }

                    select = item;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);

                // 자동으로 PoolTimeTracker 붙이기
                if (select.GetComponent<PoolTimeTracker>() == null)
                {
                    select.AddComponent<PoolTimeTracker>();
                }
            }

            // BaseItem 초기화는 여전히 존재할 수 있음 (필요한 경우에만)
            BaseItem isItem = select.GetComponent<BaseItem>();
            if (isItem != null)
            {
                isItem.isCollected = false;
            }

            return select;
        }

        /// <summary>
        /// 지정한 인덱스의 오브젝트를 불러오고, 크기를 설정하여 반환합니다.
        /// </summary>
        /// <param name="index">prefabs 배열의 인덱스</param>
        /// <param name="size">로컬 스케일로 적용할 크기</param>
        /// <returns>풀에서 얻은 GameObject 인스턴스</returns>
        public GameObject Get(int index, Vector3 size)
        {
            GameObject obj = Get(index);
            if (obj != null)
            {
                obj.transform.localScale = size;
            }
            return obj;
        }

        /// <summary>
        /// 자식 트랜스폼에 속한 모든 GameObject를 비활성화합니다.
        /// </summary>
        public void DeActiveAllChild()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        #endregion
    }
}