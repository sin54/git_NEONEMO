using UnityEngine;

public class StunEffect : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 0f, 90f * Time.deltaTime);
    }
}
