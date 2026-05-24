using System;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private BasicMovement player;
    [SerializeField] private EnemyAI ai;
    
    private void OnParticleCollision(GameObject other)
    {
        if(player.currentPlayerState == BasicMovement.State.Crouch && EnemyAI.enemyAI.enemyState == EnemyAI.EnemyStates.Stunned) return;


        if (other.transform.CompareTag("Player"))
            ai.playerDetected = true;
        else
            ai.playerDetected = false;
    }
}
