using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AttackState : State
{
    [Header("STATE CONTROLS")]
    public StateManager stateManager;
    [Header("SHOOTING")]
    public GameObject projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float chargeTime;
    public float shootSpeed;
    public bool isAttacking = false;


    public override State RunCurrentState()
    {

        projectileSpawnPoint = GameObject.Find("BoltSpawn");
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
        Debug.Log("I have attacked!");
        return stateManager.chaseState;
    }

    IEnumerator Attack()
    {
        
        yield return new WaitForSeconds(chargeTime);

        //Instantiate the bolt
        GameObject instProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        //Shoot the bolt
        //projectilePrefab.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, 0, shootSpeed));
        Rigidbody instProjectileRigidbody = instProjectile.GetComponent<Rigidbody>();
        instProjectileRigidbody.velocity = instProjectileRigidbody.transform.forward * shootSpeed;
        //instProjectileRigidbody.AddRelativeForce(new Vector3(0, 0, shootSpeed));
        //instProjectileRigidbody.AddForce(Vector3.forward * (shootSpeed * Time.deltaTime));
        Destroy(instProjectile, 1);
        isAttacking = false;

    }
}
