using UnityEngine;
using Scene;

public class CellBox : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private GameObject itemObject;
    private CollectSceneManager CCM;

    private ItemRarity itemRarity;  
    private bool isArrived;
    private Vector2 arrivePos;

    private Vector2 startPos;
    private float moveTime = 0.15f;   // 이동에 걸리는 시간 (초)
    private float elapsedTime;     // 경과 시간

    private void Awake()
    {
        isArrived = false;
        CCM=GameObject.Find("CollectSceneManager").GetComponent<CollectSceneManager>();
    }
    public void Init(Transform pos,ItemRarity itemR)
    {
        startPos = transform.position; // 시작 위치 기록
        arrivePos = pos.position;      // 도착 위치 기록
        elapsedTime = 0f;              // 시간 초기화
        isArrived = false;
        itemRarity= itemR;
    }

    private void Update()
    {
        if (isArrived) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveTime);

        // t를 부드럽게 → 시작은 빠르고 끝은 느려짐
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector2.Lerp(startPos, arrivePos, smoothT);

        if (t >= 1f)
        {
            isArrived = true;
        }
    }
    private void OnMouseDown()
    {
        SpawnItem();
        
    }
    public void SpawnItem()
    {
        if (!isArrived) return;
        GameObject GO = Instantiate(itemObject, transform.position, Quaternion.identity);
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        GO.GetComponent<ItemObj>().Init(CCM.GetRandomItem(itemRarity));
        CCM.RemoveFromList(this);
        Destroy(gameObject);
    }
}