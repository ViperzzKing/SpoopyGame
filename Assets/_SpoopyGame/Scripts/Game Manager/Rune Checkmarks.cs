using UnityEngine;
using UnityEngine.SceneManagement;

public class RuneCheckmarks : MonoBehaviour
{
    // GameObjects holding the runes for tutorial and main game
    public GameObject tutorialRunes;
    public GameObject gameRunes;

    // Static manager
    public static RuneCheckmarks RuneManager;

    // Tracks whether the main game has started
    public bool gameStarted = false;

    // Stores rune counts for each ending: [0]=start, [1]=crypt, [2]=ritual, [3]=mansion
    public int[] finishes = new int[4];

    // only one RuneCheckmarks can exist
    private void Awake()
    {
        if (RuneManager == null) RuneManager = this;
        else Destroy(gameObject);
    }

    //----------------------------------------------------\\

    // Adds runeAmount to a specific finish slot, but caps at 5
    public void ChangeFinish(int specificFinish, int runeAmount)
    {
        if (finishes[1] + finishes[2] + finishes[3] >= 5 && runeAmount >= 1) return;
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
        Debug.Log("Checking endings");

        // 5 tutorial runes collected - switch to main game runes
        if (CheckRunes(0) == 5)
        {
            Debug.Log("Started Game");
            gameStarted = true;
            tutorialRunes.SetActive(false);
            gameRunes.SetActive(true);
            ChangeFinish(0, -5); // Reset tutorial rune count
        }

        // Crypt ending triggered
        if (CheckRunes(1) == 5)
        {
            Debug.Log("crypt ending");
            SceneManager.LoadScene(0);
        }

        // Ritual ending triggered
        if (CheckRunes(2) == 5)
        {
            Debug.Log("Finished Ritual Ending");
            SceneManager.LoadScene(0);
        }

        // Mansion ending triggered
        if (CheckRunes(3) == 5)
        {
            Debug.Log("Finished Mansion Ending");
            SceneManager.LoadScene(0);
        }
    }
    
    [ContextMenu("Change Finish")]
    public void TestChange()
    {
        ChangeFinish(1, 1);
    }
}