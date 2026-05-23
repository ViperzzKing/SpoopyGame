using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI enemyAI;

    [SerializeField] private float searchingTime = 10;
    [SerializeField] private float requiredDistance;
    public float currentDistance;
    public bool destinationReached = true;
    public bool playerDetected = false;
    
    public bool enemyStunned = false;

    public float noiseVolume;
    
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
                StartCoroutine(AttackingState());
                break;
            case EnemyStates.Stunned:
                StunnedState();
                break;
        }
    }

    private void StateSwitcher()
    {
        bool playerHidesFromChase = HidePlayer.playerHider.playerIsHiding && enemyState == EnemyStates.Chasing;
        bool hearsNoise = noiseVolume >= 30;

        if (playerDetected)
            enemyState = EnemyStates.Chasing;

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
        bool enemyReachedSound = enemy.remainingDistance <= enemy.stoppingDistance && !enemy.pathPending &&
                                 !target.CompareTag("Player");
        
        bool enemyReachedPlayer = enemy.remainingDistance <= 5 && !enemy.pathPending &&
                                  target.CompareTag("Player");
        
        
        enemy.destination = target.transform.position;
        
        if (enemyReachedSound)
        {
            destinationReached = true;
            enemyState = EnemyStates.Searching; // Chased Sound
        }
        else if (enemyReachedPlayer)
        {
            enemyState = EnemyStates.Attacking;
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
        
        // TODO -- when enemy loses sight and player is hiding
    }

    private IEnumerator AttackingState()
    {
        // TODO -- Whatever happends when the monster attacks the player idk, damage, instant death something like that?
        Debug.Log("Attacking Player");
        // Damage Taken?
        // Finisher?
        yield return new WaitForSeconds(2f); // Attacking Animiation Time
        Debug.Log("Continuing To Chase");
        enemyState = EnemyStates.Chasing;

        // TODO -- just make a condition that enables a bool for now
    }
    private void StunnedState()
    {
        //TODO -- Set up code to make the enemy in a certain animation
        StartCoroutine(StunTime(10f)); // Replace 10f with the rune's stun time or add a variable
    }


    //-------------------------------------\\


    private IEnumerator SearchingTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        Debug.Log("finished searching");
        // If Couldn't Find Player
        enemyState = EnemyStates.Roaming;
        // If Found Player Hiding
        // Attacking State
        // If Found Player Sneaking
        // Chasing State
    }


    private IEnumerator StunTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        enemyStunned = false;
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-150, 150), Random.Range(0, 50), Random.Range(-150, 150));
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
