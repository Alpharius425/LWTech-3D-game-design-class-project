using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public StateManager stateManager;
    float distanceFromTarget; // actual distance from the target
    bool canChaseTarget; // true if targetIsVisible, set by EnemyVisibility.cs

    public override State RunCurrentState()
    {
        ChaseTarget();
        if (stateManager.isInAttackRange && stateManager.canSeePlayer)
        {
            return stateManager.attackState;
        }
        else if (!stateManager.isInAttackRange && stateManager.canSeePlayer)
        {
            return this;
        }
        else
        {
            return stateManager.searchState;
        }
    }

    //Put the rotate script into a method for ease of use
    void LookAtTarget()
    {
        //get the distance between target and current position
        Vector3 direction = stateManager.playerTarget.transform.position - stateManager.thisEnemy.transform.position;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction);
        //rotate and look at the target
        stateManager.thisEnemy.transform.rotation = Quaternion.Slerp(stateManager.thisEnemy.transform.rotation, rotation, stateManager.rotateSpeed * Time.deltaTime);
    }

    // CHASE TARGET
    void ChaseTarget()
    {
        //Debug.Log("chase");

        LookAtTarget();

        //Get the distance from self to target
        distanceFromTarget = Vector3.Distance(stateManager.thisEnemy.transform.position, stateManager.playerTarget.transform.position);
        
        //Move toward the target until minDetectDistance is reached (as not to run into the target)
        if (distanceFromTarget > stateManager.minDetectDistance)
        {
            //move to target
            stateManager.thisEnemy.transform.position = Vector3.MoveTowards(stateManager.thisEnemy.transform.position, stateManager.playerTarget.transform.position, stateManager.chaseSpeed * Time.deltaTime);
        } 
    }
}
