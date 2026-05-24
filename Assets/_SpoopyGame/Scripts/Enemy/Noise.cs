using System;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public float objectVolume;
    public float soundDistance;
    public float enemyDistanceMultipler;
    public float newSoundVolume;


    private void Update()
    {
        soundDistance = Vector3.Distance(EnemyAI.enemyAI.transform.position, transform.position) * 3;
        enemyDistanceMultipler = 100 / soundDistance;
    }

    private void GenerateSound(float soundVolume)
    {
        EnemyAI.enemyAI.noiseVolume = soundVolume;
    }

    [ContextMenu("Make Sound")]
    private void MakeSound()
    {
        soundDistance = Vector3.Distance(EnemyAI.enemyAI.transform.position, transform.position) * 3;
        enemyDistanceMultipler = 100 / soundDistance;

        
        newSoundVolume = objectVolume * enemyDistanceMultipler;
        if (EnemyAI.enemyAI.enemyState == EnemyAI.EnemyStates.Searching)
        {
            newSoundVolume = newSoundVolume * 1.5f;
        }
        
        if (EnemyAI.enemyAI.enemyState != EnemyAI.EnemyStates.Chasing)
        {
            GenerateSound(newSoundVolume);
            EnemyAI.enemyAI.chasingTarget = transform;
            EnemyAI.enemyAI.soundDetected = true;
        }
    }
}
