using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class SneakImage : MonoBehaviour
{
    [SerializeField] private BasicMovement basicMovement;

    private Image sneakVingette;
    private Color vingetteColor;

    [SerializeField] private float alphaCurrent;
    [SerializeField] private float alphaMax;

    [SerializeField] private float fadeTime;

    
    void Awake()
    {
        if(basicMovement == null) 
            basicMovement = BasicMovement.Instance;
        
        sneakVingette = GetComponent<Image>();
    }

    private void Start()
    {
        vingetteColor = sneakVingette.color;
    }

    // Update is called once per frame
    void Update()
    {
        bool crouching = basicMovement.CurrentState == BasicMovement.PlayerState.Crouch;
        
        if (crouching)
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, alphaMax, fadeTime);
        }
        else
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, 0, fadeTime);
        }

        vingetteColor.a = alphaCurrent;
        sneakVingette.color = vingetteColor;
    }
}

