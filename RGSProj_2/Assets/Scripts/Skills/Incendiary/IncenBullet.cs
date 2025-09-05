using Core;
using UnityEngine;

public class IncenBullet : MonoBehaviour
{
    public float moveDuration = 1.0f;
    public float arcHeight = 0.3f;

    private Vector2 startPos;
    private Vector2 endPos;
    private float elapsedTime = 0f;
    private bool isMoving = false;
    private SO_IncenData skilData;
    private int nowLevel;
    private Skill_Incendiary SI;

    private void Awake()
    {
    }
    public void Attack(Vector2 targetPosition,SO_IncenData data,int level,Skill_Incendiary si)
    {
        startPos = transform.position;
        endPos = targetPosition;
        elapsedTime = 0f;
        isMoving = true;
        skilData = data;
        nowLevel = level;
        SI = si;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);
        transform.rotation = Quaternion.Euler(0, t * 360, t * 720);
        // 평면상 위치 보간 (X와 Y)
        Vector2 flatPos = Vector2.Lerp(startPos, endPos, t);

        // 포물선 높이 계산 (Y축을 높이처럼 씀)
        float arc = arcHeight * t * (1 - t); // 포물선 공식

        // 최종 위치: flatPos는 X와 Y, arc는 높이로써 Y축에 추가
        transform.position = new Vector3(flatPos.x, flatPos.y + arc, 0f);

        if (t >= 1f)
        {
            isMoving = false;
            transform.position = new Vector3(endPos.x, endPos.y, 0f); // 정확히 도착 위치 보정
            if (SI.reinforcedNum != 1)
            {
                GameObject incenArea = GameManager.Instance.poolManager.Get(9);
                incenArea.transform.position = transform.position;
                if (SI.reinforcedNum != 3)
                {
                    incenArea.GetComponent<IncenPrefabMain>().Init(skilData.incenDuration[nowLevel], skilData.attackDamage[nowLevel], skilData.incenRadius[nowLevel], SI);
                }
                else
                {
                    incenArea.GetComponent<IncenPrefabMain>().Init(skilData.incenDuration[nowLevel], SI.incenDamage, SI.incenRadius, SI);
                }

            }
            else
            {
                GameObject incenArea = GameManager.Instance.poolManager.Get(11);
                incenArea.transform.position = transform.position;
                incenArea.GetComponent<IncenGradePrefab>().Init(skilData.incenDuration[nowLevel], skilData.attackDamage[nowLevel], skilData.incenRadius[nowLevel], SI);

            }
            gameObject.SetActive(false);
        }
    }
}
