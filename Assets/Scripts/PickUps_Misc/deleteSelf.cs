using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteSelf : MonoBehaviour
{
    /*
     * A basic utility script used for objects to delete themselves if nothing else tells them to.
     * -Grant Hargraves
     */
    //==========================FIELDS==========================
    [SerializeField] bool disableInstead = false; //decides whether to delete or disable the object
    [SerializeField] int deleteTime = 0; //amount of time to wait before deleting self
    //==========================METHODS==========================
    void Start()
    {
        if(! disableInstead)
        {
            Destroy(gameObject, deleteTime);
        }
        else
        {
            StartCoroutine(disableTimer());
        }
    }

    /*
     * The following method is for use with disabling, since that method does not have a
     * built-in timer.
     */
    private IEnumerator disableTimer()
    {
        yield return new WaitForSeconds(deleteTime);
        gameObject.SetActive(false);
    }


}
