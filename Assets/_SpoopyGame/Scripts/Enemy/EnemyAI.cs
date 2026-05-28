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

    private void StateSwitcher()
    {
        bool playerHidesFromChase = HidePlayer.playerHider.playerIsHiding && enemyState == EnemyStates.Chasing || 
                                    BasicMovement.playerController.currentPlayerState == BasicMovement.State.Crouch && 
                                    enemyState == EnemyStates.Chasing &&
                                    !playerDetected;
        bool hearsNoise = noiseVolume >= noiseDetectionVolume;

        if (playerDetected)
        {
            chasingTarget = BasicMovement.playerController.transform;
            enemyState = EnemyStates.Chasing;
        }

        if (playerHidesFromChase)
            enemyState = EnemyStates.Searching;
        
        if (enemyStunned)
            enemyState = EnemyStates.Stunned;

        if (hearsNoise)
        {
            enemyState = EnemyStates.Chasing;
            Debug.Log("I HEARD THAT");
            noiseVolume = 0;
        }
        
    }


    //----------------- STATES -----------------\\


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
        
        
        if (enemyReachedLocation && target.CompareTag("Player"))
        {
            enemyState = EnemyStates.Attacking;
        }
        else if (enemyReachedLocation && soundDetected)
        {
            destinationReached = true;
            enemyState = EnemyStates.Searching; // Chased Sound
            soundDetected = false;
        }
        
        if (!playerDetected && BasicMovement.playerController.currentPlayerState == BasicMovement.State.Crouch && enemyReachedLocation)
        {
            enemyState = EnemyStates.Searching;
        }
    }

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

    private void AttackingState()
    {
        if(!enemyIsAttacking)
            StartCoroutine(AttackingTime(1f));
    }
    
    private void StunnedState()
    {
        //TODO -- Set up code to make the enemy in a certain animation
        StartCoroutine(StunTime(10f)); // Replace 10f with the rune's stun time or add a variable
    }


    //-------------------------------------\\

    private IEnumerator AttackingTime(float attackingTime)
    {
        enemyIsAttacking = true;
        Debug.Log("Attacking");
        yield return new WaitForSeconds(attackingTime);
        enemyState = EnemyStates.Chasing;
        yield return new WaitForSeconds(3);
        enemyIsAttacking = false;
    }
    
    private IEnumerator SearchingTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        Debug.Log("finished searching");
        enemyState = EnemyStates.Roaming;
    }


    private IEnumerator StunTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        enemyStunned = false;
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-250, 250), Random.Range(0, 5), Random.Range(-250, 250));
    }

    private Vector3 RandomLocalPostion()
    {
        return new Vector3(transform.position.x + Random.Range(-20, 20), 
                            0,
                            transform.position.z + Random.Range(-20, 20));
    }

    private Vector3 GetValidRandomPosition()
    {
        int maxTries = 1000;
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 pos = RandomPosition();
            Vector3 localPos = RandomLocalPostion();
            //Debug.Log(RandomPosition());
            if (IsInsideValidArea(pos) && enemyState == EnemyStates.Roaming) return pos;
            if (IsInsideValidArea(localPos) && enemyState == EnemyStates.Searching) return localPos;

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
                return true;
            }
        }

        return false;
    }
}
