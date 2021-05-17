using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float shootSpeed;
    public GameObject projectileSpawnPoint;
    public GameObject projectilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        projectileSpawnPoint = GameObject.Find("BoltSpawn");
        //projectilePrefab = 
    }

    // Update is called once per frame
    void Update()
    {

        //Instantiate the bolt
        GameObject instProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        //Shoot the bolt
        //projectilePrefab.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, 0, shootSpeed));

        Rigidbody instProjectileRigidbody = instProjectile.GetComponent<Rigidbody>();
        instProjectileRigidbody.AddForce(Vector3.forward * shootSpeed * Time.deltaTime);
        Destroy(this.gameObject, 1);
    }
}
