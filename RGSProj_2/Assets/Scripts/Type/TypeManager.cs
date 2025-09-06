using TMPro;
using UnityEngine;
using System.Collections;

namespace Type
{
    /// <summary>
    /// ������ ��ü ������ �� ���ھ� ���������� ǥ���ϴ� Ÿ���� �ؽ�Ʈ �Ŵ����Դϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class TypeManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>
        /// ����� TextMeshProUGUI ������Ʈ�Դϴ�.
        /// </summary>
        [Tooltip("����� TextMeshPro �ؽ�Ʈ ������Ʈ�� �Ҵ��ϼ���.")]
        public TextMeshProUGUI textUI;

        /// <summary>
        /// �� ���ھ� ǥ���� ��ü �����Դϴ�.
        /// </summary>
        [Tooltip("ǥ���� ��ü ������ �Է��ϼ���.")]
        public string fullText;

        /// <summary>
        /// ���� �� ǥ�� ����(��)�Դϴ�.
        /// </summary>
        [Tooltip("���� ������ ������ �ð��� �����ϼ���.")]
        public float delay = 0.15f;

        #endregion

        #region Public Methods

        /// <summary>
        /// ��ü ������ �� ���ھ� ���������� ǥ���ϴ� �ڷ�ƾ�� �����մϴ�.
        /// </summary>
        public void ShowText()
        {
            StartCoroutine(ShowTextLetterByLetter());
        }

        /// <summary>
        /// ǥ�õ� �ؽ�Ʈ�� ��� �����մϴ�.
        /// </summary>
        public void DeleteText()
        {
            textUI.text = string.Empty;
        }

        #endregion

        #region Private Coroutines

        /// <summary>
        /// ���� ������ �ؽ�Ʈ�� ǥ���ϸ� ������ �����̸�ŭ ����մϴ�.
        /// </summary>
        private IEnumerator ShowTextLetterByLetter()
        {
            textUI.text = string.Empty;
            foreach (char letter in fullText)
            {
                textUI.text += letter;
                yield return new WaitForSeconds(delay);
            }
        }

        #endregion
    }
}

