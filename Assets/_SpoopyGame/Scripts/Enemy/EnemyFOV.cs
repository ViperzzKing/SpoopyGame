using System;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private BasicMovement player;
    [SerializeField] private EnemyAI ai;
    
    private void OnParticleCollision(GameObject other)
    {
        if(player.currentPlayerState == BasicMovement.State.Crouch) return;

        Debug.Log("detected");
        ai.playerDetected = true;
        gameObject.SetActive(false);
    }
}
