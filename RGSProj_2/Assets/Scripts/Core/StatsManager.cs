using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    #region Data Structures

    /// <summary>
    /// 단일 스탯의 이름과 기본값을 저장하는 데이터 클래스입니다.
    /// Inspector에서 초기 스탯 값을 설정할 때 사용합니다.
    /// </summary>
    [Serializable]
    public class StatData
    {
        /// <summary>스탯 식별용 이름 (예: "Health", "Attack").</summary>
        public string statName;

        /// <summary>해당 스탯의 기본값.</summary>
        public float baseValue;
    }

    /// <summary>
    /// 스탯에 적용되는 수정자(Modifier)를 정의합니다.
    /// 지속 시간, 가산(additive) 및 곱셈(multiplier) 효과를 가집니다.
    /// </summary>
    public class StatModifier
    {
        /// <summary>수정 대상 스탯 키 (예: "attack", "magic", "crit").</summary>
        public string type;

        /// <summary>기본값에 더해지는 값.</summary>
        public float additive;

        /// <summary>기본값 및 가산 값에 곱해지는 배율.</summary>
        public float multiplier;

        /// <summary>수정자가 유지되는 총 지속 시간(초). 0이면 무제한.</summary>
        public float duration;

        /// <summary>수정자 시작 시점의 타임스탬프.</summary>
        public float startTime;

        /// <summary>수정자 그룹 식별용 태그 (예: "buff", "debuff").</summary>
        public string tag;
    }

    #endregion

    /// <summary>
    /// 스탯의 기본값과 적용 중인 수정자를 관리하고,
    /// 최종 보정된 스탯 값을 계산하여 제공합니다.
    /// </summary>
    public class StatsManager : MonoBehaviour
    {
        #region Fileds


        [Header("초기 스탯 (Inspector 설정)")]
        /// <summary>Inspector에서 설정한 초기 스탯 리스트.</summary>
        public List<StatData> initialStats = new List<StatData>();

        /// <summary>스탯 이름 → 기본값 매핑 딕셔너리.</summary>
        private Dictionary<string, float> baseValues = new Dictionary<string, float>();

        /// <summary>현재 적용 중인 모든 스탯 수정자 목록.</summary>
        private List<StatModifier> modifiers = new List<StatModifier>();

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake 시 초기Stats를 baseValues 딕셔너리에 복사합니다.
        /// </summary>
        void Awake()
        {
            // 초기 스탯을 Dictionary로 변환
            foreach (var stat in initialStats)
            {
                baseValues[stat.statName] = stat.baseValue;
            }
        }

        /// <summary>
        /// 매 프레임마다 지속 시간이 만료된 수정자를 제거합니다.
        /// </summary>
        void Update()
        {
            float now = Time.time;
            modifiers.RemoveAll(mod => mod.duration > 0 && now - mod.startTime >= mod.duration);
        }

        #endregion

        #region Public API

        /// <summary>
        /// 스탯의 기본값을 새로 설정합니다. 존재하지 않으면 새로 추가합니다.
        /// </summary>
        /// <param name="type">스탯 키</param>
        /// <param name="value">새 기본값</param>
        public void SetBase(string type, float value)
        {
            baseValues[type] = value;
        }

        /// <summary>
        /// 새로운 스탯 수정자를 추가합니다.
        /// </summary>
        /// <param name="type">수정 대상 스탯 키</param>
        /// <param name="additive">가산 값</param>
        /// <param name="multiplier">곱셈 배율</param>
        /// <param name="duration">지속 시간(초), 0 이면 무제한</param>
        /// <param name="tag">수정자 구분용 태그</param>
        public void AddModifier(string type, float additive = 0f, float multiplier = 1f, float duration = 0f, string tag = null)
        {
            modifiers.Add(new StatModifier
            {
                type = type,
                additive = additive,
                multiplier = multiplier,
                duration = duration,
                startTime = Time.time,
                tag = tag
            });
        }

        /// <summary>
        /// 지정된 태그로 등록된 모든 수정자를 제거합니다.
        /// </summary>
        /// <param name="tag">제거할 수정자 태그</param>
        public void RemoveModifiersByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            modifiers.RemoveAll(mod => mod.tag == tag);
        }

        /// <summary>
        /// 해당 스탯에 적용 중인 모든 수정자를 적용한
        /// 최종 보정 스탯 값을 반환합니다.
        /// </summary>
        /// <param name="type">스탯 키</param>
        /// <returns>기본값에 가산 합계와 곱셈 배율을 적용한 결과</returns>
        public float GetFinalValue(string type)
        {
            float baseValue = baseValues.ContainsKey(type) ? baseValues[type] : 0f;
            float additiveSum = 0f;
            float multiplierProduct = 1f;

            foreach (var mod in modifiers)
            {
                if (mod.type != type) continue;
                additiveSum += mod.additive;
                multiplierProduct *= mod.multiplier;
            }

            return (baseValue + additiveSum) * multiplierProduct;
        }

        #endregion
    }
}

