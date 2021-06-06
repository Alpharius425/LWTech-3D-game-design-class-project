using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas_Trail : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        EnemyHealth curEnemy = col.GetComponent<EnemyHealth>();

        if (curEnemy != null)
        {
            curEnemy.DeductHealth(damage, AmmoType.gas);
        }
    }
}
