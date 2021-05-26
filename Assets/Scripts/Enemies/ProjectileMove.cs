using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public static float speed;
    public static float chargeTime;
    public static GameObject weapon;
    public static GameObject spawnPoint;

    public void Update()
    {
        AddVelocity();
    }

    public void AddVelocity()
    {
        Debug.Log("Speed " + speed);
        transform.position = Vector3.MoveTowards(transform.position, AttackState.targetLastPos.position, (speed * Time.deltaTime));
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Walls")
        { 
            Destroy(gameObject, 1f);  
        }
    }
}
