using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// 전체 코드에서 사용 가능한 유틸 함수
    /// </summary>
    public static class UtilClass
    {
        public static bool GetPercent(float probability)
        {
            if (probability < 0f)
            {
                return false;
            }
            if (probability > 1f)
            {
                return true;
            }

            float randomValue = UnityEngine.Random.value;
            return randomValue <= probability;
        }
        public static int[] GenerateShuffledArray(int x)
        {
            System.Random rand = new System.Random();
            return Enumerable.Range(0, x + 1)
                             .OrderBy(_ => rand.Next())
                             .ToArray();
        }
        public static Vector2 RotateVector2(Vector2 original, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            float x = original.x * cos - original.y * sin;
            float y = original.x * sin + original.y * cos;

            return new Vector2(x, y);
        }
        public static string ToLocalizedString(this ItemRarity rarity)
        {
            FieldInfo field = rarity.GetType().GetField(rarity.ToString());
            var attr = field.GetCustomAttribute<InspectorNameAttribute>();
            return attr != null ? attr.displayName : rarity.ToString();
        }
        public static string ToLocalizedString(this ItemTag tag)
        {
            if (tag == 0) return string.Empty;

            var parts = new List<string>();
            foreach (ItemTag value in Enum.GetValues(typeof(ItemTag)))
            {
                if (tag.HasFlag(value))
                {
                    var field = typeof(ItemTag).GetField(value.ToString());
                    var attr = field.GetCustomAttribute<InspectorNameAttribute>();
                    string display = attr != null ? attr.displayName : value.ToString();
                    parts.Add($"{{{display}}}");
                }
            }

            return string.Join(" ", parts);
        }
        public static Gradient ChangeToGrad(Color color)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(color, 0f),
                    new GradientColorKey(color, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(color.a, 0f),
                    new GradientAlphaKey(color.a, 1f)
                }
            );
            return gradient;
        }
    }
}