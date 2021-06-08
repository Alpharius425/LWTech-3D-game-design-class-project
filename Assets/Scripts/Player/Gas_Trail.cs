using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas_Trail : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Mayfly"))
        {
            col.GetComponent<EnemyHealth>().DeductHealth(damage);
        }
        if (col.CompareTag("Juggernaut"))
        {
            col.GetComponent<JuggernautAI>().TakeDamage(damage, false);
        }
    }
}
