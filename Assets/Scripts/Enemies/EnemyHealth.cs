using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float curHealth = 100f;

    public virtual void DeductHealth(float damage, AmmoType ammoType = AmmoType.gas)
    {
        curHealth -= damage;
        if (curHealth < 0)
        {
            curHealth = 0;
        }
    }
}
