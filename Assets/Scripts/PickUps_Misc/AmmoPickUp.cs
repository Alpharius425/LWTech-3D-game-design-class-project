using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{

    [SerializeField] AmmoType ammoType; // what kind of ammo the pickup gives
    [SerializeField] int value; // how much ammo the pickup gives
    [SerializeField] bool active = true; // has the pickup been used recently

    [SerializeField] float respawnTime; // max time between respawns
    [SerializeField] float curTimeToRespawn; // how long until it respawns next
    [SerializeField] GameObject player; // reference for the player
    [SerializeField] float requiredDistanceFromPlayer; // how far away the player needs to be
    [SerializeField] float distFromPlayer; // the actual distance from the player

    Renderer myRender; // allows us to reference the renderer easily

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && active == true) // checks if the player runs into the pickup and that it is active
        {
            col.GetComponent<Player_Stats>().IncreaseAmmo(ammoType, value); // gives the player ammo
            curTimeToRespawn = respawnTime; // resets the spawn timer
            active = false; // turns off the pickup
            myRender.enabled = false; // turns off the renderer so it looks like it disappeared
        }
    }

    private void Update()
    {
        curTimeToRespawn -= Time.deltaTime; // counts down the respawn timer

        distFromPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position); // calculates the distance from the player

        if(curTimeToRespawn <= 0 && active == false && distFromPlayer >= requiredDistanceFromPlayer) // checks if the timer has counted down, if the pickup is inactive and if the player is far enough away
        {
            Reactivate();
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // saves the player
        myRender = gameObject.GetComponent<MeshRenderer>(); // saves our renderer
    }

    void Reactivate() // turns the pickup back on and the renderer is reactivated
    {
        active = true;
        myRender.enabled = true;
    }
}
