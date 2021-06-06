using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Stats : MonoBehaviour
{
    //=========================FIELDS=========================

    public LayerMask groundedLayer;
    public float maxHealth;
    public float curHealth;
    [SerializeField] float maxTimeUntilRegen; // max time until the player can begin getting their health back
    [SerializeField] float timeUntilRegen; // how long it takes until we START regening health
    [SerializeField] float healthRegen; // how long it takes to restore 1 health
    [SerializeField] bool isRegening = false; // are we currently healing

    [SerializeField] bool reloading = false;
    public int maxAmmoType1; // max amount of this ammo we can have
    public int curAmmoType1; // how much of this ammo we currently have
    [SerializeField] int type1AmmoConsumption; //amount of ammo each shot consumes
    [SerializeField] float ammoType1Range; // how far this can shoot
    [SerializeField] int ammoType1Damage; // how much damage this does
    [SerializeField] float gasTimeMax; // how long the gas effect stays for our attack
    [SerializeField] int gasDamage;
    [SerializeField] GameObject gasTrail; // prefab for our gas attack trail

    public int maxAmmoType2; // max amount of this ammo we can have
    public int curAmmoType2; // how much of this ammo we currently have
    [SerializeField] int type2AmmoConsumption; //amount of ammo each shot consumes
    [SerializeField] GameObject ammoType2Collider; // the collider for our shotgun style weapon (Attached to a child of the gun, damage dealt is also on the child)
    [SerializeField] Player_Shotgun shotGun;

    public int maxAmmoType3; // max amount of this ammo we can have
    public int curAmmoType3; // how much of this ammo we currently have
    [SerializeField] int type3AmmoConsumption; //amount of ammo each shot consumes
    [SerializeField] float ammoType3Range; // how far this can shoot
    [SerializeField] int ammoType3Damage; // how much damage this does

    [SerializeField] LayerMask attackMask; // holds the layers that the attacks should be affecting

    [SerializeField] Camera fpsCamera; // where we fire from (in this case the camera)
    [SerializeField] GameObject firePoint;

    public bool hasKey = false;

    [SerializeField] AmmoType currentAmmo = AmmoType.ammo1; // determines what kind of ammo we are currently using. by default set to ammo type 1

    [SerializeField] bool isDead = false;

    [SerializeField] GameObject gun;
    [SerializeField] Animator myAnim;
    [SerializeField] FirstPersonController myController;
    [SerializeField] Image fadeRenderer;


    [SerializeField] Pause_Menu pauseMenu;
    // Improvising the Potion change!!! This can probably be done better!!!
    [SerializeField] GameObject yellowPotion;
    [SerializeField] GameObject redPotion;
    [SerializeField] GameObject bluePotion;

    //=========================DEBUG UI==================================
    //public Slider HealthBar; //for storing reference to healthbar UI element

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
    //=========================UI ELEMENTS=========================
    [Header("UI Elements")]
    [SerializeField] Slider yellowSlider; //Slider used to show amount of yellow ammo available
    [SerializeField] Slider blueSlider; //Slider used to show amount of blue ammo available
    [SerializeField] Slider redSlider; //Slider used to show amount of red ammo available
    [SerializeField] GameObject yellowSelection; //an object used to highlight the yellow ammo when in use
    [SerializeField] GameObject blueSelection; //an object used to highlight the blue ammo when in use
    [SerializeField] GameObject redSelection; //an object used to highlight the red ammo when in use
    //=========================METHODS=========================

    private void Awake()
    {
        //ammoType2Collider.SetActive(false);
    }

    private void Start()
    {
        timeUntilRegen = maxTimeUntilRegen;
        maxHealth = 100; //initialize max health to 100
        //myAnim = GetComponentInChildren<Animator>();
        myController = GetComponent<FirstPersonController>();
        //HealthBar.value = CalculateHealth(); //calculate health and set to current
        yellowSlider.value = curAmmoType1; //set the ammo sliders to the proper amounts
        blueSlider.value = curAmmoType2;
        redSlider.value = curAmmoType3;
        yellowSelection.gameObject.SetActive(true); //display the selection on the proper ammo type
        blueSelection.gameObject.SetActive(false); //turn off selection on this ammo type
        redSelection.gameObject.SetActive(false); //turn off selection on this ammo type
        Cursor.lockState = CursorLockMode.Locked; // locks the cursor

        StartCoroutine("BlackFadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetButtonDown("Fire2"))
        //{
            //TakeDamage(10);
        //}

        if(Input.GetButtonDown("Ammo1") && pauseMenu.pause == false) // swaps to ammo 1 if we push this input
        {
            StartCoroutine(changeAmmo(1));
        }

        if (Input.GetButtonDown("Ammo2") && pauseMenu.pause == false) // swaps to ammo 2 if we push this input
        {
            StartCoroutine(changeAmmo(2));
        }

        if (Input.GetButtonDown("Ammo3") && pauseMenu.pause == false) // swaps to ammo 3 if we push this input
        {
            StartCoroutine(changeAmmo(3));
        }

        if(Input.GetButtonDown("Cancel"))
        {
            if(pauseMenu.pause == true)
            {
                myController.cameraCanMove = false; //lock camera movement
            }
            else
            {
                myController.cameraCanMove = true; //unlock camera movement
            }
        }


        if (Input.GetButtonDown("Fire1") && reloading == false && pauseMenu.pause == false) // if we left click
        {
            switch(currentAmmo) // looks at what ammo type we are currently using
            {
                case AmmoType.ammo1: // if we are using ammo type 1
                    if(curAmmoType1 > 0) // checks if we have ammo
                    {
                        Shoot(AmmoType.ammo1); // fires using this ammo
                        curAmmoType1 -= type1AmmoConsumption; // subtracts that ammo from our counter
                        yellowSlider.value = curAmmoType1; //set the ammo slider to the proper amount
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
                        curAmmoType2 -= type2AmmoConsumption; // subtracts that ammo from our counter
                        blueSlider.value = curAmmoType2; //set the ammo slider to the proper amount
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
                        curAmmoType3 -= type3AmmoConsumption; // subtracts that ammo from our counter
                        redSlider.value = curAmmoType3; //set the ammo slider to the proper amount
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

        if(isRegening == false && timeUntilRegen <= 0 && curHealth < maxHealth && pauseMenu.pause == false) // starts the coroutine that heals the player
        {
            //Debug.Log("Starting regen");
            StartCoroutine("RegenHealth");
        }



        // improvising the animations and crouch scale cause we're using the player controller

        myAnim.SetBool("Moving", myController.isWalking);
        myAnim.SetBool("Sprinting", myController.isSprinting);

        //if(Input.GetKey(KeyCode.LeftControl)) // will hopefully fix our gun scaling issue when we crouch
        //{
        //    gun.transform.localScale = new Vector3(1, 2f, 1);
        //}
        //else
        //{
        //    gun.transform.localScale = new Vector3(1, 1, 1);
        //}
    }

    public void TakeDamage(int damage)
    {
        myAnim.SetTrigger("Hurt");
        playerAudio.clip = hurtSFX; //set sound clip
        playerAudio.Play(); //play sound clip
        curHealth -= damage; // subtracts from our health
        timeUntilRegen = maxTimeUntilRegen;
        //HealthBar.value = CalculateHealth(); //calculate health and set to current
        isRegening = false;
        StopCoroutine("RegenHealth");
        if (curHealth <= 0) // if we have less than 0 health
        {
            playerAudio.clip = deathSFX;
            playerAudio.Play();
            isDead = true; // we are ded
            fadeRenderer.gameObject.SetActive(true); // turns the fade gameobject on
            StartCoroutine("BlackFadeIn");
        }
    }

    IEnumerator BlackFadeIn()
    {

        for (float fade = 0f; fade <= 1f; fade += 0.1f) // runs a loop that makes the fade slowly become solid
        {
            Color newColor = fadeRenderer.color; // sets the color to same as our base color
            newColor.a = fade; // sets the alpha of the new color to be less transparent
            fadeRenderer.color = newColor; // resets the color of the sprite to be transparent

            if (fade <= 0.9f)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        fadeRenderer.gameObject.SetActive(false); // turns the fade gameobject off
        SceneManager.LoadScene(1);
        StopCoroutine("BlackFadeIn");
    }

    IEnumerator BlackFadeOut()
    {

        for (float fade = 1f; fade >= 0f; fade -= 0.1f) // runs a loop that makes the fade slowly become transparent
        {
            Color newColor = fadeRenderer.color; // sets the color to same as our base color
            newColor.a = fade; // sets the alpha of the new color to be more transparent
            fadeRenderer.color = newColor; // resets the color of the sprite to be transparent

            if (fade >= 0.1f)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        fadeRenderer.gameObject.SetActive(false); // turns the fade gameobject off
        
        StopCoroutine("BlackFadeInOut");
    }

    IEnumerator RegenHealth()
    {

        isRegening = true; // tells us we are regening health
        while (curHealth < maxHealth) // while our health is less than our max
        {
            yield return new WaitForSeconds(healthRegen); // wait this long
            HPRegen(1f); // then start this function and give it 1 as a heal value
        }
        isRegening = false; // tells us we aren't regening
    }

    public void HPRegen(float amount) // amount is the amount we are healed by
    {
        curHealth += amount; // increases our health
        //Debug.Log("Our health is now" + curHealth);
        if (curHealth > maxHealth) // makes sure we don't go over our max health
        {
            isRegening = false;
            curHealth = maxHealth;
            StopCoroutine("RegenHealth");
        }
        //HealthBar.value = CalculateHealth(); //calculate health and set to current
    }

    public void IncreaseAmmo(AmmoType ammo, int value) // ammo determines what ammo type is increased. Value determines how much ammo we get.
    {
        switch(ammo) // determines what type of ammo we get from the pick up
        {
            case AmmoType.ammo1:
                curAmmoType1 += value; // increases our current ammo
                yellowSlider.value = curAmmoType1; //set the ammo slider to the proper amount
                if (curAmmoType1 > maxAmmoType1) // stops us from going over the max
                {
                    curAmmoType1 = maxAmmoType1;
                }
                break;

            case AmmoType.ammo2:
                curAmmoType2 += value; // increases our current ammo
                blueSlider.value = curAmmoType2; //set the ammo slider to the proper amount
                if (curAmmoType2 > maxAmmoType2) // stops us from going over the max
                {
                    curAmmoType2 = maxAmmoType2;
                }
                break;

            case AmmoType.ammo3:
                curAmmoType3 += value; // increases our current ammo
                redSlider.value = curAmmoType3; //set the ammo slider to the proper amount
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
        myAnim.SetTrigger("Attack");
        RaycastHit hit;

        switch(ammo)
        {
            case AmmoType.ammo1:
               if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, ammoType1Range, attackMask))
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
                        hit.collider.GetComponent<EnemyHealth>().DeductHealth(ammoType1Damage);
                    }
                    if (hit.collider.CompareTag("Destructable"))
                    {
                        hit.collider.GetComponent<Destructable_Object>().DestroyObject();
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
                shotGun.Fire();
                //ammoType2Collider.SetActive(true);
                break;

            case AmmoType.ammo3:
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, ammoType3Range, attackMask))
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
                    if (hit.collider.CompareTag("Destructable"))
                    {
                        hit.collider.GetComponent<Destructable_Object>().DestroyObject();
                    }
                }
                else
                {
                    Debug.Log("Out of range");
                }
                break;
        }
    }

    float CalculateHealth() //returns the health calculation for updating UI
    {
        return curHealth / maxHealth;
    }
    //=========================Grant's Stuff=========================
    private IEnumerator changeAmmo(int type)
    {
        //myAnim.SetBool("Reloading", true);
        myAnim.SetTrigger("Reloading");
        reloading = true;

        if (type == 1) //if changing to ammo type 1...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSeconds(2); //wait for animation and sfx to finish

            //changes the color of the bottle by swapping them out with the other models
            yellowPotion.SetActive(true);
            redPotion.SetActive(false);
            bluePotion.SetActive(false);
            yellowSelection.gameObject.SetActive(true); //display the selection on the proper ammo type
            blueSelection.gameObject.SetActive(false); //turn off selection on this ammo type
            redSelection.gameObject.SetActive(false); //turn off selection on this ammo type

            playerAudio.clip = reloadInSFX; //change clip to in sfx
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo1; //set ammo type to 1
        }
        else if (type == 2) //if changing to ammo type 2...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSeconds(2); //wait for animation and sfx to finish

            //changes the color of the bottle by swapping them out with the other models
            yellowPotion.SetActive(false);
            redPotion.SetActive(false);
            bluePotion.SetActive(true);
            yellowSelection.gameObject.SetActive(false);  //turn off selection on this ammo type
            blueSelection.gameObject.SetActive(true); //display the selection on the proper ammo type
            redSelection.gameObject.SetActive(false); //turn off selection on this ammo type

            playerAudio.clip = reloadInSFX; //change clip to in sfx
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo2; //set ammo type to 2
        }
        else if (type == 3) //if changing to ammo type 3...
        {
            playerAudio.clip = reloadOutSFX; //change clip to out sfx
            playerAudio.Play(); //play clip
            yield return new WaitForSecondsRealtime(2); //wait for animation and sfx to finish

            //changes the color of the bottle by swapping them out with the other models
            yellowPotion.SetActive(false);
            redPotion.SetActive(true);
            bluePotion.SetActive(false);
            yellowSelection.gameObject.SetActive(false); //turn off selection on this ammo type
            blueSelection.gameObject.SetActive(false); //turn off selection on this ammo type
            redSelection.gameObject.SetActive(true); //display the selection on the proper ammo type

            playerAudio.clip = reloadInSFX; //change clip to in sfx
            playerAudio.Play(); //play clip
            currentAmmo = AmmoType.ammo3; //set ammo type to 3
        }

        reloading = false;
        //myAnim.SetBool("Reloading", false);
    }

    public void ResumeCameraControl() //needed for pause functionality to work properly
    {
        myController.cameraCanMove = true; //unlock camera movement
    }
}

public enum AmmoType { ammo1, ammo2, ammo3}; // enum used to determine what ammo type we are using, can also be used in other scripts
