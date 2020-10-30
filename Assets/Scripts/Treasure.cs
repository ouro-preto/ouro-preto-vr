using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(VelocityEstimator))]
public class Treasure : MonoBehaviour
{
    private Throwable m_Throwable;

    public string Name { get; private set; }

    public string Description { get; private set; }

    private void Start()
    {
        m_Throwable = GetComponent<Throwable>();
        m_Throwable.onPickUp.AddListener(PickUp);
    }

    private void PickUp()
    {
        PuzzleManager.Instance.PickUp(this);
    }

    public void Setup(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
