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
    
    public float gasTimeMax; // how long the gas effect stays for our attack
    public int gasDamage;
    public GameObject gasTrail; // prefab for our gas attack trail
    
    [SerializeField] GameObject ammoType2Collider; // the collider for our shotgun style weapon (Attached to a child of the gun, damage dealt is also on the child)
    [SerializeField] Weapon_Shotgun shotGun;

    [SerializeField] public Weapon[] weapons;
    private Weapon curWeapon;

    public LayerMask attackMask; // holds the layers that the attacks should be affecting

    public Camera fpsCamera; // where we fire from (in this case the camera)
    public GameObject firePoint;

    public bool hasKey = false;

    [SerializeField] AmmoType currentAmmo = AmmoType.gas; // determines what kind of ammo we are currently using. by default set to ammo type 1

    [SerializeField] bool isDead = false;

    [SerializeField] GameObject gun;
    [SerializeField] Animator myAnim;
    [SerializeField] FirstPersonController myController;
    [SerializeField] Image fadeRenderer;


    [SerializeField] Pause_Menu pauseMenu;
    // Improvising the Potion change!!! This can probably be done better!!!
    [SerializeField] GameObject[] ammoPotions;

    //=========================DEBUG UI==================================
    //public Slider HealthBar; //for storing reference to healthbar UI element

    //=========================SOUND EFFECTS=========================
    [Header("Sound Effects")]
    public AudioSource playerAudio; //the source we will be playing sounds from on this specific object
    [SerializeField] AudioClip reloadOutSFX; //the audio clip containing the "reload out" SFX
    [SerializeField] AudioClip reloadInSFX; //the audio clip containing the "reload in" SFX
    [SerializeField] AudioClip getKeySFX; //the audio clip containing the "get key" SFX
    [SerializeField] AudioClip hurtSFX; //the audio clip containing the "damage" SFX
    [SerializeField] AudioClip deathSFX; //the audio clip containing the "death" SFX
    //=========================UI ELEMENTS=========================
    [Header("UI Elements")]
    [SerializeField] Slider[] UISliders;
    [SerializeField] GameObject[] UISelectionOutlines;
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
        for (int i = 0; i < UISliders.Length; i++)
        {
            UpdateAmmoHUD(i);
            UISelectionOutlines[i].gameObject.SetActive(false);
        }

        curWeapon = weapons[0];
        UISelectionOutlines[System.Array.IndexOf(weapons, curWeapon)].gameObject.SetActive(true);

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
            StartCoroutine(changeAmmo(AmmoType.gas));
        }

        if (Input.GetButtonDown("Ammo2") && pauseMenu.pause == false) // swaps to ammo 2 if we push this input
        {
            StartCoroutine(changeAmmo(AmmoType.shotgun));
        }

        if (Input.GetButtonDown("Ammo3") && pauseMenu.pause == false) // swaps to ammo 3 if we push this input
        {
            StartCoroutine(changeAmmo(AmmoType.acid));
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


        if (Input.GetButtonDown("Fire1") && !reloading && !pauseMenu.pause) // if we left click
        {
            if (curWeapon.curAmmo > 0) // checks if we have ammo
            {
                Shoot(); // fires using this ammo
                
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
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].ammoType == ammo)
            {
                weapons[i].curAmmo += value;

                if (weapons[i].curAmmo > weapons[i].maxAmmo) // stops us from going over the max
                {
                    weapons[i].curAmmo = weapons[i].maxAmmo;
                }

                UpdateAmmoHUD(i);
            }
        }
    }

    public void PickUpKey()
    {
        playerAudio.clip = getKeySFX; //set sound clip
        playerAudio.Play(); //play sound clip
        hasKey = true;
    }

    void Shoot() // ask what type of ammo we are using and then takes it from the pool
    {
        myAnim.SetTrigger("Attack");
        curWeapon.Fire();
        UpdateAmmoHUD(System.Array.IndexOf(weapons, curWeapon));
    }

    void UpdateAmmoHUD(int sliderIndex)
    {
        UISliders[sliderIndex].value = weapons[sliderIndex].curAmmo;
    }

    float CalculateHealth() //returns the health calculation for updating UI
    {
        return curHealth / maxHealth;
    }
    //=========================Grant's Stuff=========================
    private IEnumerator changeAmmo(AmmoType type)
    {
        //myAnim.SetBool("Reloading", true);
        myAnim.SetTrigger("Reloading");
        reloading = true;

        playerAudio.clip = reloadOutSFX; //change clip to out sfx
        playerAudio.Play(); //play clip
        yield return new WaitForSeconds(2); //wait for animation and sfx to finish

        //changes the color of the bottle by swapping them out with the other models
        for (int i = 0; i < ammoPotions.Length; i++)
        {
            ammoPotions[i].gameObject.SetActive(false);
        }
        
        playerAudio.clip = reloadInSFX; //change clip to in sfx
        playerAudio.Play(); //play clip

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].ammoType == type)
            {
                curWeapon = weapons[i];
            }
        }

        ammoPotions[System.Array.IndexOf(weapons, curWeapon)].gameObject.SetActive(true);

        reloading = false;
        //myAnim.SetBool("Reloading", false);

        for (int i = 0; i < UISelectionOutlines.Length; i++)
        {
            UISelectionOutlines[i].gameObject.SetActive(false);
        }

        UISelectionOutlines[System.Array.IndexOf(weapons,curWeapon)].gameObject.SetActive(true);
    }

    public void ResumeCameraControl() //needed for pause functionality to work properly
    {
        myController.cameraCanMove = true; //unlock camera movement
    }
}