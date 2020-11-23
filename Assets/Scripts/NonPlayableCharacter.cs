using System.Collections;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Prefabs = null;

    [SerializeField, TextArea]
    private string[] m_Text = null;

    private TextMesh m_TextMesh = null;
    private Coroutine m_Coroutine = null;

    // Start is called before the first frame update
    private void Start()
    {
        var i = Random.Range(0, m_Prefabs.Length);
        Instantiate(m_Prefabs[i], transform.position, Quaternion.identity, transform);
        transform.LookAt(Vector3.zero);

        m_TextMesh = GetComponentInChildren<TextMesh>();
        m_TextMesh.text = m_Text.Length == 0 ? "" : m_Text[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Coroutine = StartCoroutine(Play());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(m_Coroutine);
        }
    }

    private IEnumerator Play()
    {
        for (int i = 1; i < m_Text.Length; i++)
        {
            m_TextMesh.text = m_Text[i];
            yield return new WaitForSeconds(2f);
        }
    }
}