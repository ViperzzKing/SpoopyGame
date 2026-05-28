using UnityEngine;
using UnityEngine.SceneManagement;

public class RuneCheckmarks : MonoBehaviour
{
    public GameObject tutorialRunes;
    public GameObject gameRunes;
    public static RuneCheckmarks RuneManager;
    public bool gameStarted = false;

    public int[] finishes = new int[4];

    private void Awake()
    {
        if (RuneManager == null) RuneManager = this;
        else Destroy(gameObject);
    }
    

    //----------------------------------------------------\\

    public void ChangeFinish(int specificFinish, int runeAmount)
    {
        if (finishes[1] + finishes[2] + finishes[3] >= 5 && runeAmount >= 1) return;
        finishes[specificFinish] += runeAmount;
    }
    
    public int CheckRunes(int specificFinish)
    {
        if (specificFinish < 0) return 0;
        return finishes[specificFinish];
        
    }

    [ContextMenu("Test Check")]
    public void EndingTrigger()
    {
        Debug.Log("Checking endings");
        
        
        if (CheckRunes(0) == 5)
        {
            Debug.Log("Started Game");
            gameStarted = true;
            tutorialRunes.SetActive(false);
            gameRunes.SetActive(true);
            ChangeFinish(0, -5);
        }
        
        if (CheckRunes(1) == 5)
        {
            Debug.Log("crypt ending");
            // Ending
            SceneManager.LoadScene(0);
        }
 
        if (CheckRunes(2) == 5)
        {
            Debug.Log("Finished Ritual Ending");
            // Ending
            SceneManager.LoadScene(0);
        }

        if (CheckRunes(3) == 5)
        {
            Debug.Log("Finished Mansion Ending");
            // Ending
            SceneManager.LoadScene(0);
        }
    }

    [ContextMenu("Change Finish")]
    public void TestChange()
    {
        ChangeFinish(1, 1);
    }
}
