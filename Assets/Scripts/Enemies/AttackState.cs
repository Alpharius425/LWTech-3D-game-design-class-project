using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class AttackState : State
{
    public StateManager stateManager;
    public GameObject player;
    public static GameObject charge;
    public static Transform targetLastPos;
    private int damageAmount;
    private State returnState;
    [HideInInspector] public static GameObject magicAttack;
    [HideInInspector] public static GameObject spawnPoint;
    [HideInInspector] public static GameObject chargeUpAttack;

    private void Start()
    {
        magicAttack = stateManager.magicAttack;
        spawnPoint = stateManager.spawnPoint;
        chargeUpAttack = stateManager.chargeUpAttack;
        stateManager.isAttacking = false;
    }

    private void Update()
    {
        //initialize player & player health from the player script
        player = stateManager.playerTarget;
        damageAmount = stateManager.damageAmount;
        targetLastPos = stateManager.playerTarget.transform;
    }

    public override State RunCurrentState()
    {
        if (!stateManager.isAttacking)
        {
            stateManager.isAttacking = true;
            StartCoroutine(ChargeUpAttack());

        }
        //Debug.Log("Enemy attacked!");
        return stateManager.chaseState;
    }

    //Enemy charging attack
   IEnumerator ChargeUpAttack()
    {
        //Debug.Log("attack state1");

        //Stop coroutine if enemy can't see player.
        if (!stateManager.canSeePlayer)
        {
            stateManager.isAttacking = false;
            StopCoroutine(ChargeUpAttack());
            ChangeState(stateManager.chaseState);
        }

        //#1 Charge up attack
        //instantiate the charge fx
        yield return new WaitForSeconds(Random.Range(stateManager.rateOfFireMin, stateManager.rateOfFireMax));
        charge = Instantiate(chargeUpAttack, stateManager.spawnPoint.transform.position, Quaternion.identity);
        charge.transform.parent = stateManager.spawnPoint.transform; //attach the prefab as a child so it will follow

        Destroy(charge, stateManager.chargeTime);

        //Shoot
        StartCoroutine(ShootMagic());
    }

    //Enemy shoot magic bolt
    IEnumerator ShootMagic()
    {
        //Debug.Log("attack state2");

        //Stop coroutine if enemy can't see player.
        if (!stateManager.canSeePlayer)
        {
            stateManager.isAttacking = false;
            //StopCoroutine(ShootMagic());
            StopAllCoroutines();
            ChangeState(stateManager.chaseState);
        }
        else
        {
            //#2 Fire the charged bolt
            //RaycastHit hit;
            Instantiate(magicAttack, stateManager.spawnPoint.transform.position, Quaternion.identity);

            player.GetComponent<Player_Stats>().TakeDamage(damageAmount);
            //Debug.Log("Enemy did damage for " + damageAmount);
            //Debug.Log(hit.transform.name);
            if (CompareTag("Player"))
            {
                GetComponent<Player_Stats>().TakeDamage(stateManager.damageAmount);
                yield return null;
            }
        }

        stateManager.isAttacking = false;
        yield return null;
    }

    public void ChangeState(State state)
    {
        returnState = state;
    }
}
