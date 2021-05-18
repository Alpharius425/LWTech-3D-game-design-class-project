using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [Header("CURRENT STATE")]
    public State currentState;

    [Header("ENEMY")]
    public Transform thisEnemy;
    public GameObject FOVCone; //light source that shows enemy FOV
    [Header("TARGET/PLAYER")]
    public GameObject playerTarget;
    [Header("DISTANCE FROM TARGET")]
    public float distanceFromTarget;
    [Header("SEARCH FOR TARGET")]
    public float minDetectDistance; // minimum distance from player before stopping (to not run into player)
    public float maxDetectDistance; //max distance for visibility
    public GameObject lookLeft, lookRight;
    [Header("CHASE TARGET")]
    public float chaseSpeed = 2f; // speed while chasing the target
    public float rotateSpeed = 5f; // speed when rotating
    [Header("ATTACK TARGET")]
    public float attackDistance; //distance when enemy begins attack
    [Header("GROUND")]
    public GameObject terrain;
    public float minDistanceFromGround;
    [Header("DISTANCE FROM GROUND")]
    float distanceFromGround; // keep drone from lowering
    
    [Header("CONTROL BOOLS")]
    public bool canSeePlayer;
    public bool isInAttackRange;
    
    [Header("STATE SCRIPTS")]
    public AttackState attackState;
    public PatrolState patrolState;
    public SearchState searchState;
    public ChaseState chaseState;



    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //find and set objects
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        FOVCone = GameObject.FindGameObjectWithTag("coneLight");
        canSeePlayer = GetComponent<EnemyVisibility>().TargetIsVisible;
        terrain = GameObject.FindGameObjectWithTag("Terrain");
        lookLeft = GameObject.Find("LookLeft");
        lookRight = GameObject.Find("LookRight");

        maxDetectDistance = GetComponent<EnemyVisibility>().maxDistance;
        attackDistance = maxDetectDistance / 2;
        isInAttackRange = (distanceFromTarget <= attackDistance);
        distanceFromTarget = Vector3.Distance(transform.position, playerTarget.transform.position);

        //keep enemy at a certain height above the ground
        distanceFromGround = Vector3.Distance(thisEnemy.transform.position, terrain.transform.position);
        if (distanceFromGround < minDistanceFromGround)
        {
            thisEnemy.transform.position = Vector3.up * 2;
        }

        RunStateMachine();   
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            //Switch to the next state
            SwitchToNextState(nextState);
        }


    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }


}
