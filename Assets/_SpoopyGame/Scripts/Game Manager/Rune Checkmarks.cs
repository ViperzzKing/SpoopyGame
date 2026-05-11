using UnityEngine;

public class RuneCheckmarks : MonoBehaviour
{
    public static RuneCheckmarks RuneManager;

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
    public void TestCheck()
    {
        Debug.Log(CheckRunes(0));
        
        if (CheckRunes(1) == 5)
            Debug.Log("Finished Crypt Ending");
        
        if (CheckRunes(2) == 5)
            Debug.Log("Finished Ritual Ending");
        
        if (CheckRunes(3) == 5)
            Debug.Log("Finished Mansion Ending");
    }

    [ContextMenu("Change Finish")]
    public void TestChange()
    {
        ChangeFinish(1, 1);
    }
}
