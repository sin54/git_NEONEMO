/*
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSpeedController : MonoBehaviour
{
    public float baseSpeed = 2f;
    private List<SpeedModifier> modifiers = new List<SpeedModifier>();

    public float FinalSpeed
    {
        get
        {
            float additiveSum = 0f;
            float multiplierProduct = 1f;

            foreach (var mod in modifiers)
            {
                additiveSum += mod.additive;
                multiplierProduct *= mod.multiplier;
            }

            return (baseSpeed + additiveSum) * multiplierProduct;
        }
    }

    void Update()
    {
        float now = Time.time;
        modifiers.RemoveAll(mod => mod.duration > 0 && now - mod.startTime >= mod.duration);
    }

    public void AddModifier(float additive = 0f, float multiplier = 1f, float duration = 0f)
    {
        modifiers.Add(new SpeedModifier
        {
            additive = additive,
            multiplier = multiplier,
            duration = duration,
            startTime = Time.time
        });
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerSpeedController))]
public class SpeedControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerSpeedController controller = (PlayerSpeedController)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Final Speed", controller.FinalSpeed.ToString("F2"));
    }
}
#endif
*/