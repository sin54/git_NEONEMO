using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using InventorySystem;
using System;

using Random = UnityEngine.Random;
using Core;
using UnityEngine.SceneManagement;

namespace Scenes
{
    /// <summary>
    /// 수집 씬에서 상자 및 아이템 스폰, 폭발 처리, 수집 리스트 관리를 담당합니다.
    /// </summary>
    public class CollectSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>아이템이 스폰될 위치 목록</summary>
        [SerializeField] private Transform[] itemSpawnPos;

        /// <summary>크레이트(상자) 관리용 컴포넌트</summary>
        [SerializeField] private CollectCrate CC;

        /// <summary>레어도별 박스 프리팹 배열</summary>
        [SerializeField] private GameObject[] boxes;

        /// <summary>수집된 아이템을 저장할 위치 목록</summary>
        [SerializeField] private Transform[] savePos;

        /// <summary>최대 클릭 가능 횟수</summary>
        [SerializeField] public int maxClickNum;

        /// <summary>아이템 증가 확률</summary>
        [SerializeField] public float increasePercent;

        /// <summary>아이템 감소 확률</summary>
        [SerializeField] public float decreasePercent;

        /// <summary>0~10 인덱스로 레어도 확률을 지정하는 배열</summary>
        [SerializeField] public float[] percentages = new float[11];

        #endregion

        #region Fields

        /// <summary>레어도별로 분류된 InventoryItem 리스트</summary>
        private List<List<InventoryItem>> EItems;

        /// <summary>씬에 생성된 CellBox 인스턴스 목록</summary>
        private List<CellBox> itemList = new List<CellBox>();

        /// <summary>현재 활성화된 박스 오브젝트 목록(필요시 사용)</summary>
        private List<GameObject> itemObjs = new List<GameObject>();

        /// <summary>이미 선택된 스폰 인덱스 배열</summary>
        private int[] selected;

        /// <summary>폭발 애니메이션 진행 중 플래그</summary>
        private bool isExploding;

        public List<InventoryItem> collectedItems=new List<InventoryItem>();

        public bool isBoxBreak;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Awake 단계에서 레어도 수(11)만큼 빈 리스트를 초기화합니다.
        /// </summary>
        private void Awake()
        {
            EItems = new List<List<InventoryItem>>(11);

            for (int i = 0; i < 11; i++)
            {
                EItems.Add(new List<InventoryItem>());
            }
        }

        /// <summary>
        /// Start 단계에서 InventoryController로부터 아이템을 가져와 레어도별로 분류하고 로그를 출력합니다.
        /// </summary>
        private void Start()
        {
            isBoxBreak = false;
            List<ItemInitializer> itemL = InventoryController.instance.items;
            for (int i = 0; i < itemL.Count; i++) {
                if (!itemL[i].GetIsWeapon())
                {
                    EItems[(int)itemL[i].GetRarity()].Add(itemL[i].GetInventoryItem());
                }
            }
            /*
            for (int i = 0; i < EItems.Count; i++)
            {
                string contents = $"Rarity {i}: ";
                for (int j = 0; j < EItems[i].Count; j++)
                {
                    contents += EItems[i][j].GetItemType() + ", "; // InventoryItem 이름 출력
                }
                Debug.Log(contents);
            }
            */

            isExploding = false;
        }

        /// <summary>
        /// 매 프레임 우클릭을 감지해 폭발 코루틴을 트리거합니다.
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!isExploding)
                {
                    isExploding = true;
                    StartCoroutine(DelayExplode());
                }
            }
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// 모든 CellBox에 대해 순차적으로 SpawnItem을 호출하며 0.1초 간격을 둡니다.
        /// </summary>
        private IEnumerator DelayExplode()
        {
            // Create a single copy of the list
            if (itemList == null)
            {
                Debug.LogError("itemList is null!");
                isExploding = false;
                yield break;
            }

            var itemListCopy = itemList.ToList();

            // Iterate over the copied list
            foreach (var item in itemListCopy)
            {
                if (item != null) // Safety check for null items
                {
                    item.SpawnItem();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            isExploding = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 아이템 박스를 스폰합니다. 확률에 따라 해당 레어도의 박스를 생성합니다.
        /// </summary>
        public void SpawnItem()
        {
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < itemSpawnPos.Length; i++)
            {
                if (selected == null || !selected.Contains(i)) // 아직 선택되지 않은 인덱스
                {
                    availableIndices.Add(i);
                }
            }

            // 남은 위치가 없으면 null 반환 (혹은 첫번째 자리 등 원하는 처리)
            if (availableIndices.Count == 0)
            {
                return;
            }
            float random = Random.value; // 0 ~ 1
            float cumulative = 0f;

            cumulative += percentages[0];
            if (random < cumulative) { SpawnBox(ItemRarity.E); return; }

            cumulative += percentages[1];
            if (random < cumulative) { SpawnBox(ItemRarity.D); return; }

            cumulative += percentages[2];
            if (random < cumulative) { SpawnBox(ItemRarity.C); return; }

            cumulative += percentages[3];
            if (random < cumulative) { SpawnBox(ItemRarity.B); return; }

            cumulative += percentages[4];
            if (random < cumulative) { SpawnBox(ItemRarity.Bp); return; }

            cumulative += percentages[5];
            if (random < cumulative) { SpawnBox(ItemRarity.A); return; }

            cumulative += percentages[6];
            if (random < cumulative) { SpawnBox(ItemRarity.Ap); return; }

            cumulative += percentages[7];
            if (random < cumulative) { SpawnBox(ItemRarity.S); return; }

            cumulative += percentages[8];
            if (random < cumulative) { SpawnBox(ItemRarity.Sp); return; }

            cumulative += percentages[9];
            if (random < cumulative) { SpawnBox(ItemRarity.X); return; }

            // 마지막은 Unique
            SpawnBox(ItemRarity.Uni);
        }

        /// <summary>
        /// 지정한 레어도의 랜덤 InventoryItem을 반환합니다.
        /// </summary>
        /// <param name="rarity">아이템 레어도</param>
        /// <returns>랜덤으로 선택된 InventoryItem</returns>
        public InventoryItem GetRandomItem(ItemRarity IR)
        {
            return EItems[(int)IR][Random.Range(0, EItems[(int)IR].Count)];
        }

        /// <summary>
        /// 수집 리스트에서 특정 CellBox를 제거합니다.
        /// </summary>
        /// <param name="cb">제거할 CellBox 인스턴스</param>
        public void RemoveFromList(CellBox CB)
        {
            itemList.Remove(CB);
        }

        public void AddCollectedItem(InventoryItem II)
        {
            collectedItems.Add(II);
        }
        public void RemoveCollectedItem(InventoryItem II)
        {
            collectedItems.Remove(II);
        }
        public void FinishCollect()
        {
            GameManager.Instance.EndCollect(collectedItems);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 사용되지 않은 스폰 포지션을 랜덤으로 선택하고 selected 배열을 갱신합니다.
        /// </summary>
        /// <returns>선택된 Transform, 모두 사용된 경우 null</returns>
        private Transform SetRandomPos()
        {
            // 아직 선택되지 않은 위치들을 담을 리스트
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < itemSpawnPos.Length; i++)
            {
                if (selected == null || !selected.Contains(i)) // 아직 선택되지 않은 인덱스
                {
                    availableIndices.Add(i);
                }
            }

            // 남은 위치가 없으면 null 반환 (혹은 첫번째 자리 등 원하는 처리)
            if (availableIndices.Count == 0)
            {
                Debug.LogWarning("모든 위치가 이미 선택됨!");
                return null;
            }

            // 랜덤으로 하나 선택
            int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];

            // 선택 기록에 추가
            if (selected == null)
            {
                selected = new int[] { randomIndex };
            }
            else
            {
                selected = selected.Concat(new int[] { randomIndex }).ToArray();
            }

            return itemSpawnPos[randomIndex];
        }

        /// <summary>
        /// 주어진 레어도의 박스 프리팹을 인스턴스화하고 초기화한 뒤 리스트에 추가합니다.
        /// </summary>
        /// <param name="rarity">박스 레어도</param>
        private void SpawnBox(ItemRarity IR)
        {

            // Additive로 불러온 CollectScene 가져오기
            Scene collectScene = SceneManager.GetSceneByName("CollectScene");
            if (!collectScene.isLoaded)
            {
                Debug.LogError("CollectScene이 아직 로드되지 않았습니다!");
                return;
            }

            GameObject prefab = null;
            if (IR == ItemRarity.E || IR == ItemRarity.D || IR == ItemRarity.C)
                prefab = boxes[0];
            else if (IR == ItemRarity.B || IR == ItemRarity.Bp)
                prefab = boxes[1];
            else if (IR == ItemRarity.A || IR == ItemRarity.Ap)
                prefab = boxes[2];
            else if (IR == ItemRarity.S || IR == ItemRarity.Sp || IR == ItemRarity.X)
                prefab = boxes[3];
            else
                prefab = boxes[4];

            GameObject GO = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));

            // 생성된 오브젝트를 CollectScene으로 이동
            SceneManager.MoveGameObjectToScene(GO, collectScene);

            GO.GetComponent<CellBox>().Init(SetRandomPos(), IR);
            itemList.Add(GO.GetComponent<CellBox>());

        }
        
        #endregion
    }
}