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

    [Header("TARGET/PLAYER")]
    public GameObject playerTarget;
    //public float playerHealth;

    [Header("VISIBILITY CHECK")]
    //[SerializeField] bool visualizeVisabilityCone = true;
    public LayerMask visibilityMask;
    [Range(0f, 360f)]
    public float visibilityAngle = 100f;
    public Light FOVCone; //light source that shows enemy FOV
    public float maxDetectDistance = 10f; //max distance for visibility

    [Header("PATROL STATE")]
    public float patrolSpeed = 1f; // speed while moving between patrol points
    public int patrolIndex; // index number of the patrol array
    [SerializeField] private Transform patrolRoute; // holds multiple patrol points
    [HideInInspector] public Transform[] patrolPoints; // holds multiple patrol points
    public float distanceFromPatrol; // distance from the patrol point

    [Header("DISTANCE FROM TARGET")]
    public float distanceFromTarget;

    [Header("SEARCH STATE")]
    public float searchDuration; //speed(time) searching left&right

    [Header("CHASE STATE")]
    public float chaseSpeed = 2f; // speed while chasing the target
    public float rotateSpeed = 3f; // speed when rotating
    public float stoppingDistance = 6f; // minimum distance from player before stopping (to not run into player)

    [Header("ATTACK STATE")]
    public GameObject spawnPoint;
    public GameObject chargeUpAttack;
    public GameObject magicAttack; //weapon effect
    public int damageAmount; //amount of damage enemy does to player
    public float chargeTime = 3f;
    public float shootSpeed = 8f;
    public float rateOfFireMin = 3f;
    public float rateOfFireMax = 3f;

    //Keeps the enemy at a certain height. Make both min and max the same to keep enemy even - and make sure patrol points are at the same height.
    [Header("GROUND")]
    public GameObject terrain;
    public float minDistanceFromGround = 4f;
    public float maxDistanceFromGround = 4f;
    public float distanceFromGround; //checks the distance above the terrain

    //Inherits from State - each controls the individual state change of the enemy
    [HideInInspector] public ChargeUpAttackState chargeUpAttackState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public SearchState searchState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public DeathState deathState;

    [Header ("ANIMATION")]
    public Animator animator;

    public Rigidbody rb;

    public void OnValidate()
    {
        if (FOVCone != null)
        {
            FOVCone.spotAngle = visibilityAngle;
            FOVCone.range = maxDetectDistance;
        }
    }

    private void Start()
    {
        chargeUpAttackState = new ChargeUpAttackState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
        searchState = new SearchState(this);
        chaseState = new ChaseState(this);
        deathState = new DeathState(this);

        //Start in Patrol State.
        currentState = patrolState;

        //Allows the static variables of ProjectileMove to be changed by StateManager.
        ProjectileMove.speed = this.shootSpeed;
        ProjectileMove.chargeTime = this.chargeTime;
        ProjectileMove.weapon = this.magicAttack;
        ProjectileMove.spawnPoint = this.spawnPoint;

        //Find and set these objects at the start.
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        FOVCone = GetComponentInChildren<Light>();
        terrain = GameObject.FindGameObjectWithTag("Terrain");

        //Put the patrol points into a list.
        List<Transform> points = new List<Transform>();

        foreach (Transform potentialPatrolPoint in patrolRoute)
        {
            if (potentialPatrolPoint.GetComponent<PatrolPoint>())
            {
                points.Add(potentialPatrolPoint);
            }
        }

        patrolPoints = points.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        enemyHealth = GetComponent<EnemyHealth>().enemyHealth;
        distanceFromTarget = Vector3.Distance(transform.position, playerTarget.transform.position);
        distanceFromGround = Terrain.activeTerrain.SampleHeight(transform.position);

        // * If there is no Terrain object (like a plane instead):
        //distanceFromGround = Vector3.Distance(transform.position, terrain.transform.position);

        currentState.RunCurrentState();
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

    //Called during state changes.
    public virtual void ChangeState(State nextState)
    {
        currentState.OnStateExit();
        currentState = nextState;
        currentState.OnStateEnter();
    }
}
