using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : Weapon
{
    public Shotgun_Cone shotGunCone;

    public override void Fire()
    {
        List<GameObject> enemies = new List<GameObject>(shotGunCone.enemiesInRange);
        List<GameObject> destructables = new List<GameObject>(shotGunCone.destructablesInRange);

        foreach (GameObject enemy in enemies)
        {
            if(enemy.CompareTag("Mayfly"))
            {
                EnemyHealth curMayFly = enemy.GetComponent<EnemyHealth>();
                curMayFly.DeductHealth(damage);

                if (curMayFly.enemyHealth == 0) shotGunCone.enemiesInRange.Remove(enemy);
            }

            if (enemy.CompareTag("Juggernaut"))
            {
                JuggernautAI curJuggernaut = enemy.GetComponent<JuggernautAI>();
                curJuggernaut.TakeDamage(damage, false);

                if (curJuggernaut.enemyHealth == 0) shotGunCone.enemiesInRange.Remove(enemy);
            }
        }
        foreach (GameObject destructable in destructables)
        {
            destructable.GetComponent<Destructable_Object>().DestroyObject();
            shotGunCone.destructablesInRange.Remove(destructable);
        }

        InstantiateVFX();
        curAmmo -= ammoCost; // subtracts that ammo from our counter
    }
}
