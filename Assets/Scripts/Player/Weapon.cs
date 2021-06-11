using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Player_Stats playerStats;
    public int maxAmmo; // max amount of this ammo we can have
    public int curAmmo; // how much of this ammo we currently have
    public int ammoCost; //amount of ammo each shot consumes
    public float range; // how far this can shoot
    public int damage; // how much damage this does
    public AmmoType ammoType;
    public AudioClip fireSound;
    public GameObject[] VFX;

    public virtual void Fire()
    {
        RaycastHit hit;

        playerStats.playerAudio.PlayOneShot(fireSound);
        float trailZScale = range;

        if (Physics.Raycast(playerStats.fpsCamera.transform.position, playerStats.fpsCamera.transform.forward, out hit, range, playerStats.attackMask))
        {
            trailZScale = hit.distance;
            EnemyHealth curEnemy = hit.collider.GetComponent<EnemyHealth>();
            if (curEnemy != null) curEnemy.DeductHealth(damage, ammoType);

            if (ammoType == AmmoType.acid && hit.collider.CompareTag("AcidDoor"))
            {
                hit.collider.GetComponent<AcidDoor>().Melt();
            }
            if (hit.collider.CompareTag("Destructable"))
            {
                hit.collider.GetComponent<Destructable_Object>().DestroyObject();
            }
        }

        InstantiateVFX();

        if(ammoType == AmmoType.gas)
        {
            GameObject trail = Instantiate(playerStats.gasTrail, playerStats.firePoint.transform.position, playerStats.fpsCamera.transform.rotation); // makes our gas trail and spawns it in the right place
            trail.GetComponent<Trail_Holder>().duration = playerStats.gasTimeMax;// gives our gas trail a duration
            trail.GetComponentInChildren<Gas_Trail>().damage = playerStats.gasDamage;
            trail.transform.localScale = new Vector3(1, 1, trailZScale); // edits the scale of the gas trail to only go where we hit
        }

        curAmmo -= ammoCost; // subtracts that ammo from our counter
    }

    protected void InstantiateVFX()
    {
        for (int i = 0; i < VFX.Length; i++)
        {
            Instantiate(VFX[i], playerStats.firePoint.transform.position, playerStats.fpsCamera.transform.rotation); //creates the gold dust burst effect
        }
    }
}