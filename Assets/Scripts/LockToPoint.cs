using UnityEngine;
using Valve.VR.InteractionSystem;

// This script was extract from SteamVR examples.
// It locks the target game object on its start
// position and rotation. Besides it simulates
// an artificial gravitational field animation.

[DisallowMultipleComponent]
public class LockToPoint : MonoBehaviour
{
    private Vector3 m_SnapPosition;
    private Quaternion m_SnapRotation;
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private float m_SnapTime = 2;

    private float m_DropTimer;
    private Interactable m_Interactable;

    private void Start()
    {
        m_Interactable = GetComponent<Interactable>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SnapPosition = transform.position;
        m_SnapRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (m_Interactable == null)
            return;

        if (m_Interactable.attachedToHand)
        {
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.useGravity = false;
            m_DropTimer = -1;
        }
        else
        {
            m_DropTimer += Time.deltaTime / (m_SnapTime / 2);
            bool finished = m_DropTimer > 1;
            m_Rigidbody.isKinematic = finished;
            m_Rigidbody.useGravity = finished;

            if (finished)
            {
                transform.position = m_SnapPosition;
                transform.rotation = m_SnapRotation;
            }
            else
            {
                float t = Mathf.Pow(35, m_DropTimer);
                m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, Vector3.zero, Time.fixedDeltaTime * 4);
                transform.position = Vector3.Lerp(transform.position, m_SnapPosition, Time.fixedDeltaTime * t * 3);
                transform.rotation = Quaternion.Slerp(transform.rotation, m_SnapRotation, Time.fixedDeltaTime * t * 2);
            }
        }
    }
}