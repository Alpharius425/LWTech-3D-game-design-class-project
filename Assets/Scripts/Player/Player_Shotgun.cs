using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shotgun : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy")) // did we hit an enemy?
        {
            //deal damage
        }
        gameObject.SetActive(false); // turns this back off
    }
}
