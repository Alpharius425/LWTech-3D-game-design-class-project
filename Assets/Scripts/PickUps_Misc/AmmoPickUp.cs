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

    [SerializeField] GameObject myChild;

    [SerializeField] bool respawns = true;
    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    [SerializeField] AudioSource myAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip pickupSFX; //the audio clip containing the "pickup" SFX
    //=========================METHODS=========================
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && active == true) // checks if the player runs into the pickup and that it is active
        {
            myAudio.clip = pickupSFX; //set sound clip
            myAudio.Play(); //play sound clip
            col.GetComponent<Player_Stats>().IncreaseAmmo(ammoType, value); // gives the player ammo
            curTimeToRespawn = respawnTime; // resets the spawn timer
            active = false; // turns off the pickup
            myChild.SetActive(false); // turns off the nested game object so it looks like it disappeared
        }
    }

    private void Update()
    {
        curTimeToRespawn -= Time.deltaTime; // counts down the respawn timer

        distFromPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position); // calculates the distance from the player

        if(curTimeToRespawn <= 0 && active == false && distFromPlayer >= requiredDistanceFromPlayer && respawns == true) // checks if the timer has counted down, if the pickup is inactive and if the player is far enough away
        {
            Reactivate();
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // saves the player
    }

    void Reactivate() // turns the pickup back on and the renderer is reactivated
    {
        active = true;
        myChild.SetActive(true);
    }
}
