using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{

    Renderer myRenderer; // saves our renderer so its easy to change
    [SerializeField] bool unlocked = false;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>(); // gets our renderer
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && unlocked == false) // checks if it detects the player
        {
            if(col.gameObject.GetComponent<Player_Stats>().hasKey == true) // checks if the player has the key
            {
                unlocked = true;
                StartCoroutine("FadeOut"); // starts the fade away
                Debug.Log("turning off");
            }
        }
    }

    IEnumerator FadeOut()
    {
        for (float fade = 1f; fade >= -0.1f; fade -= 0.1f) // runs a loop that makes the door slowly become transparent
        {
            Color newColor = myRenderer.material.color; // sets the color to same as our base color
            newColor.a = fade; // sets the alpha of the new color to be more transparent
            myRenderer.material.color = newColor; // resets the color of the sprite to be transparent

            if (fade >= 0.1f)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }

        gameObject.SetActive(false);
    }
}
