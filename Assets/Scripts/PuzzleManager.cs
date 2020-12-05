using UnityEngine;

[DisallowMultipleComponent]
public class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    private TreasureInfo m_Dialogue = null;

    [SerializeField]
    private Menu m_Menu = null;

    [SerializeField]
    private GameObject m_Minimap = null;

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
        var data = LoadJson("Data");
        var id = 0;

        foreach (var x in data.items)
        {
            var prefab = Resources.Load(x.prefab) as GameObject;
            var instance = Instantiate(prefab, x.position, prefab.transform.rotation);
            Instantiate(m_Minimap, x.position, Quaternion.identity);

            instance.GetComponent<Treasure>().Setup(id++, x.name, x.description);
            if (x.locked) instance.AddComponent<LockToPoint>();

            var sprite = Resources.Load<Sprite>(x.image);
            m_Menu.CreateTreasure(sprite, x.name);
        }

        Save.LoadGame();
        foreach (var index in Save.FoundTreasures)
            m_Menu.UpdateTreasure(index);
    }

    private void OnApplicationQuit()
    {
        Save.SaveGame();
    }

    public void PickUp(Treasure treasure)
    {
        m_Dialogue.PickUp(treasure);
        m_Menu.UpdateTreasure(treasure.Id);
        Save.AddTreasure(treasure.Id);
    }

    public void DetachFromHand() => m_Dialogue.DetachFromHand();

    public void DeleteAchievements()
    {
        Save.Reset();
        foreach (var index in Save.FoundTreasures)
            m_Menu.UpdateTreasure(index);
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