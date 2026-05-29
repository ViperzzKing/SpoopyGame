using System;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private BasicMovement player;
    [SerializeField] private EnemyAI ai;
    
    
   // I dont know how to raycast a fov 
    private void OnParticleCollision(GameObject other)
    {
        if(player.currentPlayerState == BasicMovement.State.Crouch || EnemyAI.enemyAI.enemyState == EnemyAI.EnemyStates.Stunned) return;
        // Returns if player is crouching or stunned

        if (other.transform.CompareTag("Player"))
            ai.playerDetected = true;
        else
            ai.playerDetected = false;
    }
}
