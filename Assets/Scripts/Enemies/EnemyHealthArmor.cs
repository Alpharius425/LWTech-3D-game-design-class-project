using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthArmor : EnemyHealth
{
    bool hasArmor = true;
    public UnityEvent armorDestroyed;
    
    public override void DeductHealth(float damage, AmmoType ammoType)
    {
        if (ammoType == AmmoType.acid)
        {
            //If hit by acid - make armor disappear
            if (hasArmor)
            {
                armorDestroyed.Invoke();
                hasArmor = false;
            }
            Debug.Log("Armor destroyed");
        }

        //Take no damage if projectile is non acid and has armor.
        if (hasArmor == false)
        {
            base.DeductHealth(damage, ammoType);
            //Take damage by any projectile
            Debug.Log("Armor is gone so attack got through");
        }
        else
        {
            Debug.Log("Armor resisted the attack");
        }
    }
}
