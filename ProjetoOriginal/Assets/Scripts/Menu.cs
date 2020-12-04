using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Transform m_GridLayout = null;

    [SerializeField]
    private GameObject m_GridItem = null;

    [SerializeField]
    private float m_FixedHeight = 4f;

    [SerializeField]
    private float m_FixedDistance = 12f;

    private Transform m_Canvas; // TODO replace by animation
    private Transform m_Camera;
    private bool m_Enabled = false;

    private void Start()
    {
        m_Canvas = transform.GetChild(0);
        m_Camera = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_Enabled = !m_Enabled;
            m_Canvas.gameObject.SetActive(m_Enabled);

            // Set transform position.
            var position = m_Camera.position + m_FixedDistance * m_Camera.forward;
            position.y = m_FixedHeight;
            transform.position = position;

            // Then set rotation to look at camera.
            transform.LookAt(m_Camera);
        }
    }

    public void CreateTreasure(Sprite sprite, string label)
    {
        var instance = Instantiate(m_GridItem, m_GridLayout);
        var image = instance.GetComponentInChildren<Image>();
        var text = instance.GetComponentInChildren<Text>();

        image.color = Color.black;
        image.sprite = sprite;
        text.text = label;
    }

    public void UpdateTreasure(int id, bool found = true)
    {
        var image = m_GridLayout.GetChild(id).GetComponentInChildren<Image>();
        image.color = found ? Color.white : Color.black;
    }
}
