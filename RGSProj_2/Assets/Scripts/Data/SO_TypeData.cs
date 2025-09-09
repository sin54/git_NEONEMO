using UnityEngine;
[CreateAssetMenu(fileName = "TypeData", menuName = "Data/TypeData")]
public class SO_TypeData : ScriptableObject
{
    public string typeName;
    [TextArea]
    public string[] typeDesc;
    public Sprite typeIcon;
    public Color32 typeColors;

    [TextArea]
    public string[] passiveTxt;
    [TextArea]
    public string[] activeTxt;
}
