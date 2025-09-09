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
    /// ���� ������ ���� �� ������ ����, ���� ó��, ���� ����Ʈ ������ ����մϴ�.
    /// </summary>
    public class CollectSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>�������� ������ ��ġ ���</summary>
        [SerializeField] private Transform[] itemSpawnPos;

        /// <summary>ũ����Ʈ(����) ������ ������Ʈ</summary>
        [SerializeField] private CollectCrate CC;

        /// <summary>����� �ڽ� ������ �迭</summary>
        [SerializeField] private GameObject[] boxes;

        /// <summary>������ �������� ������ ��ġ ���</summary>
        [SerializeField] private Transform[] savePos;

        /// <summary>�ִ� Ŭ�� ���� Ƚ��</summary>
        [SerializeField] public int maxClickNum;

        /// <summary>������ ���� Ȯ��</summary>
        [SerializeField] public float increasePercent;

        /// <summary>������ ���� Ȯ��</summary>
        [SerializeField] public float decreasePercent;

        /// <summary>0~10 �ε����� ��� Ȯ���� �����ϴ� �迭</summary>
        [SerializeField] public float[] percentages = new float[11];

        #endregion

        #region Fields

        /// <summary>������� �з��� InventoryItem ����Ʈ</summary>
        private List<List<InventoryItem>> EItems;

        /// <summary>���� ������ CellBox �ν��Ͻ� ���</summary>
        private List<CellBox> itemList = new List<CellBox>();

        /// <summary>���� Ȱ��ȭ�� �ڽ� ������Ʈ ���(�ʿ�� ���)</summary>
        private List<GameObject> itemObjs = new List<GameObject>();

        /// <summary>�̹� ���õ� ���� �ε��� �迭</summary>
        private int[] selected;

        /// <summary>���� �ִϸ��̼� ���� �� �÷���</summary>
        private bool isExploding;

        public List<InventoryItem> collectedItems=new List<InventoryItem>();

        public bool isBoxBreak;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Awake �ܰ迡�� ��� ��(11)��ŭ �� ����Ʈ�� �ʱ�ȭ�մϴ�.
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
        /// Start �ܰ迡�� InventoryController�κ��� �������� ������ ������� �з��ϰ� �α׸� ����մϴ�.
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
                    contents += EItems[i][j].GetItemType() + ", "; // InventoryItem �̸� ���
                }
                Debug.Log(contents);
            }
            */

            isExploding = false;
        }

        /// <summary>
        /// �� ������ ��Ŭ���� ������ ���� �ڷ�ƾ�� Ʈ�����մϴ�.
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
        /// ��� CellBox�� ���� ���������� SpawnItem�� ȣ���ϸ� 0.1�� ������ �Ӵϴ�.
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
        /// ������ �ڽ��� �����մϴ�. Ȯ���� ���� �ش� ����� �ڽ��� �����մϴ�.
        /// </summary>
        public void SpawnItem()
        {
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < itemSpawnPos.Length; i++)
            {
                if (selected == null || !selected.Contains(i)) // ���� ���õ��� ���� �ε���
                {
                    availableIndices.Add(i);
                }
            }

            // ���� ��ġ�� ������ null ��ȯ (Ȥ�� ù��° �ڸ� �� ���ϴ� ó��)
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

            // �������� Unique
            SpawnBox(ItemRarity.Uni);
        }

        /// <summary>
        /// ������ ����� ���� InventoryItem�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="rarity">������ ���</param>
        /// <returns>�������� ���õ� InventoryItem</returns>
        public InventoryItem GetRandomItem(ItemRarity IR)
        {
            return EItems[(int)IR][Random.Range(0, EItems[(int)IR].Count)];
        }

        /// <summary>
        /// ���� ����Ʈ���� Ư�� CellBox�� �����մϴ�.
        /// </summary>
        /// <param name="cb">������ CellBox �ν��Ͻ�</param>
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
        /// ������ ���� ���� �������� �������� �����ϰ� selected �迭�� �����մϴ�.
        /// </summary>
        /// <returns>���õ� Transform, ��� ���� ��� null</returns>
        private Transform SetRandomPos()
        {
            // ���� ���õ��� ���� ��ġ���� ���� ����Ʈ
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < itemSpawnPos.Length; i++)
            {
                if (selected == null || !selected.Contains(i)) // ���� ���õ��� ���� �ε���
                {
                    availableIndices.Add(i);
                }
            }

            // ���� ��ġ�� ������ null ��ȯ (Ȥ�� ù��° �ڸ� �� ���ϴ� ó��)
            if (availableIndices.Count == 0)
            {
                Debug.LogWarning("��� ��ġ�� �̹� ���õ�!");
                return null;
            }

            // �������� �ϳ� ����
            int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];

            // ���� ��Ͽ� �߰�
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
        /// �־��� ����� �ڽ� �������� �ν��Ͻ�ȭ�ϰ� �ʱ�ȭ�� �� ����Ʈ�� �߰��մϴ�.
        /// </summary>
        /// <param name="rarity">�ڽ� ���</param>
        private void SpawnBox(ItemRarity IR)
        {

            // Additive�� �ҷ��� CollectScene ��������
            Scene collectScene = SceneManager.GetSceneByName("CollectScene");
            if (!collectScene.isLoaded)
            {
                Debug.LogError("CollectScene�� ���� �ε���� �ʾҽ��ϴ�!");
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

            // ������ ������Ʈ�� CollectScene���� �̵�
            SceneManager.MoveGameObjectToScene(GO, collectScene);

            GO.GetComponent<CellBox>().Init(SetRandomPos(), IR);
            itemList.Add(GO.GetComponent<CellBox>());

        }
        
        #endregion
    }
}