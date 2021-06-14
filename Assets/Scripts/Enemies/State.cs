using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    protected StateManager stateManager;

    public State(StateManager manager)
    {
        stateManager = manager;
    }

    public virtual void RunCurrentState()
    {

    }

    public virtual void OnStateEnter()
    {

    }

    public virtual void OnStateExit()
    {

    }

    // Returns true if a straight line can be drawn between this object and target. Must be within range and visibility arc.
    public bool CheckVisibility()
    {
        //compute direction to target
        var directionToTarget = stateManager.playerTarget.transform.position - stateManager.transform.position;
        //calculate degrees from forward direction
        var degreesToTarget = Vector3.Angle(stateManager.transform.forward, directionToTarget);
        //target visible if within half of the angle arc
        var withinArc = degreesToTarget < (stateManager.visibilityAngle / 2);

        if (withinArc == false)
        {
            return false;
        }

        //compute distance to the point
        var distanceToTarget = directionToTarget.magnitude;
        //ray should go as far as target is or max distance, whicher if shorter
        var rayDistance = Mathf.Min(stateManager.maxDetectDistance, distanceToTarget);
        //create a ray that shoots out from current position
        var ray = new Ray(stateManager.transform.position, directionToTarget);
        //store info on what was hit
        RaycastHit hit;
        //records info about whether target is in range and not blocked
        var canSee = false;

        //fire raycast. Did it hit anything?
        if (Physics.Raycast(ray, out hit, rayDistance, stateManager.visibilityMask))
        {
            //did ray hit target?
            if (hit.collider.transform == stateManager.playerTarget.transform)
            {
                //nothing blocking ray
                canSee = true;
            }
            //visualise the ray
            //Debug.DrawLine(stateManager.transform.position, hit.point);
            //transform.LookAt(target);
        }
        else
        {
            //ray hit nothing
            //Debug.DrawRay(stateManager.transform.position, directionToTarget.normalized * rayDistance);

        }
        return canSee;
    }

    protected void ChaseTarget()
    {
        LookAtTarget();
        MoveTowardsTarget();
    }

    void LookAtTarget()
    {
        //get the distance between target and current position
        Vector3 direction = stateManager.playerTarget.transform.position - stateManager.transform.position;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        //rotate and look at the target
        stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.rotateSpeed * Time.deltaTime);
    }

    void MoveTowardsTarget()
    {
        //Get the distance from self to target
        stateManager.distanceFromTarget = Vector3.Distance(stateManager.transform.position, stateManager.playerTarget.transform.position);

        //Move toward the target until minDetectDistance is reached (as not to run into the target)
        if (stateManager.distanceFromTarget > stateManager.stoppingDistance)
        {
            //move to target
            stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, stateManager.playerTarget.transform.position, stateManager.chaseSpeed * Time.deltaTime);
        }
    }

}
