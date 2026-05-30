using System;
using UnityEngine;
using System.Collections;

public class OutlineMesh : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material outlineMaterial;
    
    [Header("Outline")]
    [SerializeField] private bool outlineEnabled;
    [SerializeField] private float outlineScale;
    private static readonly int OutlineScaleID = Shader.PropertyToID("_Outline_Scale");

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    [ContextMenu("Toggle Outline")]
    public void ToggleOutline() 
    {
        SetOutline(!outlineEnabled);
    }
    
    private IEnumerator AnimateOutlineScale(float start, float end, float duration)
    {
        // put outline here
        Material outlineMat = outlineMaterial;
        
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
            rend.materials = new Material[] {baseMaterial};
        }
    }
    
    public void SetOutline(bool toggled)
    {
        outlineEnabled = toggled;
        if (outlineEnabled)
        {
            // Restore 2 materials: [Base, Outline]// Outline
            
            // Set initial scale before animation
            outlineMaterial.SetFloat(OutlineScaleID, 20f);
            rend.materials = new Material[] {baseMaterial, outlineMaterial};
            
            // Animate 50 -> 5
            StopCoroutine(AnimateOutlineScale(20, 5, 0.5f));
            StartCoroutine(AnimateOutlineScale(20f, 5f, 0.5f));
        }
        else
        {
            // Animate 5 -> 50 then remove outline
            StopCoroutine(AnimateOutlineScale(5f, 20f, 0.5f));
            StartCoroutine(AnimateOutlineScale(5f, 20f, 0.5f));
        }

    }
}