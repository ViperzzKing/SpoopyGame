using UnityEngine;
using UnityEngine.UI;
public class EyeOverlayImage : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private Image imageDetectionOverlay;

    public float alphaCurrent;
    public float alphaMax;

    public float fadeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageDetectionOverlay = GetComponent<Image>();
        enemyAI = FindFirstObjectByType<EnemyAI>();
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
        imageDetectionOverlay.color = new Color(imageDetectionOverlay.color.r, imageDetectionOverlay.color.b, imageDetectionOverlay.color.g, alphaCurrent);
    }

    public bool isCrouching()
    {
        if (enemyAI.playerDetected || Input.GetKey(KeyCode.K))
        {
            return true;
        }
        return false;
    }

}
