using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

// Both Throwable and VelocityEstimator are added
// automatically on Editor with Treasure component.

[DisallowMultipleComponent]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(VelocityEstimator))]
public class Treasure : MonoBehaviour
{
    // These properties store basic data and
    // information about treasure.
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    // The minimum display time of TreasureInfo menu.
    [SerializeField] private float m_SnapTime = 3f;

    // Auxiliary member used to evaluate display time.
    private float m_Timestamp = 0f;

    private void Start()
    {
        // Add events to Throwable component.
        var throwable = GetComponent<Throwable>();
        throwable.onPickUp.AddListener(PickUp);
        throwable.onDetachFromHand.AddListener(DetachFromHand);
    }

    // Handle onPickUp event.
    private void PickUp()
    {
        // Player is not allowed to grab any treasure by now.
        // He/she should try again later.
        if (m_Timestamp > 0f)
            return;

        // Propagates onPickUp event.
        PuzzleManager.Instance.PickUp(this);

        // Capture current timestamp.
        m_Timestamp = Time.time;
    }

    // Handle onDetachFromHand event.
    private void DetachFromHand()
    {
        StartCoroutine(DetachFromHandCoroutine());
    }
    
    private IEnumerator DetachFromHandCoroutine()
    {
        // If player drops a treasure before snap time,
        // we will have to wait a little before
        // hide TreasureInfo menu and so on.
        float deltaTime = Time.time - m_Timestamp;
        if (deltaTime < m_SnapTime)
            yield return new WaitForSeconds(m_SnapTime - deltaTime);

        // Propagates onDetachFromHand event.
        PuzzleManager.Instance.DetachFromHand();

        // Reset timestamp.
        m_Timestamp = 0f;
    }

    // Just initialize private properties.
    public void Setup(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
