using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    private RaycastHit2D[] targets;

    public GameObject GetNearestEnemy()
    {
        targets = Physics2D.BoxCastAll(transform.position, new Vector2(100,100),0,Vector2.zero,0, targetLayer);
        return FindNearestEnemy();
    }
    private GameObject FindNearestEnemy()
    {
        Transform result = null;
        float diff = 10000;

        foreach (RaycastHit2D hit in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetpos = hit.transform.position;
            float curDiff = Vector3.Distance(myPos, targetpos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = hit.transform;
            }
        }
        return result!=null?result.gameObject:null;
    }
    public GameObject[] FindNearestEnemies(int count)
    {
        Vector2 range = new Vector2(100, 100);
        // 지정된 범위 내의 적 감지
        targets = Physics2D.BoxCastAll(transform.position, range, 0, Vector2.zero, 0, targetLayer);

        // 정렬을 위해 리스트로 변환
        List<RaycastHit2D> sortedTargets = targets
            .OrderBy(hit => Vector3.Distance(transform.position, hit.transform.position))
            .Take(count) // 가장 가까운 n개 선택
            .ToList();

        // GameObject 배열로 변환
        GameObject[] nearestEnemies = new GameObject[sortedTargets.Count];
        for (int i = 0; i < sortedTargets.Count; i++)
        {
            nearestEnemies[i] = sortedTargets[i].transform.gameObject;
        }

        return nearestEnemies;
    }
}
