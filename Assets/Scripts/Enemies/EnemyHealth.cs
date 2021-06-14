using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public UnityEvent died;
    bool hasDied = false;
    public float curHealth = 100f;
    //=========================DAMAGE EFFECTS=========================
    [Header("Damage Effects")]
    public GameObject damageEffect; //the damage effect gameobject that will be spawned when taking damage
    public AudioSource myAudio; //the audiosource used to play damage sound effects
    public AudioClip damageSound; //the audio clip to play upon taking damage

    public virtual void DeductHealth(float damage, AmmoType ammoType = AmmoType.gas)
    {
        curHealth -= damage;
        //-----Effects start-----
        Instantiate(damageEffect, gameObject.transform.position, gameObject.transform.rotation); //instantiate a particle effect for damage
        myAudio.PlayOneShot(damageSound); //play the damage clip provided
        //-----Effects end-----
        if (curHealth < 0)
        {
            curHealth = 0;
        }

        if (!hasDied && curHealth == 0)
        {
            died.Invoke();
            hasDied = true;
        }
    }
}
