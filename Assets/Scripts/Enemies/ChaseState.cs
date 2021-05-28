using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(StateManager manager) : base(manager)
    {

    }

    public override void OnStateEnter()
    {
        stateManager.FOVCone.color = Color.red;
        stateManager.animator.SetBool("chasing", true);
        stateManager.animator.SetBool("dead", false);
        stateManager.animator.SetBool("patrol", false);
    }

    public override void RunCurrentState()
    {
        ChaseTarget();
        if (CheckVisibility())
        {
            stateManager.ChangeState(stateManager.chargeUpAttackState);
        }
        else
        {
            stateManager.ChangeState(stateManager.searchState);
        }
    }
}
