using System;
using System.Collections;
using UnityEngine;

public class AffectLights : MonoBehaviour
{
    
    [SerializeField] private Light light;
    
    // Fire Particle
    [SerializeField] public GameObject fire;
    private bool lightActive = true;

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Light has a trigger sphere collider
        // when "Enemy" walks in it FadeLightOff()
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if(other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("triggered");
            StartCoroutine(FadeLightOff());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // when "Enemy" walks out of light FadeLightOn()
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if(other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("exited");
            StartCoroutine(FadeLightOn());
        }
    }

    IEnumerator FadeLightOff()
    {
        // Only does this when the light has intensity
        while (light.intensity > 1f)
        {
            light.intensity = Mathf.Clamp(Mathf.MoveTowards(light.intensity, 1f, 600f * Time.deltaTime), 0f, 5000f);
            yield return null;
        }
        // when light reaches 1f set to 0f
        light.intensity = 0f;
        
        // turn of fire particle
        fire.SetActive(false);
    }

    IEnumerator FadeLightOn()
    {
        // Only does this when the light has lower intensity then 395 or less
        while (light.intensity < 395f)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, 397, 600f * Time.deltaTime);
            yield return null;
        }
        
        // very specific cause its weird in the inspector this is actually 5000
        light.intensity = 397.887377182f;
        fire.SetActive(true);
    }
}