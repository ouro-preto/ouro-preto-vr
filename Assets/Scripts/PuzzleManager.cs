using UnityEngine;

[DisallowMultipleComponent]
public class PuzzleManager : MonoBehaviour
{
    // Store reference to user interfaces (UIs)
    // managed by this script.
    [SerializeField] private TreasureInfo m_TreasureInfo = null;
    [SerializeField] private Menu m_Menu = null;
    [SerializeField] private GameObject m_Minimap = null;

    // A static instance is very useful to access the
    // script directly. No object reference is needed.
    public static PuzzleManager Instance { get; private set; }

    // Awake() is called before Start(). It is useful since
    // Instance should be initialized before any other
    // component to prevent NullReferenceException.
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        // Load entire treasures data from JSON file.
        var data = LoadJson("Data");

        // Start counting our zero-based id.
        var id = 0;

        // Iterate over each pre-defined treasure.
        foreach (var x in data.items)
        {
            // Load respective prefab.
            var prefab = Resources.Load(x.prefab) as GameObject;

            // Instantiate treasure at specified position.
            var instance = Instantiate(prefab, x.position, prefab.transform.rotation);

            // Instantiate the static minimap mark.
            Instantiate(m_Minimap, x.position, Quaternion.identity);

            // Pass treasure data to instantiated component.
            instance.GetComponent<Treasure>().Setup(id++, x.name, x.description);

            // Apply locked configuration parameter.
            if (x.locked) instance.AddComponent<LockToPoint>();

            // Load 2D sample picture.
            var sprite = Resources.Load<Sprite>(x.image);

            // Apply sample picture on treasures menu.
            m_Menu.CreateTreasure(sprite, x.name);
        }

        // Load saved data from previous game sessions.
        Save.LoadGame();

        // Update menu status accordingly found treasures.
        foreach (var index in Save.FoundTreasures)
            m_Menu.UpdateTreasure(index);
    }

    private void OnApplicationQuit()
    {
        // Make sure player achievements were
        // stored before quit.
        Save.SaveGame();
    }

    // Propagates onPickUp event.
    public void PickUp(Treasure treasure)
    {
        m_TreasureInfo.PickUp(treasure);
        m_Menu.UpdateTreasure(treasure.Id);
        Save.AddTreasure(treasure.Id);
    }

    // Propagates onDetachFromHand event.
    public void DetachFromHand() => m_TreasureInfo.DetachFromHand();

    // Deletes entire saved data.
    // WARNING: by now this is used for test purposes only.
    // TODO: implement double confirmation.
    public void DeleteAchievements()
    {
        // Hide all treasures on menu UI.
        foreach (var index in Save.FoundTreasures)
            m_Menu.UpdateTreasure(index, false);

        // Then clear saved data.
        Save.Reset();
    }

    // Load JSON file at filePath from Resources folder.
    private static Data LoadJson(string filePath)
    {
        // Load JSON file as a TextAsset.
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        // Then convert its content to an C# structured data object.
        return JsonUtility.FromJson<Data>(targetFile.text);
    }

    // Data structures below are used by JSON
    // serialization and deserialization.

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