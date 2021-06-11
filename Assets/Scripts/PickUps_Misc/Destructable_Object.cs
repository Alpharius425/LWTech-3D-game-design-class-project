using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Object : MonoBehaviour
{
    [SerializeField] GameObject destroyedObject; // object we will replace this gameobject with when we destroy it
    bool isDestroyed = false;

    public void DestroyObject() // called when something has hit the gameobject
    {
        if(isDestroyed == false)
        {
            Instantiate(destroyedObject, gameObject.transform.position, gameObject.transform.rotation); // creates the destroyed object
            gameObject.SetActive(false); // removes the intact object
            isDestroyed = true;
        }
    }
}
