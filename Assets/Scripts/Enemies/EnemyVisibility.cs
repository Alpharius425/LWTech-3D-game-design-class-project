using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

// ENEMY AI THAT DETECTS IF THE TARGET IS WITHIN FIELD OF VIEW //

public class EnemyVisibility : MonoBehaviour
{
    //public StateManager stateManager;
    public Transform target;
    // If target is further than this distance then it can't be seen
    public float maxDistance = 10f;
    // Visibility
    [Range(0f, 360f)]
    public float angle = 45f;
    // If true, change material color
    [SerializeField] bool visualize = true;
    public Light FOVCone;
    public new Renderer renderer;

    // Property can be accessed by other classes to determine if target is visible
    public bool TargetIsVisible { get; private set; }

    private void Start()
    {
        //Auto set the drone Field Of View light source
        FOVCone = GetComponentInChildren<Light>();
    }
    // Check every frame to see if target is visible
    void Update()
    {
        target = GetComponent<StateManager>().playerTarget.transform;

        TargetIsVisible = CheckVisibility();
        if (visualize)
        {
            //Adjust the light angle to the visibility angle
            FOVCone.spotAngle = angle;

            //change the material color, otherwise remain white
            var color = TargetIsVisible ? Color.red : Color.white;
            var coneColor = TargetIsVisible ? Color.red : Color.white;
            //GetComponent<Renderer>().material.color = color;
            renderer.material.color = color;
            FOVCone.color = coneColor;
        }
    }

    // Returns true if target is visible
    public bool CheckVisibilityToPoint(Vector3 worldPoint)
    {
        //calculate direction from location to the point
        var directionToTarget = worldPoint - transform.position;
        //calculate the number of degrees from the forward direction
        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
        //this target is within visibility if it's within half of the angle, else not visible
        var withinArc = degreesToTarget < (angle / 2);

        if (withinArc == false)
        {
            return false;
        }

        //calculate distance to target
        var distanceToTarget = directionToTarget.magnitude;
        //use max distance
        var rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        //create a ray going from current location in the specified direction
        var ray = new Ray(transform.position, directionToTarget);
        //store information about what is hit
        RaycastHit hit;

        //Did the raycast hit anything?
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            //something was hit
            if (hit.collider.transform == target)
            {
                //target is visible
                return true;
            }
            //something is blocking the target, therefore it is not visible
            return false;
        }
        else
        {
            //unobstructed line of sight -- target is visible
            return true;
        }
    }

    // Returns true if a straight line can be drawn between this object and target. Must be within range and visibility arc.
    public bool CheckVisibility()
    {
        //compute direction to target
        var directionToTarget = target.position - transform.position;
        //calculate degrees from forward direction
        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
        //target visible if within half of the angle arc
        var withinArc = degreesToTarget < (angle / 2);

        if (withinArc == false)
        {
            return false;
        }

        //compute distance to the point
        var distanceToTarget = directionToTarget.magnitude;
        //ray should go as far as target is or max distance, whicher if shorter
        var rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        //create a ray that shoots out from current position
        var ray = new Ray(transform.position, directionToTarget);
        //store info on what was hit
        RaycastHit hit;
        //records info about whether target is in range and not blocked
        var canSee = false;

        //fire raycast. Did it hit anything?
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            //did ray hit target?
            if (hit.collider.transform == target)
            {
                //nothing blocking ray
                canSee = true;
            }
            //visualise the ray
            Debug.DrawLine(transform.position, hit.point);
            //transform.LookAt(target);
        }
        else
        {
            //ray hit nothing
            Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);

        }
        return canSee;
    }   
}
