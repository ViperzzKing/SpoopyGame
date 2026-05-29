using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI enemyAI;

    private bool enemyIsAttacking;
    [SerializeField] private float searchingTime = 10;
    [SerializeField] private float requiredDistance;
    public float currentDistance;
    public bool destinationReached = true;
    public bool playerDetected = false;
    
    public bool enemyStunned = false;

    public float noiseDetectionVolume = 30;
    public float noiseVolume;
    public bool soundDetected;
    
    [Space] [SerializeField] private Collider[] validAreas;
    public EnemyStates enemyState;
    [SerializeField] private NavMeshAgent enemy;
    public Transform chasingTarget;
    [SerializeField] private Transform searcher;

    [SerializeField] private bool audioCheck = true;

    public enum EnemyStates
    {
        Roaming, 
        Chasing,
        Searching,
        Attacking,
        Stunned
    }

    private void Awake()
    {
        enemyAI = this;
    }

    private void Start()
    {
        destinationReached = true;
    }

    private void Update()
    {
        StateHandler();
        StateSwitcher();
    }

    // Handles The States
    private void StateHandler()
    {
        switch (enemyState)
        {
            case EnemyStates.Roaming:
                RoamingState(searcher);
                break;
            case EnemyStates.Chasing:
                ChasingState(chasingTarget);
                break;
            case EnemyStates.Searching:
                SearchingState();
                break;
            case EnemyStates.Attacking:
                AttackingState();
                break;
            case EnemyStates.Stunned:
                StunnedState();
                break;
        }
    }

    // Detections for Switching States
    private void StateSwitcher()
    {
        bool playerHidesFromChase = HidePlayer.playerHider.playerIsHiding && enemyState == EnemyStates.Chasing || 
                                    BasicMovement.playerController.currentPlayerState == BasicMovement.State.Crouch && 
                                    enemyState == EnemyStates.Chasing &&
                                    !playerDetected;
        bool hearsNoise = noiseVolume >= noiseDetectionVolume;

        // If Player Detected Player Becomes Chasing Target And Enemy Enters Chasing State
        if (playerDetected)
        {
            chasingTarget = BasicMovement.playerController.transform;
            enemyState = EnemyStates.Chasing;
        }

        // When the player hides during a chase and is not seen enemy enters searching state
        if (playerHidesFromChase)
            enemyState = EnemyStates.Searching;
        
        // If enemyStunned enter Stunned State
        if (enemyStunned)
            enemyState = EnemyStates.Stunned;

        // uses Noise script And if noise is bigger than noise detection volume
        if (hearsNoise)
        {
            enemyState = EnemyStates.Chasing;
            Debug.Log("I HEARD THAT");
            noiseVolume = 0;
        }
        
    }


    //----------------- STATES -----------------\\

    // when destination reached
    // Get random postion on map, move the searcher there, set enemy navmesh there
    private void RoamingState(Transform target)
    {
        if (destinationReached)
        {
            Vector3 newPos = GetValidRandomPosition();
            Debug.Log(newPos);
            
            searcher.position = newPos;
            Debug.Log(searcher.position);
            
            enemy.SetDestination(newPos);
            Debug.Log("set enemy destination");
            
            destinationReached = false;
        }
        else
        {
            // reset once enemy reaches destination
            if (enemy.remainingDistance <= enemy.stoppingDistance && !enemy.pathPending)
            {
                destinationReached = true;
            }
        }
    }
    

    private void ChasingState(Transform target)
    {
        bool enemyCloseEnough = enemy.remainingDistance <= enemy.stoppingDistance + 5;
        bool pathReady = !enemy.pathPending;
        bool hasPath = enemy.hasPath;
        
        bool enemyReachedLocation = hasPath && pathReady && enemyCloseEnough;


        enemy.destination = target.transform.position;
        
        // When enemy reaches player go to attack state
        if (enemyReachedLocation && target.CompareTag("Player"))
        {
            enemyState = EnemyStates.Attacking;
        }
        else if (enemyReachedLocation && soundDetected) // If it reached a sound instead
        {
            destinationReached = true;
            enemyState = EnemyStates.Searching; // Chased Sound
            soundDetected = false;
        }
        
        // When player crouches while not being seen enter searching state
        if (!playerDetected && BasicMovement.playerController.currentPlayerState == BasicMovement.State.Crouch && enemyReachedLocation)
        {
            enemyState = EnemyStates.Searching;
        }
    }

    // Same as Roaming but smaller radius and around the enemy
    private void SearchingState()
    {
        if (destinationReached)
        {
            Vector3 newPos = GetValidRandomPosition();
            Debug.Log(newPos);
            
            searcher.position = newPos;
            Debug.Log(searcher.position);
            
            enemy.SetDestination(newPos);
            Debug.Log("set enemy searching destination");
            
            destinationReached = false;
            StartCoroutine(SearchingTime(searchingTime));
        }
        else
        {
            if (enemy.remainingDistance <= enemy.stoppingDistance && !enemy.pathPending)
            {
                destinationReached = true;
            }
        }
    }

    // starts attack Enumerator only when enemy is not already attacking
    private void AttackingState()
    {
        if(!enemyIsAttacking)
            StartCoroutine(AttackingTime(1f));
    }
    
    // Stuns the enemy for StunTime
    private void StunnedState()
    {
        //TODO -- Set up code to make the enemy in a certain animation
        StartCoroutine(StunTime(10f)); // Replace 10f with the rune's stun time or add a variable
    }


    //-------------------------------------\\

    private IEnumerator AttackingTime(float attackingTime)
    {
        // True at start, false at end
        enemyIsAttacking = true;
        Debug.Log("Attacking");
        yield return new WaitForSeconds(attackingTime); // Animation Time
        enemyState = EnemyStates.Chasing;
        yield return new WaitForSeconds(3); // Cooldown before can attack again
        enemyIsAttacking = false;
    }
    
    // Time for Searching with how long to search for
    private IEnumerator SearchingTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        Debug.Log("finished searching");
        enemyState = EnemyStates.Roaming;
    }

    //Time for stun with how long stunned for
    private IEnumerator StunTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        enemyStunned = false;
    }

    // Random World Position Between -250 and 250 radius
    private Vector3 RandomWorldPosition()
    {
        return new Vector3(Random.Range(-250, 250), Random.Range(0, 5), Random.Range(-250, 250));
    }

    // Random local postion of the enemy
    private Vector3 RandomLocalPostion()
    {
        return new Vector3(transform.position.x + Random.Range(-20, 20), 
                            0,
                            transform.position.z + Random.Range(-20, 20));
    }

    // makes sure it's a valid position and not outside the map
    private Vector3 GetValidRandomPosition()
    {
        int maxTries = 1000;
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 pos = RandomWorldPosition();
            Vector3 localPos = RandomLocalPostion();
            //Debug.Log(RandomPosition());
            if (IsInsideValidArea(pos) && enemyState == EnemyStates.Roaming) return pos;
            if (IsInsideValidArea(localPos) && enemyState == EnemyStates.Searching) return localPos;
            // checks bounds for pos and local to see if its valid
        }
        //Debug.Log("failed");
        return transform.position;
    }

    [ContextMenu("test")]
    private void TestLocalPos()
    {
        Debug.Log(RandomLocalPostion());
    }
    
    bool IsInsideValidArea(Vector3 checkerPosition)
    {
        foreach (var area in validAreas)
        {
            if (area.bounds.Contains(checkerPosition))
            {
                // Random Position is inside bounds
                return true;
            }
        }

        // its not in bounds
        return false;
    }
}
