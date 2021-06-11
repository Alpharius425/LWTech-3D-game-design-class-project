using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{   
    public PatrolState(StateManager manager) : base(manager)
    {

    }

    public override void OnStateEnter()
    {
        stateManager.FOVCone.color = Color.white;
        stateManager.animator.SetBool("patrol", true);
        stateManager.animator.SetBool("dead", false);
        stateManager.animator.SetBool("chasing", false);
    }

    public override void RunCurrentState()
    {
        PatrolMovement();
        if (CheckVisibility())
        {
            stateManager.ChangeState(stateManager.chaseState);
        }
    }

    // PATROL
    void PatrolMovement()
    {
        Vector3 currentDronePos = stateManager.transform.position;
        Vector3 currentPatrolPoint = stateManager.patrolPoints[stateManager.patrolIndex].position;

        //checking distance to patrol point
        stateManager.distanceFromPatrol = Vector3.Distance(currentDronePos, currentPatrolPoint);

        //Rotate
        //get the distance between target and current position
        Vector3 direction = currentPatrolPoint - currentDronePos;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction);
        //rotate and look at the target
        stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.rotateSpeed * Time.deltaTime);

        //move toward patrol point
        stateManager.transform.Translate(Vector3.forward * stateManager.patrolSpeed * Time.deltaTime);


        //once at the patrol point increase the index, check that the value is within bounds, otherwise reset to 0
        if (stateManager.distanceFromPatrol < 0.8f)
        {
            stateManager.patrolIndex++;
            
            if (stateManager.patrolIndex >= stateManager.patrolPoints.Length)
            {
                stateManager.patrolIndex = 0;
            }
        }
    }
}
