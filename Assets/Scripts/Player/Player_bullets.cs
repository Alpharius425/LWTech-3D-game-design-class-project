using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_bullets : MonoBehaviour
{

    [SerializeField] AmmoType ammoType; // what kind of bullet this is
    [SerializeField] int damage; // how much damage this bullet does
    [SerializeField] float lifeSpan; // how long this bullet lives before being turned off
    [SerializeField] float speed; // how fast the bullet is
    [SerializeField] Rigidbody rb; // lets us affect the bullet with physics

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed; // causes the bullet to move forward

        Invoke("DestroyPooled", lifeSpan); // will deactivate the object if it doesn't hit anything for however long its lifespan is
    }

    private void DestroyPooled()
    {
        if (gameObject.activeInHierarchy) // disables the gameobject returning it to the pool
        {
            gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        CancelInvoke(); // stops the disabling invoke just in case its still going
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy") == true)
        {
            // placeholder for doing damage to the enemy
        }

        gameObject.SetActive(false);
    }
}
