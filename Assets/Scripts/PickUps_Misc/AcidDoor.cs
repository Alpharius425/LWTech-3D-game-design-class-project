using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDoor : MonoBehaviour
{
    Renderer myRenderer;
    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    [SerializeField] AudioSource myAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip meltSFX; //the audio clip containing the "acid hiss" SFX
    //=========================METHODS=========================
    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }
    public void Melt()
    {
        StartCoroutine("FadeOut"); // starts the fade away
    }

    IEnumerator FadeOut()
    {
        myAudio.clip = meltSFX; //set sound clip
        myAudio.Play(); //play sound clip
        for (float fade = 1f; fade >= -0.1f; fade -= 0.1f) // runs a loop that makes the door slowly become transparent
        {
            Color newColor = myRenderer.material.color; // sets the color to same as our base color
            newColor.a = fade; // sets the alpha of the new color to be more transparent
            myRenderer.material.color = newColor; // resets the color of the sprite to be transparent

            if(fade >= 0.1f)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        gameObject.SetActive(false);
    }
}
