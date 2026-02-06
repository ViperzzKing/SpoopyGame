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
            Debug.Log("triggered");
            StartCoroutine(FadeLight());
        }
    }
    
    IEnumerator FadeLight()
    {
        while (light.intensity > 1f)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, 1f, 600f * Time.deltaTime);
            yield return null;
        }
        light.intensity = 0f;
        fire.SetActive(false);
    }
}