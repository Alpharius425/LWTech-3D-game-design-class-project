using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public UnityEvent died;
    bool hasDied = false;
    public float curHealth = 100f;

    public virtual void DeductHealth(float damage, AmmoType ammoType = AmmoType.gas)
    {
        curHealth -= damage;
        if (curHealth < 0)
        {
            curHealth = 0;
        }

        if (!hasDied && curHealth == 0)
        {
            died.Invoke();
            hasDied = true;
        }
    }
}
