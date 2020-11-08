﻿using UnityEngine;

[DisallowMultipleComponent]
public class PuzzleManager : MonoBehaviour
{
    const string k_FolderName = "TreasureHunt";

    [SerializeField]
    private Dialogue m_Dialogue;

    public static PuzzleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        var data = LoadJson($"{k_FolderName}/Data");
        Debug.Log(JsonUtility.ToJson(data, true));

        foreach (var x in data.items)
        {
            var prefab = Resources.Load($"{k_FolderName}/{x.prefab}") as GameObject;
            var instance = Instantiate(prefab, x.position, Quaternion.identity);

            instance.GetComponent<Treasure>().Setup(x.name, x.description);
            if (x.locked) instance.AddComponent<LockToPoint>();
        }
    }

    public void PickUp(Treasure treasure) => m_Dialogue.PickUp(treasure);

    public void DetachFromHand() => m_Dialogue.DetachFromHand();

    private static Data LoadJson(string filePath)
    {
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return JsonUtility.FromJson<Data>(targetFile.text);
    }

    [System.Serializable]
    private struct Item
    {
        public string prefab;
        public bool locked;
        public Vector3 position;
        public string name;
        public string description;
    }

    [System.Serializable]
    private struct Data
    {
        public Item[] items;
        public string[] prefabs;
    }
}