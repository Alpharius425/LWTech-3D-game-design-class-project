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
        JuggernautAI juggernautThatEntered = col.GetComponent<JuggernautAI>();
        Destructable_Object destructableThatEntered = col.GetComponent<Destructable_Object>();

        if (enemyThatEntered != null) enemiesInRange.Add(enemyThatEntered.gameObject);
        if (enemyThatEntered != null) enemiesInRange.Add(juggernautThatEntered.gameObject);
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
            if(enemy.CompareTag("Mayfly"))
            {
                EnemyHealth curMayFly = enemy.GetComponent<EnemyHealth>();
                curMayFly.DeductHealth(damage);

                if (curMayFly.enemyHealth == 0) enemiesInRange.Remove(enemy);
            }

            if (enemy.CompareTag("Juggernaut"))
            {
                JuggernautAI curJuggernaut = enemy.GetComponent<JuggernautAI>();
                curJuggernaut.TakeDamage(damage, false);

                if (curJuggernaut.enemyHealth == 0) enemiesInRange.Remove(enemy);
            }
        }
        foreach (GameObject destructable in destructables)
        {
            destructable.GetComponent<Destructable_Object>().DestroyObject();
            destructablesInRange.Remove(destructable);
        }
    }
}
