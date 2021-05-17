using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("CONTROLS")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float rotateSpeed = 1f;
    //float offset = -90f; // for angle direction
    private Transform thisTransform;

    [Header("PATROL")]
    [SerializeField] GameObject[] patrolPoints; // holds multiple patrol points
    public int patrolIndex; // index number of the patrol array
    private float distanceFromPatrol;

    [Header("CHASE")]
    [SerializeField] Transform playerTarget;
    float distanceFromTarget;
    public float minDetectDistance; // minimum distance from player before chasing
    public float maxDetectDistance; // maximum distance from player before giving up the chase (getting from EnemyVisibility.cs)
    [SerializeField] bool canChasePlayer;

    private void Start()
    {
        patrolIndex = 0;
        //transform.LookAt(patrolPoints[patrolIndex].transform.position);
        thisTransform = transform;
        maxDetectDistance = GetComponent<EnemyVisibility>().maxDistance;
    }
    private void Update()
    {
        distanceFromPatrol = Vector3.Distance(transform.position, patrolPoints[patrolIndex].transform.position);

        //get target visibility
        canChasePlayer = GetComponent<EnemyVisibility>().targetIsVisible;
        
        if (canChasePlayer == false || playerTarget == null)
        {
            PatrolMovement();      
        }
        // If there's a target then chase.
        if (playerTarget != null && canChasePlayer == true)
        {
            // Find the distance from player
            distanceFromTarget = Vector3.Distance(playerTarget.position, transform.position);
            
            if(distanceFromTarget >= minDetectDistance)
            {
                ChasePlayer();
                // Attack();
            }
            else
            {
                // Attack();
            }
        }
    }

    private void LateUpdate()
    {
        // Find the player and store location in inspector.
        playerTarget = FindTarget();
    }

    // ROTATE
    void Rotate()
    {
        //get the distance between target and current position
        Vector3 direction = patrolPoints[patrolIndex].transform.position - thisTransform.position;
        //get the angle needed to turn to look at target
        Quaternion rotation = Quaternion.LookRotation(direction);
        //rotate and look at the target
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    // PATROLLED MOVEMENT
    void PatrolMovement()
    {
        //transform.LookAt(patrolPoints[patrolIndex].transform.position);
        Rotate();

        transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
        if (distanceFromPatrol < 0.5f)
        {
            IncreaseIndex();
        }
    }

    void IncreaseIndex()
    {
        patrolIndex++;
        if (patrolIndex >= patrolPoints.Length)
        {
            patrolIndex = 0;
        }
    }

    // CHASE TARGET
    void ChasePlayer()
    {
        //look at target
        //thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, Quaternion.LookRotation(transform.forward, playerTarget.position), speed * Time.deltaTime);

        Vector3 direction = playerTarget.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        // Move toward player position
        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, chaseSpeed * Time.deltaTime);

    }

    // When the player is out of the distance the player object is disconnected and normal AI resumes.
    Transform FindTarget()
    {
        if (playerTarget == null)
        {
            //canChasePlayer = false;
            // Find the player
            Transform _playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
            playerTarget = _playerTarget;

            if (_playerTarget != null)
            {
                // Find player if it is close enough
                if (canChasePlayer == true)
                {
                    return _playerTarget;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        else
            return playerTarget;     
    }

    // ATTACK TARGET
    void Attack()
    {
        //Shoot at target
        // Instantiate(bullet, bulletSpawnPoint, Quaternion.identity);

        //Do damage
        // playerHealth -= damageAmount;
    }

}
