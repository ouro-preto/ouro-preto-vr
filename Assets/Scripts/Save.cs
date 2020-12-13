using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    // Store zero-based id for each treasure found by player.
    [SerializeField] private List<int> foundTreasures = new List<int>();

    // Property used for read purposes only.
    public static List<int> FoundTreasures => instance.foundTreasures;

    // This method should be called when player find a new treasure.
    public static void AddTreasure(int id)
    {
        if (!instance.foundTreasures.Contains(id))
            instance.foundTreasures.Add(id);
    }

    // Auxiliar constants and members.
    private static string MyPath => Path.Combine(Application.persistentDataPath, k_FileName);
    const string k_FileName = "game.save";

    // An instance is necessary to safely store and
    // manipulate array of found treasures, making
    // sure everything is private.
    private static Save instance = new Save();

    // Write on .save file.
    public static void SaveGame()
    {       
        var binaryFormatter = new BinaryFormatter();
        var fileStream = File.Create(MyPath);
        binaryFormatter.Serialize(fileStream, instance);
        fileStream.Close();
    }

    // Read from .save file.
    public static void LoadGame()
    {
        if (File.Exists(MyPath))
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(MyPath, FileMode.Open);
            instance = binaryFormatter.Deserialize(fileStream) as Save;
            fileStream.Close();
        }
    }

    // Delete player achievements.
    public static void Reset()
    {
        File.Delete(MyPath);
        instance.foundTreasures.Clear();
    }
}
