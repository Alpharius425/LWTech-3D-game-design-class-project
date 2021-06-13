using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    /**
     * This script controls the opening of locked blast doors and the effects with them
     * -Grant Hargraves and Mikey Petersen
     */
    //=========================FIELDS=========================
    [SerializeField] bool unlocked = false; //controls whether the door is locked
    [SerializeField] GameObject lockedPadlock; //holds a reference to the locked padlock object
    [SerializeField] GameObject openPadlock; //holds a reference to the unlocked padlock object so it can fall to the ground
    [SerializeField] GameObject keyIcon; //reference to UI element
    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    [SerializeField] AudioSource myAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip blastdoorSFX; //the audio clip containing the "blast door" SFX
    //=========================METHODS=========================
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && unlocked == false) // checks if it detects the player
        {
            if(col.gameObject.GetComponent<Player_Stats>().hasKey == true) // checks if the player has the key
            {
                unlocked = true;
                StartCoroutine("OpenUp"); // starts opening the door
                Debug.Log("turning off");
                col.gameObject.GetComponent<Player_Stats>().hasKey = false; //uses up the key so others can be used with the same code
                keyIcon.SetActive(false);
            }
        }
    }

    IEnumerator OpenUp()
    {
        myAudio.clip = blastdoorSFX; //set sound clip
        myAudio.Play(); //play sound clip
        lockedPadlock.gameObject.SetActive(false); //turn off locked padlock object
        openPadlock.gameObject.SetActive(true); //turn on open padlock object
        for (float fade = 30f; fade >= -0.1f; fade -= 0.1f) // runs a loop that makes the door slowly move down
        {
            gameObject.transform.position += new Vector3(0, 5f * Time.deltaTime, 0);
            if (fade >= 0.1f)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
        openPadlock.gameObject.SetActive(false); //turn off the open padlock when the door is open
        gameObject.SetActive(false);
    }
}
