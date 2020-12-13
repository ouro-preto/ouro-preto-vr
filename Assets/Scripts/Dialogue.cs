using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

public class Dialogue : MonoBehaviour
{
    // Store a reference to target component.
    [SerializeField] private TextAnimation m_Animation = null;

    // You must set text content on Editor.
    [SerializeField, TextArea(5, 30)] private string m_Content = "";

    // Auxiliar members.
    private string[] m_Pages;
    private Coroutine m_Coroutine;

    // These patterns are useful to correctly comprehend text content.
    // So if a pageSeparator is found on editor input it must be
    // treat as a page break. In a similar way if a rowSeparator
    // is found on editor input it means a line break.
    private static readonly string pageSeparator = "\n\n";
    private static readonly string rowSeparator = "\n";

    private void Start()
    {
        // Split text content by pages.
        m_Pages = Regex.Split(m_Content, pageSeparator);

        // Set default text content.
        m_Animation.Fill(m_Pages[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player is closer enough...
        if (other.CompareTag("Player"))
        {
            // ...don't hesitate to start speaking with her/him!
            m_Coroutine = StartCoroutine(Speak());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Whoops! Player got away from you.
        if (other.CompareTag("Player"))
        {
            // Stop "speaking" coroutine.
            StopCoroutine(m_Coroutine);

            // Reset text content to default.
            m_Animation.Fill(m_Pages[0]);
        }
    }

    // This asynchronous task updates and
    // animates text content dinamically.
    private IEnumerator Speak()
    {
        // Loop through paginated content.
        foreach (var page in m_Pages)
        {
            // Split current page by rows.
            var rows = Regex.Split(page, rowSeparator);

            // Clear content before write new page.
            m_Animation.Clear();
            
            // Loop through each row.
            foreach (var row in rows)
            {
                // Loop though each character.
                foreach (var chr in row)
                {
                    yield return new WaitForEndOfFrame();

                    // Append character to text animation.
                    m_Animation.Append(chr);

                    // Delay between character appending.
                    yield return new WaitForSeconds(0.125f);
                }

                // Wait a moment before start to write a new row.
                yield return new WaitForSeconds(0.5f);

                // Finally append line break.
                m_Animation.Append('\n');
            }

            // TODO fade-out

            // Delay between page transition.
            yield return new WaitForSeconds(1.0f);
        }

        // Clear content so player will notice
        // dialogue have finished.
        m_Animation.Clear();
    }
}