using UnityEngine;
using UnityEngine.SceneManagement;

public class RuneCheckmarks : MonoBehaviour
{
    public static RuneCheckmarks Instance;
    
    [Header("References")]
    public GameObject tutorialRunes;
    public GameObject gameRunes;

    [Header("Ending Settings")]
    [SerializeField] private int[] finishes = new int[4];
    [SerializeField] private float maxRunes = 5;
    [SerializeField] private bool gameStarted;

    [Header("Ending Scene ")]
    [SerializeField] private int endingCryptScene;
    [SerializeField] private int endingRitualScene;
    [SerializeField] private int endingMansionScene;

    public enum RuneEnding
    {
        Tutorial,
        Crypt,
        Ritual,
        Mansion
    }
    
    // only one RuneCheckmarks can exist
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //----------------------------------------------------\\

    // Adds runeAmount to a specific finish slot, but caps at 5
    public void ChangeFinish(int specificFinish, int runeAmount)
    {
        int runesCombined = finishes[(int)RuneEnding.Crypt] + finishes[(int)RuneEnding.Mansion] + finishes[(int)RuneEnding.Ritual];
        
        if (runesCombined >= maxRunes && runeAmount >= 1) return;
        finishes[specificFinish] += runeAmount;
    }

    // Returns the rune count for a given finish slot, or 0 if invalid
    public int CheckRunes(int specificFinish)
    {
        if (specificFinish < 0) return 0;
        return finishes[specificFinish];
    }

    // Called when a rune is placed - checks if any ending or game start condition is met
    [ContextMenu("Test Check")]
    public void EndingTrigger()
    {
        CheckEndingTriggers();
    }
    
    private void TutorialCompletion()
    {
        if (FindFirstObjectByType<Highlight>() != null)
        {
            FindFirstObjectByType<Highlight>().ForceClearHighlight();
        }
        Debug.Log("Started Game");
        gameStarted = true;
        tutorialRunes.SetActive(false);
        gameRunes.SetActive(true);
        ChangeFinish(0, -5); // Reset tutorial rune count
    }

    private void CheckEndingTriggers()
    {
        // 5 tutorial runes collected - switch to main game runes
        if (CheckRunes(0) == 5)
        {
            TutorialCompletion();
        }

        // Crypt ending triggered
        if (CheckRunes(1) == 5 && gameStarted)
        {
            Debug.Log("crypt ending");
            SceneManager.LoadScene(endingCryptScene);
        }

        // Ritual ending triggered
        if (CheckRunes(2) == 5)
        {
            Debug.Log("Finished Ritual Ending");
            SceneManager.LoadScene(endingRitualScene);
        }

        // Mansion ending triggered
        if (CheckRunes(3) == 5)
        {
            Debug.Log("Finished Mansion Ending");
            SceneManager.LoadScene(endingMansionScene);
        }
    }
}