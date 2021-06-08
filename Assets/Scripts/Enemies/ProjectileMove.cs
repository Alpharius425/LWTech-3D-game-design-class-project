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
    public int damage;
    Vector3 aimAt;

    private void Start()
    {
        aimAt = AttackState.targetLastPos.position;
    }

    public void Update()
    {
        AddVelocity();
    }

    public void AddVelocity()
    {
        //Debug.Log("Speed " + speed);
        transform.position = Vector3.MoveTowards(transform.position, aimAt, (speed * Time.deltaTime));
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Stats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Walls")
        { 
            Destroy(gameObject);  
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}
