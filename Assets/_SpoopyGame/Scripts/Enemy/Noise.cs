using System;
using UnityEngine;

public class Noise : MonoBehaviour
{
    // This goes on anything that can make sound

    [SerializeField] private float objectVolume; // how much sound it makes
    [SerializeField] private float soundDistance; // how far is the enemy
    [SerializeField] private float enemyDistanceMultipler; // multiplier based on distance
    [SerializeField] private float newSoundVolume; // object volume x multiplier
    

    // run this to make sound
    [ContextMenu("Make Sound")]
    private void MakeSound()
    {
        if (EnemyAI.Instance == null)
        {
            Debug.LogWarning("No EnemyAI found");
            return;
        }
        
        // enemy distance 
        soundDistance = Mathf.Clamp(Vector3.Distance(EnemyAI.Instance.transform.position, transform.position), 1, 1000) * 3;
        // turn it into multiplier
        enemyDistanceMultipler = 100 / soundDistance;

        // multiply objects sound by certain amount based on distance
        newSoundVolume = objectVolume * enemyDistanceMultipler;

        EnemyAI.Instance.HearSound(newSoundVolume, transform);

    }
}