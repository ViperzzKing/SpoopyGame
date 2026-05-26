using System;
using System.Collections;
using UnityEngine;

public class AffectLights : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] public GameObject fire;
    private bool lightActive = true;

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if(other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("triggered");
            StartCoroutine(FadeLightOff());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if(other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("exited");
            StartCoroutine(FadeLightOn());
        }
    }

    IEnumerator FadeLightOff()
    {
        while (light.intensity > 1f)
        {
            light.intensity = Mathf.Clamp(Mathf.MoveTowards(light.intensity, 1f, 600f * Time.deltaTime), 0f, 5000f);
            yield return null;
        }
        light.intensity = 0f;
        fire.SetActive(false);
    }

    IEnumerator FadeLightOn()
    {
        while (light.intensity < 395f)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, 397, 600f * Time.deltaTime);
            yield return null;
        }
        light.intensity = 397.887377182f;
        fire.SetActive(true);
    }
}