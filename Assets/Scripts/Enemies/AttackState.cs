using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Windows;

public class AttackState : State
{
    public AttackState(StateManager manager) : base(manager)
    {

    }

    public static GameObject projectile;
    public static GameObject projectileCharge;
    public static Transform targetLastPos;
    public static Vector3 shootDirection;
    private int damageAmount;
    [HideInInspector] public static GameObject magicAttack;
    [HideInInspector] public static GameObject spawnPoint;
    [HideInInspector] public static GameObject magicCharge;

    //For timer
    [SerializeField] private float seconds; // How long the timer lasts in seconds
    private float chargeProgress; // Progress through the timer from 0 - 1

    public override void OnStateEnter()
    {
        damageAmount = stateManager.damageAmount;
        magicAttack = stateManager.magicAttack;
        //Grant's additions
        magicCharge = stateManager.chargeUpAttack;
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
        stateManager.gunAudio.clip = stateManager.fireSound;
        stateManager.gunAudio.Play();
        projectile = GameObject.Instantiate(magicAttack, stateManager.spawnPoint.transform.position, Quaternion.identity); //create the magic bolt
    }
}
