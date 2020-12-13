using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Every grid item is pushed to this grid layout.
    [SerializeField] private Transform m_GridLayout = null;

    // Prefab used by every grid item.
    [SerializeField] private GameObject m_GridItem = null;

    // Fixed height of Menu in the world space.
    [SerializeField] private float m_FixedHeight = 4f;

    // Fixed distance to Player in the world space.
    [SerializeField] private float m_FixedDistance = 12f;

    // Canvas UI, root object.
    private Transform m_Canvas; // TODO replace by fade animation.

    // Store reference to main camera (Player) transform.
    private Transform m_Camera;

    // This flag tells if menu is being displayed or not.
    private bool m_Enabled = false;

    private void Start()
    {
        // Get Canvas UI reference.
        m_Canvas = transform.GetChild(0);

        // Get reference to main camera which
        // represents Player transform.
        m_Camera = Camera.main.transform;
    }

    private void Update()
    {
        // ESC key is used for both open and close menu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Switch flag state.
            m_Enabled = !m_Enabled;

            // Set canvas active based on flag state.
            m_Canvas.gameObject.SetActive(m_Enabled);

            if (m_Enabled)
            {
                // Update transform position.
                var position = m_Camera.position + m_FixedDistance * m_Camera.forward;
                position.y = m_FixedHeight;
                transform.position = position;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Enabled)
        {
            // Update transform rotation to face Player.
            transform.LookAt(m_Camera);
        }
    }

    public void CreateTreasure(Sprite sprite, string label)
    {
        // Add a new treasure (grid item) to grid layout.
        var instance = Instantiate(m_GridItem, m_GridLayout);
        var image = instance.GetComponentInChildren<Image>();
        var text = instance.GetComponentInChildren<Text>();

        // Set treasure (Grid item) label and sprite.
        image.color = Color.black;
        image.sprite = sprite;
        text.text = label;
    }

    public void UpdateTreasure(int id, bool found = true)
    {
        // Update treasure image filtering based
        // whether treasure was found or not.
        var image = m_GridLayout.GetChild(id).GetComponentInChildren<Image>();
        image.color = found ? Color.white : Color.black;
    }
}