using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(VelocityEstimator))]
public class Treasure : MonoBehaviour
{
    private Throwable m_Throwable;

    public string Name { get; private set; }

    public string Description { get; private set; }

    [SerializeField]
    private float m_SnapTime = 3f;

    private float m_Timestamp = 0f;

    private void Start()
    {
        m_Throwable = GetComponent<Throwable>();
        m_Throwable.onPickUp.AddListener(PickUp);
        m_Throwable.onDetachFromHand.AddListener(DetachFromHand);
    }

    private void PickUp()
    {
        if (m_Timestamp > 0f)
            return;

        PuzzleManager.Instance.PickUp(this);
        m_Timestamp = Time.time;
    }

    private void DetachFromHand()
    {
        StartCoroutine(DetachFromHandCoroutine());
    }
    
    private IEnumerator DetachFromHandCoroutine()
    {
        float deltaTime = Time.time - m_Timestamp;
        if (deltaTime < m_SnapTime)
            yield return new WaitForSeconds(m_SnapTime - deltaTime);
        PuzzleManager.Instance.DetachFromHand();
        m_Timestamp = 0f;
    }

    public void Setup(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
