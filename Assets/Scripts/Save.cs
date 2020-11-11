using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    [SerializeField]
    private List<int> foundTreasures = new List<int>();

    public static List<int> FoundTreasures => instance.foundTreasures;

    public static void AddTreasure(int id)
    {
        if (!instance.foundTreasures.Contains(id))
            instance.foundTreasures.Add(id);
    }

    private static string MyPath => Path.Combine(Application.persistentDataPath, k_FileName);
    const string k_FileName = "game.save";
    private static Save instance = new Save();

    public static void SaveGame()
    {       
        Debug.Log(JsonUtility.ToJson(instance, true));
        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.Create(MyPath);
        binaryFormatter.Serialize(fileStream, instance);
        fileStream.Close();
    }

    public static void LoadGame()
    {
        if (File.Exists(MyPath))
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(MyPath, FileMode.Open);
            instance = binaryFormatter.Deserialize(fileStream) as Save;
            fileStream.Close();

            Debug.Log(JsonUtility.ToJson(instance, true));
        }
    }

    public static void Reset()
    {
        File.Delete(MyPath);
    }
}
