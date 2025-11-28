using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // TODO -- Speed Control Varibles
    // TODO -- Extra Varibles For States

    private EnemyStates enemyState;

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
                ChasingState(target: gameObject.transform); // TODO -- have a way to change target to different things
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
        bool tempName1 = true;
        bool tempName2 = true;
        bool tempName3 = true;
        bool tempName4 = true;




        if (tempName1)
            enemyState = EnemyStates.Roaming;

        else if (tempName2)
            enemyState = EnemyStates.Chasing;
        
        else if (tempName3)
            enemyState = EnemyStates.Searching;
        
        else if (tempName4)
            enemyState = EnemyStates.Attacking;
    }


    //----------------- STATES -----------------\\


    private void RoamingState()
    {
        // TODO -- choose random spot on map OR nearby and change NavMesh target to that spot
        // TODO -- Once spot has been reached repeat
    }

    private void ChasingState(Transform target)
    {
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
