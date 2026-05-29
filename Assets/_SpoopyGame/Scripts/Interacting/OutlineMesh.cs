using UnityEngine;
using System.Collections;

public class OutlineMesh : MonoBehaviour
{
    public Material[] materials;
    private bool outlineEnabled;
    public float outlineScale;
    private static readonly int OutlineScaleID = Shader.PropertyToID("_Outline_Scale");
    
    [ContextMenu("Toggle Outline")]
    public bool ToggleOutline() 
    {
        var rend = GetComponentInChildren<Renderer>();
        outlineEnabled = !outlineEnabled;
    
        if (outlineEnabled)
        {
            // Restore 2 materials: [Base, Outline]
            Material[] full = new Material[2];
            full[0] = materials[0];  // Base
            full[1] = materials[1];  // Outline
            
            // Set initial scale before animation
            materials[1].SetFloat(OutlineScaleID, 20f);
            rend.materials = full;
            
            // Animate 50 -> 5
            StartCoroutine(AnimateOutlineScale(20f, 5f, 0.5f));
        }
        else
        {
            // Animate 5 -> 50 then remove outline
            StartCoroutine(AnimateOutlineScale(5f, 20f, 0.5f));
        }
    
        return outlineEnabled;
    }
    
    private IEnumerator AnimateOutlineScale(float start, float end, float duration)
    {
        //gets the renderer
        var rend = GetComponentInChildren<Renderer>();
        // put outline here
        Material outlineMat = materials[1];
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float time = elapsed / duration;
            outlineScale = Mathf.Lerp(start, end, time); // lerp from start to end by time
            outlineMat.SetFloat(OutlineScaleID, outlineScale); // sets float using shader graph
            yield return null;
        }
        outlineScale = end;
        
        // Remove outline material after animation
        if (!outlineEnabled)
        {
            Material[] baseOnly = new Material[1] { materials[0] };
            rend.materials = baseOnly;
        }
    }
}