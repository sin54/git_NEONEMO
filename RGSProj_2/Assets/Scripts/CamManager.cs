using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CamManager : MonoBehaviour
{
    private CinemachineCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin perlin;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineCamera>();
        perlin=GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        if (perlin == null) return;
        StopAllCoroutines(); // 이전 흔들림 중지
        StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    private IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
    {
        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        perlin.AmplitudeGain = 0f;
        perlin.FrequencyGain = 0f;
    }
}
