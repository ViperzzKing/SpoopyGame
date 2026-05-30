using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ThrowInputOverlay : MonoBehaviour
{
    [SerializeField] private InspectObject inspectObject;

    private Image sneakVingette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(inspectObject == null)
            inspectObject = FindAnyObjectByType<InspectObject>();
        
        sneakVingette = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ThrowProgress() > 1)
        {
            sneakVingette.fillAmount = Mathf.Clamp01(ThrowProgress());
        }
    }

    public float ThrowProgress()
    {
        return inspectObject.TimePressingDropkey / inspectObject.HoldThrowTime;
    }

}
