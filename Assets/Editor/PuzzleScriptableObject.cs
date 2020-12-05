using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle", menuName = "Custom/Puzzle", order = 1)]
public class PuzzleScriptableObject : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}