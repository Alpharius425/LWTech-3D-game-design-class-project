using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas_Trail : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag ("Enemy"))
        {
            Debug.Log(col.transform.name); // placeholder for enemy attack
        }
    }
}
