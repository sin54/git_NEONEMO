using UnityEngine;

namespace Core
{
    /// <summary>
    /// Ǯ���� ��ü�� ���������� ��Ȱ��ȭ�� ������ ����մϴ�.
    /// �� �����͸� �̿��� ��Ȱ�� ���� �Ǵ� ��Ȱ�� ��� �ð��� �Ǵ��� �� �ֽ��ϴ�.
    /// </summary>
    public class PoolTimeTracker : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// ���������� <c>OnDisable</c>�� ȣ��� �ð�(��).  
        /// �ʱⰪ�� ������ �ξ� ��� ��Ȱ���� �����մϴ�.
        /// </summary>
        public float lastDisabledTime = -10f;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// ������Ʈ�� ��Ȱ��ȭ�� �� ����˴ϴ�.
        /// ���� <see cref="Time.time"/>�� <see cref="lastDisabledTime"/>�� ����մϴ�.
        /// </summary>
        private void OnDisable()
        {
            lastDisabledTime = Time.time;
        }

        #endregion
    }
}
