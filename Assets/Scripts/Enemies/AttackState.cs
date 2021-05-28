using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class AttackState : State
{
    public AttackState(StateManager manager) : base(manager)
    {

    }

    public GameObject player;
    public static Transform targetLastPos;
    private int damageAmount;
    [HideInInspector] public static GameObject magicAttack;
    [HideInInspector] public static GameObject spawnPoint;

    //For timer
    [SerializeField] private float seconds; // How long the timer lasts in seconds
    private float chargeProgress; // Progress through the timer from 0 - 1

    public override void OnStateEnter()
    {
        player = stateManager.playerTarget;
        damageAmount = stateManager.damageAmount;
        magicAttack = stateManager.magicAttack;
        spawnPoint = stateManager.spawnPoint;
        stateManager.FOVCone.color = Color.red;
    }

    public override void RunCurrentState()
    {
        targetLastPos = stateManager.playerTarget.transform;
        ShootMagic();
        stateManager.ChangeState(stateManager.chargeUpAttackState);
    }

    private void ShootMagic()
    {
        GameObject.Instantiate(magicAttack, stateManager.spawnPoint.transform.position, Quaternion.identity);

        //TODO: Only damage player if player collides with magic attack collider
        //player.GetComponent<Player_Stats>().TakeDamage(damageAmount);

        //Debug.Log("I have fired at the player!");
    }
}
