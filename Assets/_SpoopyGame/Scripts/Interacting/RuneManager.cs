using System;
using Unity.Collections;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    private Health playerHealth;
    
    public enum RuneType
    {
        Progression,
        Heal,
        Shield,
        Stun,
        Locate,
    }
    
    [field: SerializeField] public RuneType CurrenType { get; private set; }

    [Header("Heal Settings")] 
    [SerializeField] private float healAmount;
    [SerializeField] private float healDestoryTime;
    
    [Header("Keybinds")] 
    [SerializeField] private KeyCode triggerRuneKey;

    private void Update()
    {
        RuneTypeHandler();
    }

    private void RuneTypeHandler()
    {
        switch (CurrenType)
        {
            case RuneType.Progression:
                break;
            case RuneType.Heal:
                Heal();
                break;
            case RuneType.Shield:
                Shield();
                break;
            case RuneType.Stun:
                break;
            case RuneType.Locate:
                break;
        }
    }

    private void Heal()
    {
        if (Input.GetKeyDown(triggerRuneKey) && InspectObject.Instance.CurrentItemHolding == transform)
        {
            playerHealth = FindFirstObjectByType<Health>();
            playerHealth.Heal(healAmount);
            Destroy(gameObject, healDestoryTime);
            // Play Particles etc,
            // Break Rune Effect
        }
    }

    private void Shield()
    {
        // Future System
    }

    private void Stun()
    {
        // Future System
    }

    private void Locate()
    {
        // Future System
    }
}
