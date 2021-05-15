using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<Player_Stats>().PickUpKey();
            Debug.Log("Grabbed key");
            gameObject.SetActive(false);
        }
    }
}
