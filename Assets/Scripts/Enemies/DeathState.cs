using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public DeathState(StateManager manager) : base(manager)
    {

    }

    public float waitTime = 5.0f;
    public float currentTime = 0.0f;

    public override void OnStateEnter()
    {
        //Turn off the light when enemy dies.
        stateManager.FOVCone.enabled = false;

        //Start the death animation.
        stateManager.animator.SetBool("dead", true);

        //Turn gravity on so the enemy appears to fall when dying.
        stateManager.rb.useGravity = true;
        //Physics.gravity = new Vector3(0, 10f, 0);

        //Grant's Additions
        stateManager.droneAudio.clip = stateManager.mutteringBroken;
        stateManager.droneAudio.Play();
        stateManager.propellerAudio.clip = stateManager.helicopterCrash;
        stateManager.propellerAudio.Play();
        GameObject.Instantiate(stateManager.deathExplosion, stateManager.spawnPoint.transform.position, Quaternion.identity); //create the magic bolt

        GameObject.Destroy(stateManager.gameObject, 1f);

    }

    public override void RunCurrentState()
    {
    }

}
