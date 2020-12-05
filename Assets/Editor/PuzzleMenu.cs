using UnityEditor;
using UnityEngine;

public class PuzzleMenu : MonoBehaviour
{
    [MenuItem("Puzzles/Import JSON")]
    public static void Import()
    {
        Debug.Log("ok");
    }

    [MenuItem("Assets/Create/Puzzle")]
    public static void CreateMyAsset()
    {
        PuzzleScriptableObject asset = ScriptableObject.CreateInstance<PuzzleScriptableObject>();
        AssetDatabase.CreateAsset(asset, "Assets/Puzzle.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}
