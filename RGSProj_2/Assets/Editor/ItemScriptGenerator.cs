using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemScriptGenerator
{
    [MenuItem("Assets/Create/Item C# Script", false, 80)]
    public static void CreateCustomScript()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "New Script",
            "NewCustomScript.cs",
            "cs",
            "Enter a file name"
        );

        if (string.IsNullOrEmpty(path)) return;

        string scriptContent =
@"using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = ""#SCRIPTNAME#"", menuName = ""Inventory/SkillEvent/F/#SCRIPTNAME#"")]
public class #SCRIPTNAME# : InventoryItemSkillEvent
{
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
    }
}";
        string className = Path.GetFileNameWithoutExtension(path);
        scriptContent = scriptContent.Replace("#SCRIPTNAME#", className);

        File.WriteAllText(path, scriptContent);
        AssetDatabase.Refresh();
    }
}
