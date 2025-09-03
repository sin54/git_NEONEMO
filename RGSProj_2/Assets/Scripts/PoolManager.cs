using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int idx = 0; idx < pools.Length; idx++)
        {
            pools[idx] = new List<GameObject>();
        }
    }

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

    public GameObject Get(int index, Vector3 size)
    {
        GameObject obj = Get(index);
        if (obj != null)
        {
            obj.transform.localScale = size;
        }
        return obj;
    }
    public void DeActiveAllChild()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
