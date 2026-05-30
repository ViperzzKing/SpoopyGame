using System;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private BasicMovement player;
    [SerializeField] private EnemyAI enemyAI;

    private void Awake()
    {
        if (player == null) player = BasicMovement.Instance;
        if (enemyAI == null)enemyAI = EnemyAI.Instance;
    }

    // I dont know how to raycast a fov 
    private void OnParticleCollision(GameObject other)
    {
        bool playerCrouching = player.CurrentState == BasicMovement.PlayerState.Crouch;
        bool enemyStunned = enemyAI.CurrentState == EnemyAI.EnemyState.Stunned;
        
        if(playerCrouching || enemyStunned) return;
        // Returns if player is crouching or stunned

        if (other.transform.CompareTag("Player"))
            enemyAI.SetPlayerDetected(true);
        else
            enemyAI.SetPlayerDetected(false);
    }
}
