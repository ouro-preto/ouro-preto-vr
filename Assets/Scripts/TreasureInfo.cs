using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TreasureInfo : MonoBehaviour
{
    // Target animator controls Canvas
    // opacity and enabled state.
    [SerializeField] private Animator m_Animator = null;

    // Store reference to UI Text which displays
    // current grabbed treasure title.
    [SerializeField] private Text m_Label = null;

    // Store reference to UI Text which displays
    // treasure description and curiosities.
    [SerializeField] private Text m_Content = null;

    // This function should be called right when
    // a treasure is picked up by Player.
    public void PickUp(Treasure treasure)
    {
        // Adjust Canvas UI content.
        m_Label.text = treasure.Name;
        m_Content.text = treasure.Description;

        // Adjust transform position to follow Treasure.
        transform.position = treasure.transform.position;

        // Adjust transform rotation to face Player.
        transform.LookAt(Camera.main.transform);

        // Finally show Canvas UI.
        m_Animator.SetBool("Visible", true);
    }

    // This function should be called right when
    // a treasure is dropped up by Player.
    public void DetachFromHand()
    {
        // Hide Canvas UI.
        m_Animator.SetBool("Visible", false);
    }
}
