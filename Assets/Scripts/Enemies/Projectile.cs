using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public StateManager stateManager;
    public GameObject player;
    private Vector3 shootDirection;
    [HideInInspector] public static float shootSpeed;
    [HideInInspector] public float playerHealth;
    [HideInInspector] public static float damageAmount;


    // Start is called before the first frame update
    void Awake()
    {
        //stateManager = enemy.GetComponent<StateManager>();
        Debug.Log("Projectile shootSpeed: " + shootSpeed);
    }

    public void Setup(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerHealth -= damageAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get the player health from the player script
        //playerHealth = player.GetComponent<PlayerScript>().playerHealth;

        transform.position += shootDirection * shootSpeed * Time.deltaTime;
    }
}
