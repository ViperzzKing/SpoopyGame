using System;
using UnityEngine;

public class Noise : MonoBehaviour
{
    // This goes on anything that can make sound
    
    public float objectVolume; // how much sound it makes
    public float soundDistance; // how far is the enemy
    public float enemyDistanceMultipler; // multiplier based on distance
    public float newSoundVolume; // object volume x multiplier
    

    // used to send a sound volume to the enemy
    private void GenerateSound(float soundVolume)
    {
        EnemyAI.enemyAI.noiseVolume = soundVolume;
    }

    // run this to make sound
    [ContextMenu("Make Sound")]
    private void MakeSound()
    {
        // enemy distance
        soundDistance = Vector3.Distance(EnemyAI.enemyAI.transform.position, transform.position) * 3;
        // turn it into multiplier
        enemyDistanceMultipler = 100 / soundDistance;

        // multiply objects sound by certain amount based on distance
        newSoundVolume = objectVolume * enemyDistanceMultipler;
        if (EnemyAI.enemyAI.enemyState == EnemyAI.EnemyStates.Searching)
        {
            // while enemy is searching sound is increased
            newSoundVolume = newSoundVolume * 1.5f;
        }
        
        // if enemy is not chasing
        if (EnemyAI.enemyAI.enemyState != EnemyAI.EnemyStates.Chasing)
        {
            GenerateSound(newSoundVolume);
            EnemyAI.enemyAI.chasingTarget = transform;
            EnemyAI.enemyAI.soundDetected = true;
        }
    }
}
