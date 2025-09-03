using UnityEngine;

public class SpinAroundForEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] spikes;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float speed = 1f;

    private float[] angles;

    private void Start()
    {
        angles = new float[spikes.Length];

        // evenly distribute spikes
        float angleStep = 360f / spikes.Length;
        for (int i = 0; i < spikes.Length; i++)
        {
            angles[i] = i * angleStep;
        }
    }

    private void Update()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            // 회전
            angles[i] += speed * Time.deltaTime;
            if (angles[i] >= 360f) angles[i] -= 360f;

            float rad = angles[i] * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            spikes[i].transform.localPosition = offset;

            // 중심을 등지도록 방향 설정 (바깥을 바라보게)
            Vector3 directionFromCenter = offset.normalized;
            float angleDeg = Mathf.Atan2(directionFromCenter.y, directionFromCenter.x) * Mathf.Rad2Deg;
            spikes[i].transform.localRotation = Quaternion.Euler(0, 0, angleDeg - 90f);
        }
    }
}
