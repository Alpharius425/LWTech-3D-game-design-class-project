using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentBoxCollider : MonoBehaviour
{
    new BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.transform.parent = this.transform;
    }
}
