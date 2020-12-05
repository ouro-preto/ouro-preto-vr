using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class TextAnimation : MonoBehaviour
{
    // Store reference to target component.
    private TMP_Text textComponent;

    private List<float> timestamps;

    private void Start()
    {
        // Get reference to TMPro component.
        textComponent = GetComponent<TMP_Text>();

        // Initialize list of positions.
        timestamps = new List<float>();
    }

    private void Update()
    {
        // Make sure TMPro component is up to date.
        textComponent.ForceMeshUpdate();

        // Keep reference to text info.
        var textInfo = textComponent.textInfo;

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

                var t = Mathf.Sin(Mathf.PI * Mathf.Min(4f * (Time.time - timestamps[charInfo.index]), 0.5f));

                // TODO o mais adequado é manipular "t" em uma coroutine.
                // No loop update, portanto, apenas os efeitos seriam aplicados.

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
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private void LateUpdate()
    {
        var relativePos = Camera.main.transform.position - transform.position;
        var eulerAngles = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
        eulerAngles.y += 180f;
        transform.eulerAngles = eulerAngles;
    }

    // Fill entire TMPro text content.
    public void Fill(string str)
    {
        textComponent.text = str;
        timestamps.Clear();
        while (timestamps.Count < str.Length)
            timestamps.Add(Time.time);
    }

    // Insert a char to the end of TMPro text content.
    public void Append(char chr)
    {
        textComponent.text += chr;
        timestamps.Add(Time.time);
    }

    // Clear entire TMPro text content.
    public void Clear()
    {
        textComponent.text = "";
        timestamps.Clear();
    }
}
