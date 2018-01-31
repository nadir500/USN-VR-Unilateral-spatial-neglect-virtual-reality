using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlath : MonoBehaviour {


    void OnTriggerExit(Collider other)
    {
        Debug.Log(" player out " + this.transform.parent.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(" player in " + this.transform.parent.name);
    }
}
