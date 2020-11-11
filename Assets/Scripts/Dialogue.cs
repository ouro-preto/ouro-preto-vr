using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator = null;

    [SerializeField]
    private Text m_Label = null;

    [SerializeField]
    private Text m_Content = null;

    public void PickUp(Treasure treasure)
    {
        // Adjust canvas UI content.
        m_Label.text = treasure.Name;
        m_Content.text = treasure.Description;
        m_Animator.SetBool("Visible", true);

        // Adjust transform position and rotation.
        transform.position = treasure.transform.position;
        transform.LookAt(Camera.main.transform);
    }

    public void DetachFromHand()
    {
        m_Animator.SetBool("Visible", false);
    }
}
