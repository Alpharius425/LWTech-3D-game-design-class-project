using System.Collections;
using UnityEngine;

public class SearchState : State
{
    public StateManager stateManager;
    private bool isRotating;
    public float rotateSpeed;
    public float duration;
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

    IEnumerator LookLeft()
    {
        Transform transform = this.stateManager.transform;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(stateManager.lookLeft.transform.position - stateManager.transform.position);
        float counter = 0f;
        while (counter < duration)
        {
            stateManager.transform.rotation = Quaternion.Slerp(start, end, counter / duration);
            yield return null;
            counter += Time.deltaTime;
            Debug.Log(Time.deltaTime / duration);
            if (stateManager.canSeePlayer)
            {
                counter = duration + 1;
                changeState(stateManager.chaseState);
                yield return null;
            }
        }

        StartCoroutine(LookRight());
        yield return null;
    }

    IEnumerator LookRight()
    {
        Transform transform = this.stateManager.transform;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(stateManager.lookRight.transform.position - stateManager.transform.position);
        float counter = 0f;
        while (counter < duration)
        {
            stateManager.transform.rotation = Quaternion.Slerp(start, end, counter / duration);
            yield return null;
            counter += Time.deltaTime;
            Debug.Log(Time.deltaTime / duration);
            if (stateManager.canSeePlayer)
            {
                counter = duration + 1;
                changeState(stateManager.chaseState);
                yield return null;
            }
        }
        changeState(stateManager.patrolState);
        yield return null;
    }
}
