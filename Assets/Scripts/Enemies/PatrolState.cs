using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public StateManager stateManager;
    public float patrolSpeed = 1f; // speed while moving between patrol points
    public int patrolIndex; // index number of the patrol array
    [SerializeField] GameObject[] patrolPoints; // holds multiple patrol points
    public float rotateSpeed = 5f; // speed when rotating
    private float distanceFromPatrol; // distance from the patrol point


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
        Debug.Log("Patrol");
        //transform.LookAt(patrolPoints[patrolIndex].transform.position);

        //checking distance to patrol point
        distanceFromPatrol = Vector3.Distance(stateManager.thisEnemy.transform.position, patrolPoints[patrolIndex].transform.position);

        //Rotate
        //get the distance between target and current position
        Vector3 direction = patrolPoints[patrolIndex].transform.position - stateManager.thisEnemy.transform.position;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction);
        //rotate and look at the target
        stateManager.thisEnemy.transform.rotation = Quaternion.Slerp(stateManager.thisEnemy.transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        //move toward patrol point
        stateManager.thisEnemy.transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
        //once at the patrol point increase the index, check that the value is within bounds, otherwise reset to 0
        if (distanceFromPatrol < 0.5f)
        {
            patrolIndex++;
            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }

    }
}
