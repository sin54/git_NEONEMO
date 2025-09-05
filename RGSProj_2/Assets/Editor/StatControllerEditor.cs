#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Core;

[CustomEditor(typeof(StatsManager))]
public class StatControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Final Value가 실시간으로 바뀌도록 인스펙터 새로고침
        if (Application.isPlaying)
            Repaint();  // 런타임일 때만 새로고침

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("현재 스탯 값 (Final Value)", EditorStyles.boldLabel);

        StatsManager controller = (StatsManager)target;

        if (controller.initialStats != null)
        {
            foreach (var stat in controller.initialStats)
            {
                string statName = stat.statName;
                float finalValue = controller.GetFinalValue(statName);
                EditorGUILayout.LabelField($"{statName}", finalValue.ToString("F2"));
            }
        }
    }
}
#endif
