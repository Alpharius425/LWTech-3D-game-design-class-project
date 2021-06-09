using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour
{
    /*
     * A basic utility script used for playing sounds, usually on a delay.
     * -Grant Hargraves
     */
    //==========================FIELDS==========================
    [SerializeField] AudioSource myAudioSource; //reference to the audio source to play this clip
    [SerializeField] AudioClip myAudioClip; //reference to the desired audio clip to play
    [SerializeField] float delayTime = 0f; //amount of time to wait before deleting self
    //==========================METHODS==========================
    void Start()
    {
            StartCoroutine(playTimer());
    }

    /*
     * The following method is for creating a delay in the time the sound is played.
     */
    private IEnumerator playTimer()
    {
        yield return new WaitForSeconds(delayTime);
        myAudioSource.clip = myAudioClip;
        myAudioSource.Play();
    }


}
