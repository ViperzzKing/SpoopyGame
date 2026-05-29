using UnityEngine;
using UnityEngine.UI;
public class SneakImage : MonoBehaviour
{
    // Made by oscar not me
    //TODO -- oscar comment this
    
    [SerializeField] private BasicMovement basicMovement;

    private Image sneakVingette;

    public float alphaCurrent;
    public float alphaMax;

    public float fadeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basicMovement = FindAnyObjectByType<BasicMovement>();
        sneakVingette = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrouching())
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, alphaMax, fadeTime);
        }
        else
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, 0, fadeTime);
        }
        sneakVingette.color = new Color(sneakVingette.color.r, sneakVingette.color.b, sneakVingette.color.g, alphaCurrent);
    }

    public bool isCrouching()
    {
        if (basicMovement.currentPlayerState == BasicMovement.State.Crouch)
        {
            return true;
        }
        return false;
    }

}

