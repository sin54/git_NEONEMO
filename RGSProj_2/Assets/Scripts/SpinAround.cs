using UnityEngine;

public class SpinAround : MonoBehaviour
{
    private Transform Target;
    public float radius;
    public float angularVelocity;

    private float angle; // 현재 각도 (도 단위)
    private bool angleInitialized = false;

    private void Awake()
    {
        Target = GameManager.instance.player.transform;
    }

    public void SetInitialAngle(float angleDeg)
    {
        this.angle = angleDeg;
        angleInitialized = true;
    }

    void Start()
    {
        if (!angleInitialized)
        {
            // 외부에서 초기화 안했으면 자동 계산
            Vector2 direction = transform.position - Target.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        // 각도 증가
        angle += angularVelocity * Time.deltaTime;

        // 새 위치 계산
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        transform.position = (Vector2)Target.position + offset;
    }
}
