using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    // store id for each treasure found by player
    [SerializeField] private List<int> foundTreasures = new List<int>();

    // used for read purpose only
    public static List<int> FoundTreasures => instance.foundTreasures;

    // this method is called when player find a new treasure
    // or even just to load previously found treasures.
    public static void AddTreasure(int id)
    {
        if (!instance.foundTreasures.Contains(id))
            instance.foundTreasures.Add(id);
    }

    private static string MyPath => Path.Combine(Application.persistentDataPath, k_FileName);
    const string k_FileName = "game.save";
    private static Save instance = new Save();

    // write on save file
    public static void SaveGame()
    {       
        Debug.Log(JsonUtility.ToJson(instance, true));
        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.Create(MyPath);
        binaryFormatter.Serialize(fileStream, instance);
        fileStream.Close();
    }

    // read from save file
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

    // delete player achievements
    public static void Reset()
    {
        File.Delete(MyPath);
    }
}
