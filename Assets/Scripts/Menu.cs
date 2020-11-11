using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private float m_FixedHeight = 4f;

    [SerializeField]
    private float m_FixedDistance = 12f;

    private Transform m_Canvas; // TODO replace by animation
    private Transform m_Camera;
    private bool m_Enabled = false;

    void Start()
    {
        m_Canvas = transform.GetChild(0);
        m_Camera = Camera.main.transform;
    }

    void Update()
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
}
