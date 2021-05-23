using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public StateManager stateManager;
    
    public override State RunCurrentState()
    {
        PatrolMovement();
        if (stateManager.canSeePlayer)
        {
            return stateManager.chaseState;
        }
        else
        {
            return this;
        }
    }


    // PATROL
    void PatrolMovement()
    {
        //Debug.Log("Patrol");
       
        //checking distance to patrol point
        stateManager.distanceFromPatrol = Vector3.Distance(stateManager.transform.position, stateManager.patrolPoints[stateManager.patrolIndex].transform.position);

        //Rotate
        //get the distance between target and current position
        Vector3 direction = stateManager.patrolPoints[stateManager.patrolIndex].transform.position - stateManager.transform.position;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction);
        //rotate and look at the target
        stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.rotateSpeed * Time.deltaTime);

        //move toward patrol point
        stateManager.transform.Translate(Vector3.forward * stateManager.patrolSpeed * Time.deltaTime);
        //once at the patrol point increase the index, check that the value is within bounds, otherwise reset to 0
        if (stateManager.distanceFromPatrol < 0.5f)
        {
            stateManager.patrolIndex++;
            if (stateManager.patrolIndex >= stateManager.patrolPoints.Length)
            {
                stateManager.patrolIndex = 0;
            }
        }

    }
}
