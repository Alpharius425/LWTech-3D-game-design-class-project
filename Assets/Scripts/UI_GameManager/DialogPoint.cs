using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPoint : MonoBehaviour
{
    /**
     * This script is used to control where and what dialog appears in the game.
     * Each dialog point is modular and can be changed individually.
     * Can be attached to any object with a trigger collider and rigidbody.
     * -Grant Hargraves
     */
    //=========================FIELDS=========================
    [Header("Dialog")]
    [SerializeField] string spokenDialog; //what the main character speaks when touching this point
    [SerializeField] string controlDialog; //the tutorial message accompanying the spoken text, if any
    [Header("Variables")]
    [SerializeField] Animator textAnimator; //The animator that makes the text fade in and out
    [SerializeField] Text spokenText; //the text object holding the spoken text displayed to the player
    [SerializeField] Text controlText; //the text object holding the tutorial text displayed to the player
    private bool hasRead = false; //keeps the player from seeing this text multiple times
    //=========================BASIC METHODS=========================
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && !hasRead) //if the player touches this point and has not already seen this dialog...
        {
            hasRead = true; //makes sure the player can only trigger this dialog once
            spokenText.text = spokenDialog; //sets the spoken text to the desired dialog
            controlText.text = controlDialog; //sets the tutorial text to the desired dialog
            textAnimator.SetTrigger("display"); //tells the animator to show the dialog
        }
    }
}
