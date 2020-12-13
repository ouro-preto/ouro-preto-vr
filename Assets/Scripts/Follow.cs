using UnityEngine;

public class Follow : MonoBehaviour
{
    // Target object which we are going to follow.
    [SerializeField] private Transform m_Target = null;

    // Tranform position is always frozen at
    // certain height (Y component).
    [SerializeField] private float m_FixedHeight = 30f;

    // FixedUpdate() runs after Update() and before LateUpdate().
    // It is reserved for physical evaluations.
    private void FixedUpdate()
    {
        var position = m_Target.position;
        position.y = m_FixedHeight;
        transform.position = position;
    }
}
