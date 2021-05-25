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
    public float rotateSpeed = 5f; // speed when rotating
    public float stoppingDistance = 4f; // minimum distance from player before stopping (to not run into player)
    public float maxDetectDistance; //max distance for visibility

    [Header("ATTACK STATE")]
    public GameObject spawnPoint;
    public GameObject chargeUpAttack;
    public GameObject magicAttack; //weapon effect
    public int damageAmount; //amount of damage enemy does to player
    public float chargeTime = 3f;
    public float shootSpeed = 50f;
    //public float startAttackDistance; //distance when enemy begins attack
    public float shootDistance; //how far the attack can reach the player

    [Header("GROUND")]
    public GameObject terrain;
    public float minDistanceFromGround = 2f;
    public float maxDistanceFromGround = 2f;
    public float distanceFromGround; // keep drone from lowering

    [Header("CONTROL BOOLS")]
    public bool canSeePlayer;
    //public bool isInAttackRange;
    public bool isAttacking;
    public bool search, patrol;

    [Header("STATE SCRIPTS")]
    public AttackState attackState;
    public PatrolState patrolState;
    public SearchState searchState;
    public ChaseState chaseState;

    [Header ("ANIMATION")]
    public Animator animator;


    private void Start()
    {
        //initialize
        isAttacking = false;
        patrol = false;

        ProjectileMove.speed = shootSpeed;
        ProjectileMove.chargeTime = chargeTime;
        ProjectileMove.weapon = this.magicAttack;
        ProjectileMove.spawnPoint = this.spawnPoint;

        playerTarget = GameObject.FindGameObjectWithTag("Player");
        FOVCone = GetComponentInChildren<Light>();
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        //projectileprefab = gameobject.findgameobjectwithtag("projectile");
        //projectilePrefab = GameObject.FindGameObjectWithTag("projectile");
        terrain = GameObject.FindGameObjectWithTag("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        //update variables
        animator = GetComponent<Animator>();
        lookLeft = GameObject.Find("LookLeft");
        lookRight = GameObject.Find("LookRight");
        search = SearchState.search;

        canSeePlayer = GetComponentInChildren<EnemyVisibility>().TargetIsVisible;
        enemyHealth = GetComponent<EnemyHealth>().enemyHealth;
        distanceFromTarget = Vector3.Distance(transform.position, playerTarget.transform.position);
        distanceFromGround = Terrain.activeTerrain.SampleHeight(transform.position);
        //isInAttackRange = (distanceFromTarget <= startAttackDistance);

        //ANIMATION BOOLS
        if (canSeePlayer)
        {
            animator.SetBool("chasing", true);
            animator.SetBool("dead", false);
            animator.SetBool("patrol", false);
            patrol = false;
        }
        else if (currentState == patrolState)
        {
            animator.SetBool("patrol", true);
            animator.SetBool("dead", false);
            animator.SetBool("chasing", false);
            patrol = true;
        }
        else if (enemyHealth <= 0)
        {
            animator.SetBool("dead", true);
            animator.SetBool("chasing", false);
            animator.SetBool("patrol", true);
            patrol = false;
        }
        else
        {
            animator.SetBool("dead", false);
            animator.SetBool("patrol", false);
            animator.SetBool("chasing", false);
            patrol = false;
        }
            RunStateMachine();
    }

    private void FixedUpdate()
    {
        HeightCheck();
    }

    void HeightCheck()
    {
        //Keep a certain distance above ground.
        if (distanceFromGround < minDistanceFromGround)
        {
            Vector3 pos = transform.position;
            pos.y = minDistanceFromGround;
            //pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            //Debug.Log("pos.y " + pos.y);
            transform.position = pos;
        }
        else if (distanceFromGround > maxDistanceFromGround)
        {
            Vector3 pos = transform.position;
            pos.y = maxDistanceFromGround;
            //pos.y = Terrain.activeTerrain.SampleHeight(transform.position) - 1.5f;
            //Debug.Log("pos.y " + pos.y);
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
