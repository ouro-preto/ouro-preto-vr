using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private Text m_Label;

    [SerializeField]
    private Text m_Content;

    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void PickUp(Treasure treasure)
    {
        m_Label.text = treasure.Name;
        m_Content.text = treasure.Description;
        m_Animator.SetBool("Visible", true);
    }

    public void DetachFromHand()
    {
        m_Animator.SetBool("Visible", false);
    }
}
