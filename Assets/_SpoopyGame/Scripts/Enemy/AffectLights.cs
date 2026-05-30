using System;
using System.Collections;
using UnityEngine;

public class AffectLights : MonoBehaviour
{
    [Header("Light Settings")] 
    [SerializeField] private float normalIntensity = 397.887377182f; // 5000
    [SerializeField] private float dimIntensity = 1;
    [SerializeField] private float fadeTime = 600;
    [SerializeField] private bool lightActive = true;

    [Header("Refrences")] 
    [SerializeField] public GameObject targetFirePrefab;
    [SerializeField] private Light targetLight;
    [SerializeField] private int enemyLayer;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        targetLight = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        if (targetLight == null)
        {
            Debug.LogError("Missing Reference To Light");
        }

        if (targetFirePrefab == null)
        {
            //Debug.LogError("Missing Reference To Fire");
            //TODO -- even though its referenced it still throws error
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Light has a trigger sphere collider
        // when "Enemy" walks in it FadeLightOff()
        if (other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("triggered");
            StartCoroutine(FadeLight(lightActive));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // when "Enemy" walks out of light FadeLightOn()
        if (other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("exited");
            StopCoroutine(FadeLight(lightActive));
            StartCoroutine(FadeLight(lightActive));
        }
    }

    IEnumerator FadeLight(bool activeLight)
    {
        if (activeLight)
        {
            // Only does this when the light has intensity
            while (targetLight.intensity > dimIntensity)
            {
                targetLight.intensity =
                    Mathf.Clamp(Mathf.MoveTowards(targetLight.intensity, 
                            dimIntensity, fadeTime * Time.deltaTime), 0f, 5000f);
                yield return null;
            }

            // when light reaches 1f set to 0f
            targetLight.intensity = dimIntensity;

            // turn of fire particle
            targetFirePrefab.SetActive(false);
            lightActive = false;
        }

        if (!activeLight)
        {
            // Only does this when the light has lower intensity then 395 or less
            while (targetLight.intensity < normalIntensity)
            {
                targetLight.intensity =
                    Mathf.MoveTowards(targetLight.intensity, 
                        normalIntensity, fadeTime * Time.deltaTime);
                yield return null;
            }

            // very specific cause its weird in the inspector this is actually 5000
            targetLight.intensity = normalIntensity;
            targetFirePrefab.SetActive(true);
            lightActive = true;
        }
    }
}