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

        for (int i = enemies.Count-1; i > -1 ; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                shotGunCone.enemiesInRange.RemoveAt(i);
            }
        }

        foreach (GameObject enemy in enemies)
        {
            EnemyHealth curEnemy = enemy.GetComponent<EnemyHealth>();
            if (curEnemy != null)
            {
                curEnemy.DeductHealth(damage, ammoType);
                if (curEnemy.curHealth == 0) shotGunCone.enemiesInRange.Remove(enemy);
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
