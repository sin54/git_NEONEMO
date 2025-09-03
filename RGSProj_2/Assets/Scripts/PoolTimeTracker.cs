using UnityEngine;

public class PoolTimeTracker : MonoBehaviour
{
    public float lastDisabledTime = -10f;

    private void OnDisable()
    {
        lastDisabledTime = Time.time;
    }
}
