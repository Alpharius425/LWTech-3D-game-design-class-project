using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// COTROL ALL STATES THROUGH ONE SCRIPT //

public class StateManager : MonoBehaviour
{
    [Header("CURRENT STATE")]
    public State currentState;

    [Header("ENEMY")]
    //public Transform thisEnemy;
    public static GameObject enemyPrefab;
    public float enemyHealth;
    public Light FOVCone; //light source that shows enemy FOV

    [Header("TARGET/PLAYER")]
    public GameObject playerTarget;
    //public float playerHealth;

    [Header("PATROL STATE")]
    public float patrolSpeed = 1f; // speed while moving between patrol points
    public int patrolIndex; // index number of the patrol array
    public GameObject[] patrolPoints; // holds multiple patrol points
    public float distanceFromPatrol; // distance from the patrol point

    [Header("DISTANCE FROM TARGET")]
    public float distanceFromTarget;

    [Header("SEARCH STATE")]
    public float searchDuration; //speed(time) searching left&right
    public GameObject lookLeft, lookRight;

    [Header("CHASE STATE")]
    public float chaseSpeed = 2f; // speed while chasing the target
    public float rotateSpeed = 3f; // speed when rotating
    public float stoppingDistance = 6f; // minimum distance from player before stopping (to not run into player)
    public float maxDetectDistance = 10f; //max distance for visibility

    [Header("ATTACK STATE")]
    public GameObject spawnPoint;
    public GameObject chargeUpAttack;
    public GameObject magicAttack; //weapon effect
    public int damageAmount; //amount of damage enemy does to player
    public float chargeTime = 3f;
    public float shootSpeed = 8f;
    public float rateOfFireMin = 3f;
    public float rateOfFireMax = 3f;
    //public float startAttackDistance; //distance when enemy begins attack
    //public float shootDistance; //how far the attack can reach the player

    //Keeps the enemy at a certain height. Make both min and max the same to keep enemy even - and make sure patrol points are at the same height.
    [Header("GROUND")]
    public GameObject terrain;
    public float minDistanceFromGround = 4f;
    public float maxDistanceFromGround = 4f;
    public float distanceFromGround; //checks the distance above the terrain

    [Header("CONTROL BOOLS")]
    public bool canSeePlayer;
    //public bool isInAttackRange;
    public bool isAttacking;
    public bool search, patrol;

    //Inherits from State - each controls the individual state change of the enemy
    [Header("STATE SCRIPTS")]
    public AttackState attackState;
    public PatrolState patrolState;
    public SearchState searchState;
    public ChaseState chaseState;

    [Header ("ANIMATION")]
    public static Animator animator;

    public static Rigidbody rb;


    private void Start()
    {
        //initialize
        isAttacking = false;
        patrol = false;

        //Allows the static variables of ProjectileMove to be changed by StateManager.
        ProjectileMove.speed = this.shootSpeed;
        ProjectileMove.chargeTime = this.chargeTime;
        ProjectileMove.weapon = this.magicAttack;
        ProjectileMove.spawnPoint = this.spawnPoint;
        SearchState.canSeePlayer = this.canSeePlayer;

        //Find and set these objects at the start.
        rb = GetComponent<Rigidbody>();
        enemyPrefab = GameObject.FindGameObjectWithTag("Enemy1");
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        FOVCone = GetComponentInChildren<Light>();
        //spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint"); //if more than one enemy this may grab the wrong SpawnPoint
        terrain = GameObject.FindGameObjectWithTag("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        //update variables
        animator = GetComponent<Animator>();
        lookLeft = GameObject.Find("LookLeft");
        lookRight = GameObject.Find("LookRight");
        //search = SearchState.search;

        canSeePlayer = GetComponentInChildren<EnemyVisibility>().TargetIsVisible;
        enemyHealth = GetComponent<EnemyHealth>().enemyHealth;
        distanceFromTarget = Vector3.Distance(transform.position, playerTarget.transform.position);
        //distanceFromGround = Terrain.activeTerrain.SampleHeight(transform.position);

        //If there is no Terrain
        distanceFromGround = Vector3.Distance(transform.position, terrain.transform.position);

        //ANIMATION BOOLS
        if (canSeePlayer)
        {
            animator.SetBool("chasing", true);
            animator.SetBool("dead", false);
            animator.SetBool("patrol", false);
        }
        else if (currentState == patrolState)
        {
            animator.SetBool("patrol", true);
            animator.SetBool("dead", false);
            animator.SetBool("chasing", false);
        }
        else if (enemyHealth <= 0)
        {
            animator.SetBool("dead", true);
            animator.SetBool("chasing", false);
            animator.SetBool("patrol", true);
        }
        else
        {
            animator.SetBool("dead", false);
            animator.SetBool("patrol", false);
            animator.SetBool("chasing", false);
        }

        RunStateMachine();
    }



    private void FixedUpdate()
    {
        HeightCheck();
    }

    //Keeps enemy at the min and max height. Keep both variables the same if no change in height is wanted.
    void HeightCheck()
    {
        //Keep a certain distance above ground.
        if (distanceFromGround < minDistanceFromGround)
        {
            Vector3 pos = transform.position;
            pos.y = minDistanceFromGround;

            transform.position = pos;
        }
        else if (distanceFromGround > maxDistanceFromGround)
        {
            Vector3 pos = transform.position;
            pos.y = maxDistanceFromGround;

            transform.position = pos;
        }
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            //Switch to the next state
            SwitchToNextState(nextState);
        }
        else
        {
            SwitchToNextState(patrolState);
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }
}
