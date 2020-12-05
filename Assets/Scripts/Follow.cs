using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target = null;

    [SerializeField]
    private float m_FixedHeight = 30f;

    private void FixedUpdate()
    {
        var position = m_Target.position;
        position.y = m_FixedHeight;
        transform.position = position;
    }
}
