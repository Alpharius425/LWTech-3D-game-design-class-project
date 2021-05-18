using System.Collections;
using UnityEngine;

public class SearchState : State
{
    public StateManager stateManager;
    private bool isRotating;
    private State returnState;


    public override State RunCurrentState()
    {
        isRotating = false;
        if (!isRotating)    
        {
            if (stateManager.canSeePlayer)
            {
                return stateManager.chaseState;
            }
            else if (!stateManager.canSeePlayer)
            {
                isRotating = true;
                StartCoroutine(LookLeft());
                return returnState;
                //return stateManager.patrolState;
            }
        }
        return this;
    }

    public void changeState(State state) {
        returnState = state;
    }

    // Search for player to the left
    IEnumerator LookLeft()
    {
        Transform transform = this.stateManager.transform;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(stateManager.lookLeft.transform.position - stateManager.transform.position);
        float counter = 0f;
        while (counter <stateManager.searchDuration)
        {
            stateManager.transform.rotation = Quaternion.Slerp(start, end, counter / stateManager.searchDuration);
            yield return null;
            counter += Time.deltaTime;
            Debug.Log(Time.deltaTime / stateManager.searchDuration);
            if (stateManager.canSeePlayer)
            {
                counter = stateManager.searchDuration + 1;
                changeState(stateManager.chaseState);
                yield return null;
            }
        }

        StartCoroutine(LookRight());
        yield return null;
    }

    // Search for player to the right
    IEnumerator LookRight()
    {
        Transform transform = this.stateManager.transform;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(stateManager.lookRight.transform.position - stateManager.transform.position);
        float counter = 0f;
        while (counter < stateManager.searchDuration)
        {
            stateManager.transform.rotation = Quaternion.Slerp(start, end, counter / stateManager.searchDuration);
            yield return null;
            counter += Time.deltaTime;
            //Debug.Log(Time.deltaTime / stateManager.searchDuration);
            if (stateManager.canSeePlayer)
            {
                counter = stateManager.searchDuration + 1;
                changeState(stateManager.chaseState);
                yield return null;
            }
        }
        changeState(stateManager.patrolState);
        yield return null;
    }
}
