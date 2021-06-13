using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Video;

/// <summary>
/// Script that has Juggernaut waiting until it detects the Player. Gives chase once detected.
/// </summary>

public class JuggernautAI : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField] NavMeshAgent enemyAgent;
    public float enemyHealth = 100f;
    [Space(15)]
    Transform guardPost;
    //Transform destination;

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

    [Header("SOUND EFFECTS")]
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip mutteringIdle;
    [SerializeField] AudioClip mutteringHostile;
    [SerializeField] AudioClip mutteringBroken;
    private bool isHostile = false; //forces a sound effect to only change once

    [Header("VISUAL EFFECTS")]
    [SerializeField] GameObject deathEffects;

    [Space(20)]
    [SerializeField] Animator animator;

    [SerializeField] GameObject[] skinnedArmor;
    [SerializeField] GameObject[] fallingOffArmor;

    void Start()
    {
        enemyAgent = this.GetComponent<NavMeshAgent>();

        //Check that there is an agent.
        if (enemyAgent == null)
        {
            Debug.LogError("The NavMesh agent is missing for " + gameObject.name);
        }
        
        // * Use this if you want the enemy to return to a certain spot *
        //guardPost = this.transform; //Set the position where the enemy starts.
        //destination = guardPost; //Start destination at the guard post.

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
            //destination = player.transform;
            if (!isHostile)
            {
                myAudio.clip = mutteringHostile;
                myAudio.Play();
                isHostile = true;
            }

            animator.SetBool("chase", true);
            animator.SetBool("attack", false);
            animator.SetBool("idle", false);

            MoveToward();
            //enemyAgent.SetDestination(player.transform.position);

            if (distanceFromPlayer <= attackDistance)
            {
                animator.SetBool("attack", true);
                animator.SetBool("chase", false);
                animator.SetBool("idle", false);
            }
        }
        else
        {
            //destination = guardPost; //Return to guard post.

            animator.SetBool("idle", true);
            animator.SetBool("chase", false);
        }
    }
        
    void MoveToward()
    {
        Vector3 targetVector = player.transform.position;
        enemyAgent.SetDestination(targetVector);
    }

    public void Died()
    {
        myAudio.clip = mutteringBroken;
        myAudio.Play();
        Instantiate(deathEffects);
        animator.SetBool("death", true);
        Destroy(gameObject, 2f);
    }

    public void BreakArmor()
    {
        //Debug.Log("Armor Broke!!!");

        for (int i = 0; i < skinnedArmor.Length; i++)
        {
            skinnedArmor[i].SetActive(false);
            fallingOffArmor[i].SetActive(true);

            fallingOffArmor[i].transform.SetParent(null, true);

        }

        // Set vfx
    }
}
