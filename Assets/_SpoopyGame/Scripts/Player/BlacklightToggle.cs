using UnityEngine;

public class BlacklightToggle : MonoBehaviour
{
    // Made by oscar not me
    //TODO -- oscar comment this
    
    [SerializeField] private GameObject blacklight;
    [SerializeField] private GameObject flashlight;

    private bool blToggle;
    private bool flashToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //inverts the active state if the key is pressed.
        if (Input.GetKeyDown(KeyCode.B))
        {
            blToggle = !blToggle;
            blacklight.SetActive(blToggle);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashToggle = !flashToggle;
            flashlight.SetActive(flashToggle);
        }
    }
}
