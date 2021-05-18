using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shotgun : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Mayfly"))
        {
            col.GetComponent<EnemyHealth>().DeductHealth(damage);
        }
        gameObject.SetActive(false); // turns this back off
    }
}
