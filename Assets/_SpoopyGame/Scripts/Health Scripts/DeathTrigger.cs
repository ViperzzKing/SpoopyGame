using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    public void PlayerDeath(int sceneIndex)
    {
        Debug.Log("Triggering Death Script");
        SceneManager.LoadScene(sceneIndex);
    }
}
