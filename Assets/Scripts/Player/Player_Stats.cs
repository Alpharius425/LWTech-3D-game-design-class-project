using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    //=========================FIELDS=========================
    public int maxHealth;
    public int curHealth;
    [SerializeField] float maxTimeUntilRegen; // max time until the player can begin getting their health back
    [SerializeField] float timeUntilRegen; // how long it takes until we START regening health
    [SerializeField] float healthRegen; // how long it takes to restore 1 health
    [SerializeField] bool isRegening = false; // are we currently healing

    public int maxAmmoType1; // max amount of this ammo we can have
    public int curAmmoType1; // how much of this ammo we currently have
    [SerializeField] float ammoType1Range; // how far this can shoot
    [SerializeField] int ammoType1Damage; // how much damage this does
    [SerializeField] float gasTimeMax; // how long the gas effect stays for our attack
    [SerializeField] int gasDamage;
    [SerializeField] GameObject gasTrail; // prefab for our gas attack trail

    public int maxAmmoType2; // max amount of this ammo we can have
    public int curAmmoType2; // how much of this ammo we currently have
    [SerializeField] GameObject ammoType2Collider; // the collider for our shotgun style weapon (Attached to a child of the gun, damage dealt is also on the child)

    public int maxAmmoType3; // max amount of this ammo we can have
    public int curAmmoType3; // how much of this ammo we currently have
    [SerializeField] float ammoType3Range; // how far this can shoot
    [SerializeField] int ammoType3Damage; // how much damage this does

    [SerializeField] Camera fpsCamera; // where we fire from (in this case the camera)
    [SerializeField] GameObject firePoint;

    public Renderer gun;

    public bool hasKey = false;

    [SerializeField] AmmoType currentAmmo = AmmoType.ammo1; // determines what kind of ammo we are currently using. by default set to ammo type 1

    [SerializeField] bool isDead = false;

    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    [SerializeField] AudioSource playerAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip reloadOutSFX; //the audio clip containing the "reload out" SFX
    [SerializeField] AudioClip reloadInSFX; //the audio clip containing the "reload in" SFX
    [SerializeField] AudioClip getKeySFX; //the audio clip containing the "get key" SFX
    [SerializeField] AudioClip hurtSFX; //the audio clip containing the "damage" SFX
    [SerializeField] AudioClip deathSFX; //the audio clip containing the "death" SFX
    [SerializeField] AudioClip fireLaunch; //the audio clip containing the "fire pop" SFX
    [SerializeField] AudioClip fireBang; //the audio clip containing the "fireshot" SFX
    [SerializeField] AudioClip acidLaunch; //the audio clip containing the "acid launch" SFX
    //=========================VISUAL EFFECTS=========================
    [Header("Visual Effects")]
    [SerializeField] GameObject goldShotFX; //gameobject containing the golddust burst effect
    [SerializeField] GameObject goldGasFX; //gameobject containing the golddust gas trail effect
    [SerializeField] GameObject blueFireFX; //gameobject containing the golddust gas trail effect
    [SerializeField] GameObject acidShotFX; //gameobject containing the golddust gas trail effect
    //=========================METHODS=========================

    private void Awake()
    {
        ammoType2Collider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Ammo1")) // swaps to ammo 1 if we push this input
        {
            StartCoroutine(changeAmmo(1));
        }

        if (Input.GetButtonDown("Ammo2")) // swaps to ammo 2 if we push this input
        {
            StartCoroutine(changeAmmo(2));
        }

        if (Input.GetButtonDown("Ammo3")) // swaps to ammo 3 if we push this input
        {
            StartCoroutine(changeAmmo(3));
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
            //Debug.Log("Starting regen");
            StartCoroutine("RegenHealth");
        }
    }

    public void TakeDamage(int damage)
    {
        playerAudio.clip = hurtSFX; //set sound clip
        playerAudio.Play(); //play sound clip
        curHealth -= damage; // subtracts from our health

        if(curHealth <= 0) // if we have less than 0 health
        {
            playerAudio.clip = deathSFX;
            playerAudio.Play();
            isDead = true; // we are ded
        }
    }

    IEnumerator RegenHealth()
    {

        isRegening = true; // tells us we are regening health
        while (curHealth < maxHealth) // while our health is less than our max
        {
            yield return new WaitForSeconds(healthRegen); // wait this long
            HPRegen(1); // then start this function and give it 1 as a heal value
        }
        isRegening = false; // tells us we aren't regening
    }

    public void HPRegen(int amount) // amount is the amount we are healed by
    {
        curHealth += amount; // increases our health
        //Debug.Log("Our health is now" + curHealth);
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

    public void PickUpKey()
    {
        playerAudio.clip = getKeySFX; //set sound clip
        playerAudio.Play(); //play sound clip
        hasKey = true;
    }

    void Shoot(AmmoType ammo) // ask what type of ammo we are using and then takes it from the pool
    {

        RaycastHit hit;

        switch(ammo)
        {
            case AmmoType.ammo1:
               if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, ammoType1Range))
               {
                    // yes I know this instantiate looks ugly don't judge me ;-;
                    //you're trying your best Mikey it's okay <3
                    playerAudio.clip = fireLaunch; //set sound clip
                    playerAudio.Play(); //play sound clip
                    Instantiate(goldShotFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the gold dust burst effect
                    Instantiate(goldGasFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the gold dust gas trail effect
                    GameObject trail = Instantiate(gasTrail, firePoint.transform.position, fpsCamera.transform.rotation); // makes our gas trail and spawns it in the right place
                    trail.GetComponentInChildren<Transform>().localScale = new Vector3(1, 1, hit.distance);// edits the scale of the gas trail to only go where we hit
                    trail.GetComponent<Trail_Holder>().duration = gasTimeMax; // gives our gas trail a duration
                    trail.GetComponentInChildren<Gas_Trail>().damage = gasDamage;
                    Debug.Log(hit.transform.name); // placeholder for dealing damage

                    if (hit.collider.CompareTag("Mayfly"))
                    {
                        hit.collider.GetComponent<EnemyHealth>().DeductHealth(ammoType3Damage);
                    }
                }
               else
               {
                    playerAudio.clip = fireLaunch; //set sound clip
                    playerAudio.Play(); //play sound clip
                    Instantiate(goldShotFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the gold dust burst effect
                    Instantiate(goldGasFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the gold dust gas trail effect
                    GameObject trail = Instantiate(gasTrail, firePoint.transform.position, fpsCamera.transform.rotation); // makes our gas trail and spawns it in the right place
                    trail.transform.localScale = new Vector3(1, 1, ammoType1Range); // edits the scale of the gas trail to only go where we hit
                    trail.GetComponent<Trail_Holder>().duration = gasTimeMax;// gives our gas trail a duration
                    trail.GetComponentInChildren<Gas_Trail>().damage = gasDamage;
                    
                }
                break;

            case AmmoType.ammo2:
                playerAudio.clip = fireBang; //set sound clip
                playerAudio.Play(); //play sound clip
                Instantiate(blueFireFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the blue fire effect
                ammoType2Collider.SetActive(true);
                break;

            case AmmoType.ammo3:
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, ammoType3Range))
                {
                    playerAudio.clip = acidLaunch; //set sound clip
                    playerAudio.Play(); //play sound clip
                    Instantiate(acidShotFX, firePoint.transform.position, fpsCamera.transform.rotation); //creates the acid shot effect
                    if (hit.collider.CompareTag("AcidDoor"))
                    {
                        hit.collider.GetComponent<AcidDoor>().Melt();
                    }
                    
                    if(hit.collider.CompareTag("Mayfly"))
                    {
                        hit.collider.GetComponent<EnemyHealth>().DeductHealth(ammoType3Damage);
                    }
                }
                else
                {
                    Debug.Log("Out of range");
                }
                break;
        }

    }
    //=========================Grant's Stuff=========================
    private IEnumerator changeAmmo(int type)
    {
        if (type == 1) //if changing to ammo type 1...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSeconds(2); //wait for animation and sfx to finish
            playerAudio.clip = reloadInSFX; //change clip to in sfxwwwwwwwwwwww
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo1; //set ammo type to 1
        }
        else if (type == 2) //if changing to ammo type 2...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSeconds(2); //wait for animation and sfx to finish
            playerAudio.clip = reloadInSFX; //change clip to in sfx
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo2; //set ammo type to 2
        }
        else if (type == 3) //if changing to ammo type 3...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSecondsRealtime(2); //wait for animation and sfx to finish
            playerAudio.clip = reloadInSFX; //change clip to in sfx
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo3; //set ammo type to 3
        }
    }
}

public enum AmmoType { ammo1, ammo2, ammo3}; // enum used to determine what ammo type we are using, can also be used in other scripts