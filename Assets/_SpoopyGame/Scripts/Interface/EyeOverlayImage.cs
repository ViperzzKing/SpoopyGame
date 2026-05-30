using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class EyeOverlayImage : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private Image imageDetectionOverlay;

    [SerializeField] private float alphaCurrent;
    [SerializeField] private float alphaMax;

    [SerializeField] private float fadeTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        imageDetectionOverlay = GetComponent<Image>();
        
        if(enemyAI == null)
            enemyAI = EnemyAI.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDetected())
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, alphaMax, fadeTime);
        }
        else
        {
            alphaCurrent = Mathf.Lerp(alphaCurrent, 0, fadeTime);
        }
        imageDetectionOverlay.color = new Color(imageDetectionOverlay.color.r, imageDetectionOverlay.color.b, imageDetectionOverlay.color.g, alphaCurrent);
    }

    public bool IsDetected()
    {
        bool debugKeybind = Input.GetKey(KeyCode.K);
        
        if (enemyAI.PlayerDetected || debugKeybind)
        {
            return true;
        }
        return false;
    }

}
