using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shotgun : MonoBehaviour
{
    [SerializeField] int damage;

    List<GameObject> enemiesInRange = new List<GameObject>();
    List<GameObject> destructablesInRange = new List<GameObject>();

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

    public void Fire()
    {
        List<GameObject> enemies = new List<GameObject>(enemiesInRange);
        List<GameObject> destructables = new List<GameObject>(destructablesInRange);

        foreach (GameObject enemy in enemies)
        {
            EnemyHealth curEnemy = enemy.GetComponent<EnemyHealth>();
            curEnemy.DeductHealth(damage);

            if (curEnemy.enemyHealth == 0) enemiesInRange.Remove(enemy);
        }
        foreach (GameObject destructable in destructables)
        {
            destructable.GetComponent<Destructable_Object>().DestroyObject();
            destructablesInRange.Remove(destructable);
        }
    }
}
