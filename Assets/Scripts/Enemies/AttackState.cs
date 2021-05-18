using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AttackState : State
{
    [Header("STATE CONTROLS")]
    public StateManager stateManager;
    
    public override State RunCurrentState()
    {

        stateManager.projectileSpawnPoint = GameObject.Find("BoltSpawn");
        if (!stateManager.isAttacking)
        {
            stateManager.isAttacking = true;
            StartCoroutine(Attack());
        }
        //Debug.Log("Enemy attacked!");
        return stateManager.chaseState;
    }

    IEnumerator Attack()
    {
        
        yield return new WaitForSeconds(stateManager.chargeTime);

        //Instantiate the bolt
        GameObject instProjectile = Instantiate(stateManager.projectilePrefab, stateManager.projectileSpawnPoint.transform.position, stateManager.projectileSpawnPoint.transform.rotation);
       
        Vector3 shootDirection = (stateManager.projectileSpawnPoint.transform.position - stateManager.thisEnemy.transform.position).normalized;
        instProjectile.GetComponent<Projectile>().Setup(shootDirection);

        Destroy(instProjectile, 1f);

        stateManager.isAttacking = false;
    }
}
