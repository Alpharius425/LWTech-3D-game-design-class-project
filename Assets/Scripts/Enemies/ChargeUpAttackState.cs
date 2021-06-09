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
    private bool particlesHaveFired = false;

    //For timer
    private float seconds; // How long the timer lasts in seconds
    private float chargeProgress; // Progress through the timer from 0 - 1

    public override void OnStateEnter()
    {
        seconds = stateManager.reloadTime + stateManager.chargeTime;
        chargeProgress = 0.0f;
        particlesHaveFired = false;
        player = stateManager.playerTarget;
        damageAmount = stateManager.damageAmount;
        magicAttack = stateManager.magicAttack;
        spawnPoint = stateManager.spawnPoint;
        chargeUpAttack = stateManager.chargeUpAttack;
        stateManager.FOVCone.color = Color.red;
        
    }

    public override void RunCurrentState()
    {
        ChaseTarget();
        targetLastPos = stateManager.playerTarget.transform;

        if (chargeProgress < seconds)
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

    private void UpdateTimer(float duration)
    {
        // Increment the timer and clamp it to a 0 - 1 range.
        chargeProgress = Mathf.Clamp(chargeProgress + Time.deltaTime, 0.0f, duration);
        if (!particlesHaveFired && chargeProgress >= stateManager.reloadTime)
        {
            particlesHaveFired = true;
            GameObject newParticles = GameObject.Instantiate(chargeUpAttack, stateManager.spawnPoint.transform.position, Quaternion.identity); //create a charging effect
            newParticles.transform.parent = stateManager.transform;
            stateManager.gunAudio.PlayOneShot(stateManager.chargeSound); //set the audio clip
        }
    }
}
