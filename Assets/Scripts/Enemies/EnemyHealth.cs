using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;
    [SerializeField] Animator myAnim;

    //void Awake()
    //{
    //    myAnim = gameObject.GetComponent<StateManager>().animator;
    //}

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
        myAnim.SetBool("dead", true);
        Destroy(gameObject, 2f);
    }
                
}
