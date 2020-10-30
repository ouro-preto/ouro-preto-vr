using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    const string k_FolderName = "TreasureHunt";

    [Header("UI")]

    [SerializeField]
    private GameObject m_Panel;

    [SerializeField]
    private Text m_NameText;

    [SerializeField]
    private Text m_DescriptionText;

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
            prefab.AddComponent<Treasure>();

            var instance = Instantiate(prefab, x.position, Quaternion.identity);
            instance.GetComponent<Treasure>().Setup(x.name, x.description);
        }
    }

    public void PickUp(Treasure treasure)
    {
        m_NameText.text = treasure.Name;
        m_DescriptionText.text = treasure.Description;
        m_Panel.SetActive(true);
    }

    private static Data LoadJson(string filePath)
    {
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return JsonUtility.FromJson<Data>(targetFile.text);
    }

    [System.Serializable]
    private struct Item
    {
        public string prefab;
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