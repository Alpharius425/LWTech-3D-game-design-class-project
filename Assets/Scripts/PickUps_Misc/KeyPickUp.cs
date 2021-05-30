using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player") && col.gameObject.GetComponent<Player_Stats>().hasKey == false)
        {
            col.GetComponent<Player_Stats>().PickUpKey();
            gameObject.SetActive(false);
        }
    }
}
