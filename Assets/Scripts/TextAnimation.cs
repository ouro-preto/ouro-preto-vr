using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextAnimation : MonoBehaviour
{
    // Store reference to target component.
    private TMP_Text m_Text;

    // Auxiliar member. Store animation timestamp
    // for each character on text.
    private List<float> timestamps;

    // Awake() runs before Start(). This script must
    // be ready before the ones who controls it.
    private void Awake()
    {
        // Get reference to TMPro component.
        m_Text = GetComponent<TMP_Text>();

        // Initialize list of timestamps.
        timestamps = new List<float>();
    }

    private void Update()
    {
        // Make sure TMPro component is up to date.
        m_Text.ForceMeshUpdate();

        // Keep reference to text info.
        var textInfo = m_Text.textInfo;

        // Loop though each character in text info.
        foreach (var charInfo in textInfo.characterInfo)
        {
            // Ignore invisible characters.
            if (!charInfo.isVisible)
                continue;

            // Get mesh info for current character.
            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

            // Loop through each vertex: BL, BR, TL, TR.
            for (int i = 0; i < 4; i++)
            {
                // Get current vertex index.
                var index = charInfo.vertexIndex + i;

                // Get current vertex position.
                var vertex = meshInfo.vertices[index];

                // TMPro keeps storing char info even after deletion.
                // This condition prevents from ArgumentOutOfRangeException.
                if (timestamps.Count <= charInfo.index)
                    break;

                // Here's where the magic happens! Feel free to change
                // parameters and create your own animations.
                var t = Mathf.Sin(Mathf.PI * Mathf.Min(4f * (Time.time - timestamps[charInfo.index]), 0.5f));

                // TODO the ideal approach is to manipulate "t" (code above) asynchronously,
                // on a coroutine. On Update() loop the effects only would be applied (below).

                // Update vertex position.
                meshInfo.vertices[index] = vertex + new Vector3(-1f * t, 0f, 0f);

                // Update vertex color.
                meshInfo.colors32[index].a = (byte) Mathf.Lerp(0, 255, t);
            }
        }

        // Loop through mesh info array.
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            // Get current mesh info.
            var meshInfo = textInfo.meshInfo[i];

            // Copy data from draft to working matrix.
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;

            // Finally apply changes made on current frame.
            m_Text.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    // LateUpdate() runs after all Update functions have been called.
    // It is useful to track objects that might have moved inside Update.
    // Here LateUpdate() is used to track main camera position, i.e.,
    // player's current position.
    private void LateUpdate()
    {
        // This piece of code grants that TMPro is always facing player.
        // Main Camera on SteamVR Debug mode is the one named "FallbackObjects".
        var relativePos = Camera.main.transform.position - transform.position;
        var eulerAngles = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
        eulerAngles.y += 180f;
        transform.eulerAngles = eulerAngles;
    }

    // Fill entire TMPro text content.
    public void Fill(string str)
    {
        m_Text.text = str;
        timestamps.Clear();
        while (timestamps.Count < str.Length)
            timestamps.Add(Time.time);
    }

    // Insert a char to the end of TMPro text content.
    public void Append(char chr)
    {
        m_Text.text += chr;
        timestamps.Add(Time.time);
    }

    // Clear entire TMPro text content.
    public void Clear()
    {
        m_Text.text = "";
        timestamps.Clear();
    }
}
