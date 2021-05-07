using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{

    [SerializeField] int maxHealth;
    [SerializeField] int curHealth;
    [SerializeField] float maxTimeUntilRegen; // max time until the player can begin getting their health back
    [SerializeField] float timeUntilRegen; // how long it takes until we START regening health
    [SerializeField] float healthRegen; // how long it takes to restore 1 health
    [SerializeField] bool isRegening = false; // are we currently healing

    [SerializeField] int maxAmmoType1; // max amount of this ammo we can have
    [SerializeField] int curAmmoType1; // how much of this ammo we currently have

    [SerializeField] int maxAmmoType2; // max amount of this ammo we can have
    [SerializeField] int curAmmoType2; // how much of this ammo we currently have

    [SerializeField] int maxAmmoType3; // max amount of this ammo we can have
    [SerializeField] int curAmmoType3; // how much of this ammo we currently have

    [SerializeField] GameObject firePoint;

    public Renderer gun;



    [SerializeField] AmmoType currentAmmo = AmmoType.ammo1; // determines what kind of ammo we are currently using. by default set to ammo type 1

    [SerializeField] bool isDead = false;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Ammo1")) // swaps to ammo 1 if we push this input
        {
            currentAmmo = AmmoType.ammo1;
        }

        if (Input.GetButtonDown("Ammo2")) // swaps to ammo 2 if we push this input
        {
            currentAmmo = AmmoType.ammo2;
        }

        if (Input.GetButtonDown("Ammo3")) // swaps to ammo 3 if we push this input
        {
            currentAmmo = AmmoType.ammo3;
        }


        if (Input.GetButtonDown("Fire1")) // if we left click
        {
            switch(currentAmmo) // looks at what ammo type we are currently using
            {
                case AmmoType.ammo1: // if we are using ammo type 1
                    if(curAmmoType1 > 0) // checks if we have ammo
                    {
                        Shoot(AmmoType.ammo1); // fires using this ammo
                        curAmmoType1 -= 1; // subtracts that ammo from our counter
                    }
                    else
                    {
                        Debug.Log("No ammoType1");
                    }
                    break;

                case AmmoType.ammo2: // if we are using ammo type 2
                    if (curAmmoType2 > 0) // checks if we have ammo
                    {
                        Shoot(AmmoType.ammo2); // fires using this ammo
                        curAmmoType2 -= 1; // subtracts that ammo from our counter
                    }
                    else
                    {
                        Debug.Log("No ammoType2");
                    }
                    break;

                case AmmoType.ammo3: // if we are using ammo type 1
                    if (curAmmoType3 > 0) // checks if we have ammo
                    {
                        Shoot(AmmoType.ammo3); // fires using this ammo
                        curAmmoType3 -= 1; // subtracts that ammo from our counter
                    }
                    else
                    {
                        Debug.Log("No ammoType3");
                    }
                    break;
            }
        }

        if(timeUntilRegen > 0)
        {
            timeUntilRegen -= Time.deltaTime; // ticks the timer down until we can heal again
        }

        if(isRegening == false && timeUntilRegen <= 0 && curHealth < maxHealth) // starts the coroutine that heals the player
        {
            Debug.Log("Starting regen");
            StartCoroutine("RegenHealth");
        }
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage; // subtracts from our health

        if(curHealth <= 0) // if we have less than 0 health
        {
            isDead = true; // we are ded
        }
    }

    IEnumerator RegenHealth()
    {

        isRegening = true; // tells us we are regening health
        while (curHealth < maxHealth) // while our health is less than our max
        {
            yield return new WaitForSeconds(healthRegen); // wait this long
            HPRegen(); // then start this function
        }
        isRegening = false; // tells us we aren't regening
    }

    void HPRegen()
    {
        curHealth += 1; // increases our health
        Debug.Log("Our health is now" + curHealth);
        if (curHealth > maxHealth) // makes sure we don't go over our max health
        {
            curHealth = maxHealth;
        }
    }

    public void IncreaseAmmo(AmmoType ammo, int value) // ammo determines what ammo type is increased. Value determines how much ammo we get.
    {
        switch(ammo) // determines what type of ammo we get from the pick up
        {
            case AmmoType.ammo1:
                curAmmoType1 += value; // increases our current ammo
                if(curAmmoType1 > maxAmmoType1) // stops us from going over the max
                {
                    curAmmoType1 = maxAmmoType1;
                }
                break;

            case AmmoType.ammo2:
                curAmmoType2 += value; // increases our current ammo
                if (curAmmoType2 > maxAmmoType2) // stops us from going over the max
                {
                    curAmmoType2 = maxAmmoType2;
                }
                break;

            case AmmoType.ammo3:
                curAmmoType3 += value; // increases our current ammo
                if (curAmmoType3 > maxAmmoType3) // stops us from going over the max
                {
                    curAmmoType3 = maxAmmoType3;
                }
                break;
        }
    }

    void Shoot(AmmoType ammo) // ask what type of ammo we are using and then takes it from the pool
    {
        GameObject projectile = Projectile_Pooler.ourPooler.GetPlayerPooledObjects(ammo);

        projectile.transform.position = firePoint.transform.position;
        projectile.transform.rotation = firePoint.transform.rotation;

        projectile.SetActive(true);
        Rigidbody rb = projectile.GetComponent<Rigidbody>(); // gets the rigidbody of the bullet

        //rb.velocity = projectile.transform.forward * projSpeed; // sends the bullet flying forward
    }
}

public enum AmmoType { ammo1, ammo2, ammo3}; // enum used to determine what ammo type we are using, can also be used in other scripts