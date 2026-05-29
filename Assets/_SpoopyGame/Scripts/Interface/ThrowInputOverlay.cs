using UnityEngine;
using UnityEngine.UI;

public class ThrowInputOverlay : MonoBehaviour
{
    // Made by oscar not me
    //TODO -- oscar comment this
    
    [SerializeField] private InspectObject inspectObject;

    private Image sneakVingette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inspectObject = FindAnyObjectByType<InspectObject>();
        sneakVingette = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (throwProgress() > 1)
        {
            sneakVingette.fillAmount = 1;
        }
        else
        {
            sneakVingette.fillAmount = throwProgress();
        }
    }

    public float throwProgress()
    {
        return inspectObject.timePressingDropkey;
    }

}
