
using UnityEngine;

public class LineARInitalizer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer1;
    [SerializeField] private LineRenderer lineRenderer2;
    [SerializeField] private GameObject sourceObj;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject tempObj;
    [SerializeField] private Repair_Target RP;

    private int hp;
    private RepairManager RM;
    private Vector2 targetPos;

    private void Awake()
    {
        RM=GameObject.Find("RepairManager").GetComponent<RepairManager>();
    }
    private void OnEnable()
    {
        RepairManager.OnGameFinished += DeleteThis;
    }
    private void OnDisable()
    {
        RepairManager.OnGameFinished -= DeleteThis;
    }
    public void InitObj(Vector2 targetP, Vector2 startPos, int HP) 
    {
        hp = HP;
        SetColorByHP();
        Vector2 squarePos;
        Vector2 turnPos;
        targetPos= targetP; 
        if (startPos.x < 0) { 
            squarePos=new Vector2(targetP.x+0.8f, targetP.y);
            turnPos = new Vector2(squarePos.x + 0.5f, targetP.y);
        }
        else
        {
            squarePos = new Vector2(targetP.x - 0.8f, targetP.y);
            turnPos = new Vector2(squarePos.x - 0.5f, targetP.y);
        }
        sourceObj.transform.position = startPos;
        targetObj.transform.position = targetP;
        lineRenderer1.SetPosition(0, startPos);
        lineRenderer1.SetPosition(1, turnPos);
        lineRenderer2.SetPosition(0, turnPos);
        lineRenderer2.SetPosition(1, squarePos);  
        tempObj.transform.position=squarePos;
        RP.SetupClickEvent(OnHitted);
    }

    private void OnHitted()
    {
        hp--;
        if (hp == 0) {
            RM.SpawnExplodeParticles(hp, targetPos);
            RemoveObj();
        }
        else
        {
            SetColorByHP();
            RM.SpawnExplodeParticles(hp, targetPos);
        }
    }
    private void RemoveObj()
    {
        RM.SpawnEnemy();
        RM.SpawnParticles(targetPos);
        Destroy(gameObject);
    }

    private void SetColorByHP()
    {
        if (hp == 3)
        {
            SetColor(Color.red);
        }
        else if (hp == 2)
        {
            SetColor(Color.yellow);
        }
        else if(hp==1)
        {
            SetColor(Color.green);
        }
        else
        {
            Debug.LogError("WARN");
        }
    }

    private void SetColor(Color color)
    {
        lineRenderer1.colorGradient = UtilClass.ChangeToGrad(color);
        lineRenderer2.colorGradient = UtilClass.ChangeToGrad(color);
        sourceObj.GetComponent<SpriteRenderer>().color = color;
        targetObj.GetComponent<SpriteRenderer>().color = color;
        tempObj.GetComponent<SpriteRenderer>().color = color;
    }
    public void DeleteThis()
    {
        Destroy(gameObject);
    }
}
