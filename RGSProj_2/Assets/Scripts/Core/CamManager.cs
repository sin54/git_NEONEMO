using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

namespace Core
{
    /// <summary>
    /// Cinemachine Virtual Camera의 Basic Multi-Channel Perlin 노이즈를 사용하여
    /// 카메라 흔들림 효과를 처리합니다.
    /// 해당 GameObject에는 CinemachineVirtualCamera와
    /// CinemachineBasicMultiChannelPerlin 컴포넌트가 있어야 합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class CamManager : MonoBehaviour
    {
    
        private CinemachineCamera virtualCam;
        private CinemachineBasicMultiChannelPerlin perlin;

        /// <summary>
        /// Awake 시점에 CinemachineVirtualCamera와 Perlin 노이즈 컴포넌트를 캐싱합니다.
        /// </summary>
        private void Awake()
        {
            virtualCam = GetComponent<CinemachineCamera>();
            perlin=GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        /// 지정한 세기와 진동수, 지속 시간으로 카메라 흔들림을 시작합니다.
        /// </summary>
        /// <param name="amplitude">흔들림 강도입니다.</param>
        /// <param name="frequency">흔들림 진동수입니다.</param>
        /// <param name="duration">흔들림 지속 시간(초)입니다.</param>
        public void Shake(float amplitude, float frequency, float duration)
        {
            if(perlin == null)
            {
                Debug.LogWarning("Shake 호출 시 Perlin 노이즈 컴포넌트가 null입니다.", this);
                return;
            }

            StopAllCoroutines(); // 이전 흔들림 중지
            StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
        }

        /// <summary>
        /// 노이즈 매개변수를 적용한 뒤, 지정된 시간 후에 값을 초기화하는 코루틴입니다.
        /// </summary>
        /// <param name="amplitude">적용할 노이즈 강도입니다.</param>
        /// <param name="frequency">적용할 노이즈 진동수입니다.</param>
        /// <param name="duration">초기화 전 대기 시간(초)입니다.</param>
        /// <returns>코루틴용 IEnumerator 객체를 반환합니다.</returns>
        private IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
        {
            perlin.AmplitudeGain = amplitude;
            perlin.FrequencyGain = frequency;

            yield return new WaitForSeconds(duration);

            perlin.AmplitudeGain = 0f;
            perlin.FrequencyGain = 0f;
        }
    }

}

