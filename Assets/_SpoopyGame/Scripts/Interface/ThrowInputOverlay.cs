using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ThrowInputOverlay : MonoBehaviour
{
    [SerializeField] private InspectObject inspectObject;

    private Image throwOverlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(inspectObject == null)
            inspectObject = FindAnyObjectByType<InspectObject>();
        
        throwOverlay = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ThrowProgress() > 0)
        {
            throwOverlay.fillAmount = Mathf.Clamp01(ThrowProgress());
        }

        if (inspectObject.TimePressingDropkey < inspectObject.HoldThrowTime)
        {
            throwOverlay.fillAmount = 0;
        }
    }

    public float ThrowProgress()
    {
        return inspectObject.TimePressingDropkey / inspectObject.HoldThrowTime;
    }

}
