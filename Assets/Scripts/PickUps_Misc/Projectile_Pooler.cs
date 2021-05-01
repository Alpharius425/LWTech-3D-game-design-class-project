using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Pooler : MonoBehaviour
{
    public static Projectile_Pooler ourPooler; // lets other scripts access this

    public GameObject playerBulletAmmo1; // prefab of ammo type 1
    public GameObject playerBulletAmmo2; // prefab of ammo type 2
    public GameObject playerBulletAmmo3; // prefab of ammo type 3
    //public GameObject enemyBullet;

    public int poolAmount = 50; // how big the pools are

    public bool willGrow = true;

    private List<GameObject> playerBulletsAmmo1; // the list of ammo type 1
    private List<GameObject> playerBulletsAmmo2; // the list of ammo type 2
    private List<GameObject> playerBulletsAmmo3; // the list of ammo type 3

    //private List<GameObject> enemyBullets;

    private void Awake()
    {
        ourPooler = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerBulletsAmmo1 = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(playerBulletAmmo1);
            obj.SetActive(false);
            playerBulletsAmmo1.Add(obj);

            //pooledObjects.Remove(obj);
        }

        playerBulletsAmmo2 = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(playerBulletAmmo2);
            obj.SetActive(false);
            playerBulletsAmmo2.Add(obj);

            //pooledObjects.Remove(obj);
        }

        playerBulletsAmmo3 = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(playerBulletAmmo3);
            obj.SetActive(false);
            playerBulletsAmmo3.Add(obj);

            //pooledObjects.Remove(obj);
        }

        //enemyBullets = new List<GameObject>();

        //for (int i = 0; i < poolAmount; i++)
        //{
        //    GameObject obj = (GameObject)Instantiate(enemyBullet);
        //    obj.SetActive(false);
        //    enemyBullets.Add(obj);

        //    //pooledObjects.Remove(obj);
        //}
    }

    public GameObject GetPlayerPooledObjects(AmmoType ammo) // ask for what kind of ammo we are using
    {
        switch (ammo)
        {
            case AmmoType.ammo1: // if we're using ammo 1
                for (int i = 0; i < playerBulletsAmmo1.Count; i++) // runs through the ammo 1 pool
                {
                    if (!playerBulletsAmmo1[i].activeInHierarchy) // if one is inactive
                    {
                        return playerBulletsAmmo1[i]; // sends back that bullet
                    }
                }

                if (willGrow) // if we for some reason don't have those bullets
                {
                    GameObject obj = Instantiate(playerBulletAmmo1); // creates the bullet
                    playerBulletsAmmo1.Add(obj); // adds to the list
                    return obj; // returns the bullet
                }
                break;

            case AmmoType.ammo2: // if we're using ammo 2
                for (int i = 0; i < playerBulletsAmmo2.Count; i++) // runs through the ammo 2 pool
                {
                    if (!playerBulletsAmmo2[i].activeInHierarchy) // if one is inactive
                    {
                        return playerBulletsAmmo2[i]; ; // sends back that bullet
                    }
                }

                if (willGrow) // if we for some reason don't have those bullets
                {
                    GameObject obj = Instantiate(playerBulletAmmo2); // creates the bullet
                    playerBulletsAmmo2.Add(obj); // adds to the list
                    return obj; // returns the bullet
                }
                break;

            case AmmoType.ammo3: // if we're using ammo 2
                for (int i = 0; i < playerBulletsAmmo3.Count; i++) // runs through the ammo 2 pool
                {
                    if (!playerBulletsAmmo3[i].activeInHierarchy) // if one is inactive
                    {
                        return playerBulletsAmmo3[i]; ; // sends back that bullet
                    }
                }

                if (willGrow) // if we for some reason don't have those bullets
                {
                    GameObject obj = Instantiate(playerBulletAmmo3); // creates the bullet
                    playerBulletsAmmo3.Add(obj); // adds to the list
                    return obj; // returns the bullet
                }
                break;

        }
        

        return null;
    }

    //public GameObject GetEnemyPooledObjects()
    //{
    //    for (int i = 0; i < enemyBullets.Count; i++)
    //    {
    //        if (!enemyBullets[i].activeInHierarchy)
    //        {
    //            return enemyBullets[i];
    //        }
    //    }

    //    if (willGrow)
    //    {
    //        GameObject obj = Instantiate(enemyBullet);
    //        enemyBullets.Add(obj);
    //        return obj;
    //    }

    //    return null;
    //}
}
