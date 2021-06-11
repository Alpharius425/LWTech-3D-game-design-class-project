using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Cone : MonoBehaviour
{
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public List<GameObject> destructablesInRange = new List<GameObject>();

    private void OnTriggerEnter(Collider col)
    {
        EnemyHealth enemyThatEntered = col.GetComponent<EnemyHealth>();
        Destructable_Object destructableThatEntered = col.GetComponent<Destructable_Object>();

        if (enemyThatEntered != null) enemiesInRange.Add(enemyThatEntered.gameObject);
        if (destructableThatEntered != null) destructablesInRange.Add(destructableThatEntered.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        if (enemiesInRange.Contains(col.gameObject)) enemiesInRange.Remove(col.gameObject);
        if (destructablesInRange.Contains(col.gameObject)) destructablesInRange.Remove(col.gameObject);
    }
}
