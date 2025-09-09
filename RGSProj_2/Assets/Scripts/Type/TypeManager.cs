using TMPro;
using UnityEngine;
using System.Collections;

namespace Type
{
    /// <summary>
    /// 지정된 전체 문장을 한 글자씩 순차적으로 표시하는 타이핑 텍스트 매니저입니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class TypeManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>
        /// 출력할 TextMeshProUGUI 컴포넌트입니다.
        /// </summary>
        [Tooltip("출력할 TextMeshPro 텍스트 컴포넌트를 할당하세요.")]
        public TextMeshProUGUI textUI;

        /// <summary>
        /// 한 글자씩 표시할 전체 문장입니다.
        /// </summary>
        [Tooltip("표시할 전체 문장을 입력하세요.")]
        public string fullText;

        /// <summary>
        /// 글자 간 표시 간격(초)입니다.
        /// </summary>
        [Tooltip("글자 사이의 딜레이 시간을 설정하세요.")]
        public float delay = 0.15f;

        #endregion

        #region Public Methods

        /// <summary>
        /// 전체 문장을 한 글자씩 순차적으로 표시하는 코루틴을 시작합니다.
        /// </summary>
        public void ShowText()
        {
            StartCoroutine(ShowTextLetterByLetter());
        }

        /// <summary>
        /// 표시된 텍스트를 모두 삭제합니다.
        /// </summary>
        public void DeleteText()
        {
            textUI.text = string.Empty;
        }

        #endregion

        #region Private Coroutines

        /// <summary>
        /// 글자 단위로 텍스트를 표시하며 지정된 딜레이만큼 대기합니다.
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

