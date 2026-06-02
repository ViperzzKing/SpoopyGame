using System.Collections;
using UnityEngine;

public class OutlineMesh : MonoBehaviour
{
    [Header("Materials")] [SerializeField] private Material baseMaterial;
    [SerializeField] private Material outlineMaterial;
    [Header("Outline")] [SerializeField] private bool outlineEnabled;
    [SerializeField] private float outlineScale;
    private static readonly int OutlineScaleID = Shader.PropertyToID("_Outline_Scale");
    private Renderer rend;
    private Coroutine outlineRoutine;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    private void OnDisable()
    {
        if (outlineRoutine != null)
        {
            StopCoroutine(outlineRoutine);
            outlineRoutine = null;
        }

        outlineEnabled = false;
    }

    [ContextMenu("Toggle Outline")]
    public void ToggleOutline()
    {
        SetOutline(!outlineEnabled);
    }

    public void SetOutline(bool toggled)
    {
        if (rend == null || baseMaterial == null || outlineMaterial == null)
        {
            Debug.LogWarning($"{name}: OutlineMesh is missing a Renderer, Base Material, or Outline Material."); 
            return;
        }

        if (outlineEnabled == toggled)
            return;
        outlineEnabled = toggled;
        if (outlineRoutine != null)
            StopCoroutine(outlineRoutine);
        if (outlineEnabled)
        {
            outlineMaterial.SetFloat(OutlineScaleID, 20f);
            rend.materials = new Material[] { baseMaterial, outlineMaterial };
            outlineRoutine = StartCoroutine(AnimateOutlineScale(20f, 5f, 0.5f));
        }
        else
        {
            outlineRoutine = StartCoroutine(AnimateOutlineScale(5f, 20f, 0.5f));
        }
    }

    private IEnumerator AnimateOutlineScale(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float time = elapsed / duration;
            outlineScale = Mathf.Lerp(start, end, time);
            outlineMaterial.SetFloat(OutlineScaleID, outlineScale);
            yield return null;
        }

        outlineScale = end;
        outlineMaterial.SetFloat(OutlineScaleID, outlineScale);
        if (!outlineEnabled)
        {
            rend.materials = new Material[] { baseMaterial };
        }

        outlineRoutine = null;
    }
}