using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float requiredDistance;
    public float currentDistance;
    
    [Space]
    
    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private EnemyStates enemyState;
    [SerializeField] private NavMeshAgent enemy;

    public enum EnemyStates
    {
        Roaming, 
        Chasing,
        Searching,
        Attacking,
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
                RoamingState();
                break;
            case EnemyStates.Chasing:
                ChasingState(target: enemyTarget.transform);
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
        float distanceFromTarget = Vector3.Distance(transform.position, enemyTarget.transform.position);
        currentDistance = distanceFromTarget;
        
        // TODO -- Change conditon names & True statements to match what needs to happen
        bool closeEnough = distanceFromTarget <= requiredDistance;



        if (closeEnough)
            enemyState = EnemyStates.Chasing;

    }


    //----------------- STATES -----------------\\


    private void RoamingState()
    {
        
        
        // TODO -- choose random spot on map OR nearby and change NavMesh target to that spot
        // TODO -- Once spot has been reached repeat
    }

    private void ChasingState(Transform target)
    {
        enemy.destination = target.transform.position;

        
        // TODO -- once in sight (AND, OR) distance has been reached, NavMesh target becomes the player
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
}
