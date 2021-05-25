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
    Vector3 aimAt;

    public int damage;

    private void Start()
    {
        aimAt = AttackState.targetLastPos.position;
    }

    void Update()
    {
        //StartCoroutine(ChargeFire());
        AddVelocity();
    }

    public void AddVelocity()
    {
        Debug.Log("Speed " + speed);
        transform.position = Vector3.MoveTowards(transform.position, aimAt, (speed * Time.deltaTime));
    }


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player_Stats>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    //IEnumerator ChargeFire()
    //{
    //    yield return new WaitForSeconds(chargeTime);

    //    //Instantiate(weapon, spawnPoint.transform.position, spawnPoint.transform.rotation);
    //    //transform.position += transform.forward * (speed * Time.deltaTime);
    //    ShootMagic();
    //    yield return null;
    //}

    //void ShootMagic()
    //{
    //    if (speed != 0)
    //    {

    //        if (chargeTime < timer)
    //        {

    //            transform.position += transform.forward * (speed * Time.deltaTime);
    //            timer = 0;
    //        }
    //        else
    //        {
    //            timer += 1;

    //        }
    //    }
    //}
}
