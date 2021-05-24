using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    /*
     * Just a utility class to make an object spin around.
     */
    [SerializeField] float spinSpeed = 1;

    void Update()
    {
        gameObject.transform.Rotate(0f, spinSpeed, 0f, Space.Self);
    }
}
