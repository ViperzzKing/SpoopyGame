using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;

    [Header("Detection")]
    [SerializeField] private float noiseDetectionVolume = 30;
    [SerializeField] private float noiseVolume;
    [SerializeField] private bool soundDetected;
    public bool PlayerDetected { get; private set; }
    
    [Header("Navigation")]
    [SerializeField] private Collider[] validAreas;
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] Transform chasingTarget;
    [SerializeField] private Transform searcher;
    [SerializeField] private float requiredDistance;
    public bool destinationReached = true;
    
    [Header("Timing")]
    [SerializeField] private float searchingTime = 10;
    
    [Header("States")]
    [SerializeField] private bool enemyIsAttacking;
    [SerializeField] private bool enemyStunned = false;
    [SerializeField] private EnemyState previousState;
    public EnemyState CurrentState { get; private set; }
    
    
    public enum EnemyState
    {
        Roaming, 
        Chasing,
        Searching,
        Attacking,
        Stunned
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        destinationReached = true;
    }

    private void Update()
    {
        StateHandler();
        ChangeState();
    }

    // Handles The States
    private void StateHandler()
    {
        switch (CurrentState)
        {
            case EnemyState.Roaming:
                Roam();
                break;
            case EnemyState.Chasing:
                Chase(chasingTarget);
                break;
            case EnemyState.Searching:
                Search();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Stunned:
                Stun();
                break;
        }
    }

    // Detections for Switching States
    private void ChangeState()
    {
        bool playerHidesFromChase = HidePlayer.Instance.PlayerIsHiding && CurrentState == EnemyState.Chasing || 
                                    BasicMovement.Instance.CurrentState == BasicMovement.PlayerState.Crouch && 
                                    CurrentState == EnemyState.Chasing &&
                                    !PlayerDetected;
        bool hearsNoise = noiseVolume >= noiseDetectionVolume;

        // If Player Detected Player Becomes Chasing Target And Enemy Enters Chasing State
        if (PlayerDetected)
        {
            chasingTarget = BasicMovement.Instance.transform;
            CurrentState = EnemyState.Chasing;
        }

        // When the player hides during a chase and is not seen enemy enters searching state
        if (playerHidesFromChase)
            CurrentState = EnemyState.Searching;
        
        // If enemyStunned enter Stunned State
        if (enemyStunned)
            ApplyStun();

        // uses Noise script And if noise is bigger than noise detection volume
        if (hearsNoise)
        {
            CurrentState = EnemyState.Chasing;
            Debug.Log("I HEARD THAT");
            noiseVolume = 0;
        }
        
    }


    //----------------- STATES -----------------\\

    // when destination reached
    // Get random postion on map, move the searcher there, set enemy navmesh there
    private void Roam()
    {
        SetDestination(searching: false);
    }
    

    private void Search()
    {
        SetDestination(searching: true);
    }
    
    private void Chase(Transform target)
    {
        bool enemyCloseEnough = enemy.remainingDistance <= enemy.stoppingDistance + 5;
        bool pathReady = !enemy.pathPending;
        bool hasPath = enemy.hasPath;
        
        bool enemyReachedLocation = hasPath && pathReady && enemyCloseEnough;


        enemy.destination = target.transform.position;
        
        // When enemy reaches player go to attack state
        if (enemyReachedLocation && target.CompareTag("Player"))
        {
            CurrentState = EnemyState.Attacking;
        }
        else if (enemyReachedLocation && soundDetected) // If it reached a sound instead
        {
            destinationReached = true;
            CurrentState = EnemyState.Searching; // Chased Sound
            soundDetected = false;
        }
        
        // When player crouches while not being seen enter searching state
        if (!PlayerDetected && BasicMovement.Instance.CurrentState == BasicMovement.PlayerState.Crouch && enemyReachedLocation)
        {
            CurrentState = EnemyState.Searching;
        }
    }

    // Same as Roaming but smaller radius and around the enemy

    // starts attack Enumerator only when enemy is not already attacking
    private void Attack()
    {
        if(!enemyIsAttacking)
            StartCoroutine(AttackingTime(1f));
    }
    
    // Stuns the enemy for StunTime
    private void Stun()
    {
        //TODO -- Set up code to make the enemy in a certain animation
        if (!enemyStunned)
        {
            
            StartCoroutine(StunTime(10f)); // Replace 10f with the rune's stun time or add a variable
        }
    }


    //-------------------------------------\\

    private IEnumerator AttackingTime(float attackingTime)
    {
        // True at start, false at end
        enemyIsAttacking = true;
        Debug.Log("Attacking");
        yield return new WaitForSeconds(attackingTime); // Animation Time
        CurrentState = EnemyState.Chasing;
        yield return new WaitForSeconds(3); // Cooldown before can attack again
        enemyIsAttacking = false;
    }
    
    // Time for Searching with how long to search for
    private IEnumerator SearchingTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        Debug.Log("finished searching");
        CurrentState = EnemyState.Roaming;
    }

    //Time for stun with how long stunned for
    private IEnumerator StunTime(float stunTime)
    {
        enemyStunned = true;
        yield return new WaitForSeconds(stunTime);
        CurrentState = previousState;
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
            if (IsInsideValidArea(pos) && CurrentState == EnemyState.Roaming) return pos;
            if (IsInsideValidArea(localPos) && CurrentState == EnemyState.Searching) return localPos;
            // checks bounds for pos and local to see if its valid
        }
        //Debug.Log("failed");
        return transform.position;
    }

    private void SetDestination(bool searching)
    {
        if (destinationReached)
        {
            Vector3 newPos = GetValidRandomPosition();
            Debug.Log(newPos);
            
            searcher.position = newPos;
            Debug.Log(searcher.position);
            
            enemy.SetDestination(newPos);
            
            destinationReached = false;
            
            if(searching)
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

        // it's not in bounds
        return false;
    }

    public void HearSound(float newSoundVolume, Transform soundTarget)
    {
        if (CurrentState != EnemyState.Chasing)
        {
            if (CurrentState == EnemyState.Searching)
            {
                // while enemy is searching sound is increased
                newSoundVolume = newSoundVolume * 1.5f;
            }
            
            noiseVolume = newSoundVolume;
            chasingTarget = soundTarget;
            soundDetected = true;
        }
    }

    public void SetPlayerDetected(bool detected)
    {
        PlayerDetected = detected;
    }

    public void ApplyStun()
    {
        previousState = CurrentState;
        CurrentState = EnemyState.Stunned;
    }
}
