using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{

    [SerializeField] AmmoType ammoType;
    [SerializeField] int value;

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<Player_Stats>().IncreaseAmmo(ammoType, value);
            gameObject.SetActive(false);
        }
    }
}
