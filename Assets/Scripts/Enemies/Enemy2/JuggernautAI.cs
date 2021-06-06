using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Video;

/// <summary>
/// Script that has Juggernaut waiting until it detects the Player. Gives chase once detected.
/// Patrol options currently commented out. Uncomment if patrol is desired.
/// </summary>

public class JuggernautAI : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField] NavMeshAgent enemyAgent;
    public float enemyHealth = 100f;
    [Space(15)]
    [SerializeField] Transform guardPost;
    [SerializeField] Transform destination;
    [HideInInspector] public bool hasArmor;

    [Header("PLAYER")]
    [SerializeField] GameObject player;
    public float playerHealth;
    [SerializeField] float distanceFromPlayer; //Distance to the player

    [Header("ATTACK")]
    public int damageAmount = 5;
    public float attackDistance; //Distance to begin attack
    [Range(0f, 360f)]
    public float visibilityAngle = 100.0f;
    public float maxDetectDistance = 10.0f;

    [Space(20)]
    [SerializeField] Animator animator;


    void Start()
    {
        hasArmor = true; //Enemy is wearing armor.

        enemyAgent = this.GetComponent<NavMeshAgent>();

        guardPost = this.transform; //Set the position where the enemy starts.
        destination = guardPost; //Start destination at the guard post.

        player = GameObject.FindGameObjectWithTag("Player");
        
        animator = GetComponent<Animator>();
        
        damageAmount = GetComponentInChildren<DoDamage>().damageAmount;
    }

    private void Update()
    {
        playerHealth = player.GetComponent<Player_Stats>().curHealth;

        //Check the distance to the player
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        //Set the destination
        if (distanceFromPlayer < maxDetectDistance)
        {
            destination = player.transform;

            animator.SetBool("chase", true);
            animator.SetBool("attack", false);
            animator.SetBool("idle", false);

            MoveToward();

            if (distanceFromPlayer <= attackDistance)
            {
                animator.SetBool("attack", true);
                animator.SetBool("chase", false);
                animator.SetBool("idle", false);
            }
        }
        else
        {
            destination = guardPost; //Return to guard post.

            animator.SetBool("idle", true);
            animator.SetBool("chase", false);
        }

        //Check that there is an agent.
        if (enemyAgent == null)
        {
            Debug.LogError("The NavMesh agent is missing for " + gameObject.name);
        }
   
        Debug.Log("Destination: " + destination);

    }

    public void TakeDamage (int amount)
    {
        //Take no damage if projectile is non acid and has armor.
        
        //If hit by acid - make armor disappear
        hasArmor = false;

        //Take damage by any projectile
        enemyHealth -= amount;

        if (enemyHealth <= 0)
        {
            animator.SetBool("death", true);
            Destroy(gameObject, 3f);
        }
    }
        
    void MoveToward()
    {
        Vector3 targetVector = destination.transform.position;
        enemyAgent.SetDestination(targetVector);
    }
}
