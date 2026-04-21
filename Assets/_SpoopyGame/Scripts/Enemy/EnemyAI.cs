using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float requiredDistance;
    public float currentDistance;
    public bool destinationReached = true;
    public bool playerDetected = false;

    [Space] [SerializeField] private Collider[] validAreas;
    [SerializeField] private EnemyStates enemyState;
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private Transform searcher;

    [SerializeField] private bool audioCheck = true;

    public enum EnemyStates
    {
        Roaming, 
        Chasing,
        Searching,
        Attacking,
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
                ChasingState(player);
                break;
            case EnemyStates.Searching:
                SearchingState();
                break;
            case EnemyStates.Attacking:
                AttackingState();
                break;
        }
    }

    private void StateSwitcher()
    {
        // TODO -- Change conditon names & True statements to match what needs to happen
        

        if (playerDetected)
            enemyState = EnemyStates.Chasing;

    }


    //----------------- STATES -----------------\\


    private void RoamingState(Transform target)
    {
        if (destinationReached)
        {
            Vector3 newPos = GetValidRandomPosition();
            searcher.position = newPos;
            enemy.SetDestination(newPos);
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
        enemy.destination = target.transform.position;
        // TODO -- when hears sound in specific distance Navmesh target becomes sound source
    }

    private void SearchingState()
    {
        // TODO -- change a searching varible to true

        // TODO -- when enemy loses sight (AND, OR) player is hiding, NavMesh target becomes Roaming but closer, AND sound detection is increased

        StartCoroutine(SearchingTime(0)); // Replace 0 with searching time or add a varible
    }

    private void AttackingState()
    {
        // TODO -- Whatever happends when the monster attacks the player idk, damage, instant death something like that?
        // TODO -- just make a condition that enables a bool for now
    }


    //-------------------------------------\\


    private IEnumerator SearchingTime(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);

        // TODO -- Change a searching varible to false
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-150, 150), 0, Random.Range(-150, 150));
    }

    private Vector3 GetValidRandomPosition()
    {
        int maxTries = 50;
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 pos = RandomPosition();
            if (IsInsideValidArea(pos)) return pos;
        }
        return transform.position;
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
