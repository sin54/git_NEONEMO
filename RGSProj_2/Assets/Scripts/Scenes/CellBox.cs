using UnityEngine;
using Scenes;
using UnityEngine.SceneManagement;

public class CellBox : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private GameObject itemObject;
    private CollectSceneManager CCM;

    private ItemRarity itemRarity;  
    private bool isArrived;
    private Vector2 arrivePos;

    private Vector2 startPos;
    private float moveTime = 0.15f;   // �̵��� �ɸ��� �ð� (��)
    private float elapsedTime;     // ��� �ð�

    private void Awake()
    {
        isArrived = false;
        CCM=GameObject.Find("CollectSceneManager").GetComponent<CollectSceneManager>();
    }
    private void Start()
    {
    }
    public void Init(Transform pos,ItemRarity itemR)
    {
        startPos = transform.position; // ���� ��ġ ���
        arrivePos = pos.position;      // ���� ��ġ ���
        elapsedTime = 0f;              // �ð� �ʱ�ȭ
        isArrived = false;
        itemRarity= itemR;
    }

    private void Update()
    {
        if (isArrived) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveTime);

        // t�� �ε巴�� �� ������ ������ ���� ������
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

        // CollectScene ��������
        Scene collectScene = SceneManager.GetSceneByName("CollectScene");
        if (!collectScene.isLoaded)
        {
            Debug.LogError("CollectScene�� ���� �ε���� �ʾҽ��ϴ�!");
            return;
        }

        GameObject GO = Instantiate(itemObject, transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(GO, collectScene); // �� �̵�

        Instantiate(destroyParticle, transform.position, Quaternion.identity);

        GO.GetComponent<ItemObj>().Init(CCM.GetRandomItem(itemRarity));
        CCM.RemoveFromList(this);
        Destroy(gameObject);
    }
    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}