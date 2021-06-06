using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDoor : MonoBehaviour
{
    /**
     * This script handles the effects of metal doors melting when hit with an acid shot.
     * -Grant Hargraves and Mikey Petersen
     */
    //=========================FIELDS=========================
    [Header("Fields")]
    [SerializeField] GameObject meltFX; //the acid steam effect that will be set active when the door is melted
    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    [SerializeField] AudioSource myAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip meltSFX; //the audio clip containing the "acid hiss" SFX
    //=========================METHODS=========================
    public void Melt()
    {
        StartCoroutine("Meltdown"); // starts the melt coroutine
    }

    IEnumerator Meltdown()
    {
        myAudio.clip = meltSFX; //set sound clip
        myAudio.Play(); //play sound clip
        meltFX.gameObject.SetActive(true);
        for (float fade = 80f; fade >= -0.1f; fade -= 0.1f) // runs a loop that makes the door slowly move down
        {
            gameObject.transform.position += new Vector3(0, -5f * Time.deltaTime, 0);
            if(fade >= 0.1f)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
        meltFX.gameObject.SetActive(false); //turn off acid steam effect
        gameObject.SetActive(false); //turn off this game object
    }
}
