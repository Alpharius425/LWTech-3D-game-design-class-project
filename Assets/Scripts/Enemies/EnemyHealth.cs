using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;
    public StateManager stateManager;

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }
    public void DeductHealth(float damage)
    {
        Debug.Log("Got hit!");
      
        enemyHealth -= damage;

        if(enemyHealth <= 0)
        {
            StateManager.animator.SetBool("dead", true);
            //Turn gravity on so the enemy appears to fall when dying.
            StateManager.rb.useGravity = true;
            
            EnemyDead();
        }
    }

    void EnemyDead()
    {
        //Destroy the enemy including the parent.
        Destroy(StateManager.enemyPrefab, 4f);
    }
                
}
