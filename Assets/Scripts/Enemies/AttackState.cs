using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class AttackState : State
{
    public StateManager stateManager;
    private int damageAmount;
    public GameObject player;
    [HideInInspector]public static GameObject magicAttack;
    [HideInInspector]public static GameObject spawnPoint;
    [HideInInspector]public static GameObject chargeUpAttack;
    public static GameObject charge;
    public static Transform targetLastPos;

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
        //stateManager.projectileSpawnPoint = GameObject.FindGameObjectWithTag("ProjectileSpawn");

        if (!stateManager.isAttacking)
        {
            stateManager.isAttacking = true;
            StartCoroutine(ChargeUpAttack());
            //StartCoroutine(ShootMagic());
            //ChargeUpAttack();
        }
        //Debug.Log("Enemy attacked!");
        return stateManager.chaseState;
    }

    //Enemy charging attack
   IEnumerator ChargeUpAttack()
    {
        Debug.Log("attack state1");

        //#1 Charge up attack
        //instantiate the charge fx
        yield return new WaitForSeconds(Random.Range(1, 4));
        charge = Instantiate(chargeUpAttack, stateManager.spawnPoint.transform.position, Quaternion.identity);
        charge.transform.parent = stateManager.transform; //attach the prefab as a child so it will follow

        Destroy(charge, 3f);
        //yield return new WaitForSeconds(stateManager.chargeTime);
        //ShootMagic();

        //Shoot
        StartCoroutine(ShootMagic());
        //yield return null;
    }

    //Enemy shoot magic bolt
    IEnumerator ShootMagic()
    {
        Debug.Log("attack state2");
        //#2 Fire the charged bolt
        //Vector3 shoot = stateManager.playerTarget.transform.TransformDirection(Vector3.forward);
        //Vector3 shoot2 = stateManager.transform.position - targetLastPos.position;
        //Vector3 shoot3 = targetLastPos.TransformDirection(Vector3.forward);

        RaycastHit hit;

        if (Physics.Raycast(magicAttack.transform.position, targetLastPos.position, out hit, stateManager.shootDistance))
        {
            Debug.Log("FIRING");
            yield return new WaitForSeconds(stateManager.chargeTime);

            GameObject projectile;
            projectile = Instantiate(magicAttack, stateManager.spawnPoint.transform.position, stateManager.spawnPoint.transform.rotation);

            projectile.GetComponent<ProjectileMove>().damage = damageAmount;
            //player.GetComponent<Player_Stats>().TakeDamage(damageAmount);
            //Debug.Log("Enemy did damage for " + damageAmount);
            Debug.Log(hit.transform.name);
            if (hit.collider.CompareTag("Player"))
            {
                //hit.collider.GetComponent<Player_Stats>().TakeDamage(stateManager.damageAmount);
                yield return null;

            }
        }

        stateManager.isAttacking = false;
        yield return null;
    }

    //void ShootMagic()
    //{
    //    //#2 Fire the charged bolt
    //    Vector3 shoot = stateManager.transform.TransformDirection(Vector3.forward);
    //    RaycastHit hit;

    //    if (Physics.Raycast(stateManager.transform.position, shoot, out hit, stateManager.shootDistance))
    //    {

    //        Instantiate(magicAttack, stateManager.spawnPoint.transform.position, stateManager.spawnPoint.transform.rotation);
    //        //Instantiate(stateManager.weapon, stateManager.spawnPoint.transform.position, stateManager.spawnPoint.transform.rotation);
    //        //stateManager.projectilePrefab.GetComponent<ProjectileMove>().ChargeFire();

    //        player.GetComponent<Player_Stats>().TakeDamage(damageAmount);
    //        Debug.Log("Enemy did damage for " + damageAmount);
    //        Debug.Log(hit.transform.name);
    //        if (hit.collider.CompareTag("Player"))
    //        {
    //            hit.collider.GetComponent<Player_Stats>().TakeDamage(stateManager.damageAmount);

    //        }
    //    }

    //    stateManager.isAttacking = false;
    //}
}
