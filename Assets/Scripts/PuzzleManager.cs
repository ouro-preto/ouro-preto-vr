using UnityEngine;

[DisallowMultipleComponent]
public class PuzzleManager : MonoBehaviour
{
    const string k_FolderName = "TreasureHunt";

    [SerializeField]
    private Dialogue m_Dialogue;

    [SerializeField]
    private Menu m_Menu;

    [SerializeField]
    private GameObject m_Minimap;

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
        var data = LoadJson(Path("Data"));
        Debug.Log(JsonUtility.ToJson(data, true));

        foreach (var x in data.items)
        {
            var prefab = Resources.Load(Path(x.prefab)) as GameObject;
            var instance = Instantiate(prefab, x.position, Quaternion.identity);
            Instantiate(m_Minimap, x.position, Quaternion.identity);

            instance.GetComponent<Treasure>().Setup(x.name, x.description);
            if (x.locked) instance.AddComponent<LockToPoint>();

            var sprite = Resources.Load<Sprite>(Path(x.image));
            m_Menu.AddItem(sprite, x.name);
        }
    }

    public void PickUp(Treasure treasure) => m_Dialogue.PickUp(treasure);

    public void DetachFromHand() => m_Dialogue.DetachFromHand();

    private static string Path(string path) => $"{k_FolderName}/{path}";

    private static Data LoadJson(string filePath)
    {
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return JsonUtility.FromJson<Data>(targetFile.text);
    }

    [System.Serializable]
    private struct Item
    {
        public string prefab;
        public string image;
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