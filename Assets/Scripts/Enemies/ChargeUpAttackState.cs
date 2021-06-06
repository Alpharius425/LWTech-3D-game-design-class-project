using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class ChargeUpAttackState : State
{
    public ChargeUpAttackState(StateManager manager) : base(manager)
    {

    }

    public GameObject player;
    public static GameObject charge;
    public static Transform targetLastPos;
    private int damageAmount;
    [HideInInspector] public static GameObject magicAttack;
    [HideInInspector] public static GameObject spawnPoint;
    [HideInInspector] public static GameObject chargeUpAttack;

    //For timer
    [SerializeField] private float seconds; // How long the timer lasts in seconds
    private float chargeProgress; // Progress through the timer from 0 - 1

    public override void OnStateEnter()
    {
        chargeProgress = 0.0f;
        seconds = stateManager.chargeTime;

        player = stateManager.playerTarget;
        damageAmount = stateManager.damageAmount;
        magicAttack = stateManager.magicAttack;
        spawnPoint = stateManager.spawnPoint;
        chargeUpAttack = stateManager.chargeUpAttack;
        stateManager.FOVCone.color = Color.red;
    }

    public override void RunCurrentState()
    {

        if (stateManager.enemyHealth <= 0)
        {
            stateManager.ChangeState(stateManager.deathState);
        }

        ChaseTarget();
        targetLastPos = stateManager.playerTarget.transform;

        if (chargeProgress < 1.0f)
        {
            //check if player is visible
            if (!CheckVisibility())
            {
                stateManager.ChangeState(stateManager.searchState);
            }

            UpdateTimer(seconds);
        }
        else
        {
            stateManager.ChangeState(stateManager.attackState);
        }
    }

    private void UpdateTimer(float durration)
    {
        if (durration == 0.0f) return; // Avoid dividing by zero

        // Increment the timer and clamp it to a 0 - 1 range.
        chargeProgress = Mathf.Clamp(chargeProgress + Time.deltaTime / durration, 0.0f, 1.0f);
    }
}
