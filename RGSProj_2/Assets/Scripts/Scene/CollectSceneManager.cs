using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using InventorySystem;
using System;

using Random = UnityEngine.Random;
public class CollectSceneManager : MonoBehaviour
{
    [SerializeField] private Transform[] itemSpawnPos;
    [SerializeField] private CollectCrate CC;

    [SerializeField] private GameObject[] boxes;
    [SerializeField] private Transform[] savePos;

    private List<List<InventoryItem>> EItems;
    private List<CellBox> itemList=new List<CellBox>();
    private List<GameObject> itemObjs=new List<GameObject>();

    public int maxClickNum;
    public float increasePercent;
    public float decreasePercent;

    public float[] percentages=new float[11];
    private int[] selected;
    private bool isExploding;
    private void Awake()
    {
        EItems = new List<List<InventoryItem>>(11);

        for (int i = 0; i < 11; i++)
        {
            EItems.Add(new List<InventoryItem>());
        }
    }
    private void Start()
    {
        List<ItemInitializer> itemL = InventoryController.instance.items;
        for (int i = 0; i < itemL.Count; i++) {
            if (!itemL[i].GetIsWeapon())
            {
                EItems[(int)itemL[i].GetRarity()].Add(itemL[i].GetInventoryItem());
            }
        }

        for (int i = 0; i < EItems.Count; i++)
        {
            string contents = $"Rarity {i}: ";
            for (int j = 0; j < EItems[i].Count; j++)
            {
                contents += EItems[i][j].GetItemType() + ", "; // InventoryItem 이름 출력
            }
            Debug.Log(contents);
        }

        isExploding = false;
    }
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

    private void SpawnBox(ItemRarity IR)
    {
        Debug.Log(IR);
        if (IR == ItemRarity.E || IR == ItemRarity.D || IR == ItemRarity.C)
        {
            GameObject GO=Instantiate(boxes[0], transform.position, Quaternion.Euler(0,0,Random.Range(0,360f)));
            GO.GetComponent<CellBox>().Init(SetRandomPos(), IR);
            itemList.Add(GO.GetComponent<CellBox>());
        }
        else if (IR == ItemRarity.B || IR == ItemRarity.Bp) {
            GameObject GO=Instantiate(boxes[1], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            GO.GetComponent<CellBox>().Init(SetRandomPos(),IR);
            itemList.Add(GO.GetComponent<CellBox>());
        }
        else if (IR == ItemRarity.A || IR == ItemRarity.Ap)
        {
            GameObject GO = Instantiate(boxes[2], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            GO.GetComponent<CellBox>().Init(SetRandomPos(),IR);
            itemList.Add(GO.GetComponent<CellBox>());
        }
        else if (IR == ItemRarity.S || IR == ItemRarity.Sp || IR == ItemRarity.X) {
            GameObject GO = Instantiate(boxes[3], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            GO.GetComponent<CellBox>().Init(SetRandomPos(),IR);
            itemList.Add(GO.GetComponent<CellBox>());
        }
        else
        {
            GameObject GO = Instantiate(boxes[4], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            GO.GetComponent<CellBox>().Init(SetRandomPos(),IR);
            itemList.Add(GO.GetComponent<CellBox>());
        }

    }
    public InventoryItem GetRandomItem(ItemRarity IR)
    {
        return EItems[(int)IR][Random.Range(0, EItems[(int)IR].Count)];
    }
    public void RemoveFromList(CellBox CB)
    {
        itemList.Remove(CB);
    }
}
