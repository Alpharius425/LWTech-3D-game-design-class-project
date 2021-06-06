using System.Collections;
using UnityEngine;

public class SearchState : State
{
    private float yawProgress;
    private int searchCyclesCurrent = 0;
    private int searchCyclesMax = 1;
    Quaternion cachedRotation;

    public SearchState(StateManager manager) : base(manager)
    {

    }

    public override void OnStateEnter()
    {
        cachedRotation = stateManager.transform.rotation;
        stateManager.FOVCone.color = Color.yellow;
    }

    public override void RunCurrentState()
    {
        if (stateManager.enemyHealth <= 0)
        {
            stateManager.ChangeState(stateManager.deathState);
        }

        if (CheckVisibility())
        {
            stateManager.ChangeState(stateManager.chaseState);
        }

        LookRightLeft();
    }

    private void LookRightLeft()
    {
        float fullCycle = 2.0f * Mathf.PI;
        float searchAmplitude = 90.0f; //degrees
        float searchFrequency = 1.0f; //speed
        float searchAngle = Mathf.Sin(yawProgress) * searchAmplitude;
        yawProgress += Time.deltaTime * searchFrequency;

        if (yawProgress >= fullCycle)
        {
            yawProgress -= fullCycle;
            searchCyclesCurrent++;

            if (searchCyclesCurrent >= searchCyclesMax)
            {
                stateManager.ChangeState(stateManager.patrolState);
            }
        }

        Quaternion searchRotation = Quaternion.Euler(0.0f, searchAngle, 0.0f);
        stateManager.transform.rotation = cachedRotation * searchRotation;
    }
}
