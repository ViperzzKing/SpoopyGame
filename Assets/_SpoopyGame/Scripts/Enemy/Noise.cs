using UnityEngine;

public class Noise : MonoBehaviour
{
    // TODO -- add varibles for sound related stuff
    private float objectSound;
    private float currentSound;

    private void Update()
    {
        if (true)
            GenerateSound(objectSound);
    }

    private void GenerateSound(float soundVolume)
    {
        currentSound = soundVolume;
    }
}
