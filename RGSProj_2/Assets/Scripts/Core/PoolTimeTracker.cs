using UnityEngine;

namespace Core
{
    /// <summary>
    /// 풀링된 객체가 마지막으로 비활성화된 시점을 기록합니다.
    /// 이 데이터를 이용해 재활용 시점 또는 비활성 대기 시간을 판단할 수 있습니다.
    /// </summary>
    public class PoolTimeTracker : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// 마지막으로 <c>OnDisable</c>이 호출된 시간(초).  
        /// 초기값을 음수로 두어 즉시 재활용을 방지합니다.
        /// </summary>
        public float lastDisabledTime = -10f;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 컴포넌트가 비활성화될 때 실행됩니다.
        /// 현재 <see cref="Time.time"/>을 <see cref="lastDisabledTime"/>에 기록합니다.
        /// </summary>
        private void OnDisable()
        {
            lastDisabledTime = Time.time;
        }

        #endregion
    }
}
