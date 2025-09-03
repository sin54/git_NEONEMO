using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatData
{
    public string statName;
    public float baseValue;
}
public class StatModifier
{
    public string type; // 예: "attack", "magic", "crit"
    public float additive;
    public float multiplier;
    public float duration;
    public float startTime;
    public string tag;
}
public class StatsManager : MonoBehaviour
{
    [Header("초기 스탯 (인스펙터에서 설정 가능)")]
    public List<StatData> initialStats = new List<StatData>();

    private Dictionary<string, float> baseValues = new Dictionary<string, float>();
    private List<StatModifier> modifiers = new List<StatModifier>();

    void Awake()
    {
        // 초기 스탯을 Dictionary로 변환
        foreach (var stat in initialStats)
        {
            baseValues[stat.statName] = stat.baseValue;
        }
    }

    void Update()
    {
        float now = Time.time;
        modifiers.RemoveAll(mod => mod.duration > 0 && now - mod.startTime >= mod.duration);
    }

    public void SetBase(string type, float value)
    {
        baseValues[type] = value;
    }

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
    public void RemoveModifiersByTag(string tag)
    {
        if (string.IsNullOrEmpty(tag)) return;

        modifiers.RemoveAll(mod => mod.tag == tag);
    }
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
}
