using System.Collections;
using UnityEngine;

public class SearchState : State
{
    public StateManager stateManager;
    [SerializeField] private bool isRotating;
    private State returnState;
    public static bool canSeePlayer;

    public override State RunCurrentState()
    {
        isRotating = false;
        stateManager.search = true;

        if (!isRotating)
        {
            if (stateManager.canSeePlayer)
            {
                stateManager.search = false;
                return stateManager.chaseState;
            }
            else if (!stateManager.canSeePlayer)
            {
                stateManager.search = true;
                isRotating = true;
                StartCoroutine(LookLeft());

                return returnState;
                //return stateManager.patrolState;
            }
            else
            {
                return this;
            }
        }
        return this;
    }

    public void ChangeState(State state)
    {
        returnState = state;
    }

    // Search for player to the left
    IEnumerator LookLeft()
    {
        Transform thisTransform = stateManager.transform;
        Quaternion start = thisTransform.rotation;

        Quaternion end = Quaternion.LookRotation(stateManager.lookLeft.transform.position - thisTransform.position);
        //Trying to correct the rotation of the drone.
        end.x = 0f;
        end.z = 0f;
        float counter = 0f;

        while (counter < stateManager.searchDuration)
        {
            thisTransform.rotation = Quaternion.Lerp(start, end, counter / stateManager.searchDuration);

            yield return null;
            counter += Time.deltaTime;

            //Debug.Log(Time.deltaTime / stateManager.searchDuration);
            if (stateManager.canSeePlayer)
            {
                stateManager.search = false;

                counter = stateManager.searchDuration + 1;
                ChangeState(stateManager.chaseState);
                yield return null;
            }
        }

        StartCoroutine(LookRight());
        yield return null;
    }

    // Search for player to the right
    IEnumerator LookRight()
    {
        Transform _thisTransform = stateManager.transform;
        Quaternion _start = _thisTransform.rotation;
        Quaternion _end = Quaternion.LookRotation(stateManager.lookRight.transform.position - _thisTransform.position);
        _end.x = 0f;
        _end.z = 0f;

        float _counter = 0f;

        while (_counter < stateManager.searchDuration)
        {

            _thisTransform.rotation = Quaternion.Lerp(_start, _end, _counter / stateManager.searchDuration);

            yield return null;
            _counter += Time.deltaTime;
            //Debug.Log(Time.deltaTime / stateManager.searchDuration);
            if (stateManager.canSeePlayer)
            {
                stateManager.search = false;

                _counter = stateManager.searchDuration + 1;
                ChangeState(stateManager.chaseState);
                yield return null;
            }
        }
        stateManager.search = false;

        ChangeState(stateManager.patrolState);
        yield return null;
    }
}
