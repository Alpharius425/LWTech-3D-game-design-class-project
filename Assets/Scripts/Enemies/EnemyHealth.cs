using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    public void DeductHealth(float damage)
    {
        enemyHealth -= damage;

        if(enemyHealth <= 0)
        {
            EnemyDead();
        }
    }
   

    void EnemyDead()
    {
        Destroy(gameObject, 1f);
    }
                
}
