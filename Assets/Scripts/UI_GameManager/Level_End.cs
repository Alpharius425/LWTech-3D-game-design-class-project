using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_End : MonoBehaviour
{
    /**
     * A clunky little script used to trigger the end of our demo level
     */
    //=========================FIELDS=========================
    [SerializeField] Animator whiteFadePanel; //animator object attached to white fade panel
    [SerializeField] int startSceneIndex = 0; //reference to where the start scene is in the build settings
    //=========================METHODS=========================
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player") //when the player enters this area...
        {
            whiteFadePanel.SetTrigger("on"); //tell the animator to start fading out.
            StartCoroutine("FadeToWhite"); //start the coroutine to wait until the animation is finished
        }
    }

    private IEnumerator FadeToWhite()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(startSceneIndex);
    }
}
