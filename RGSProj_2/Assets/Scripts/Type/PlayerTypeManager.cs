using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Type
{
    /// <summary>
    /// 플레이어의 타입 전환을 관리하고,
    /// SpriteRenderer 및 ParticleSystem 색상을 갱신합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerTypeManager : MonoBehaviour
    {
        #region Fields

        /// <summary>현재 활성화된 BaseType</summary>
        public BaseType BT;

        /// <summary>사용 가능한 타입 코드 목록</summary>
        public List<int> NowType = new List<int>();

        /// <summary>타입 코드에 대응하는 BaseType 리스트</summary>
        public List<BaseType> Types = new List<BaseType>();

        /// <summary>타입별 색상 배열 (길이 4)</summary>
        public Color[] Colors = new Color[4];

        /// <summary>타입별 그라디언트 배열 (길이 4)</summary>
        public Gradient[] Gradients = new Gradient[4];

        /// <summary>마지막 타입 변경 시각 (초 단위)</summary>
        private float lastChangeTime;

        /// <summary>타입 변경 쿨타임 (초)</summary>
        [SerializeField] private float changeCool;

        /// <summary>색상 변경에 사용할 SpriteRenderer</summary>
        [SerializeField] private SpriteRenderer SR;

        /// <summary>색상 변경에 사용할 ParticleSystem</summary>
        [SerializeField] private ParticleSystem PS;

        /// <summary>현재 선택된 타입 인덱스</summary>
        public int idx = 0;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// MonoBehaviour 시작 시 호출됩니다.
        /// 초기화 로직이 필요할 경우 여기에 구현하세요.
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 매 프레임 타입 전환 키(A/D) 입력을 감지하여
        /// 지정된 쿨타임 이후에 <see cref="ChangeColor"/>를 호출합니다.
        /// </summary>
        private void Update()
        {

            if (Time.time>lastChangeTime+changeCool) {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    idx = (idx - 1 + NowType.Count) % NowType.Count;
                    ChangeColor();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    idx = (idx + 1) % NowType.Count;
                    ChangeColor();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 새로운 타입 코드를 등록합니다.
        /// </summary>
        /// <param name="typeCode">추가할 타입 코드</param>
        public void AddType(int typeCode)
        {
            NowType.Add(typeCode);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 현재 선택된 타입 인덱스에 대응하는 색상을
        /// SpriteRenderer 및 ParticleSystem에 적용하고
        /// 마지막 변경 시각을 갱신합니다.
        /// </summary>
        private void ChangeColor()
        {
            lastChangeTime = Time.time;
            BT = Types[NowType[idx]];

            // SpriteRenderer 색상 갱신
            SR.color = Colors[BT.typeCode];

            // ParticleSystem 그라디언트 갱신
            var colorOverLifetime = PS.colorOverLifetime;
            colorOverLifetime.color = Gradients[BT.typeCode];
        }

        #endregion
    }
}