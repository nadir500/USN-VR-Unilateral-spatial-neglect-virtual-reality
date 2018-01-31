using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlath : MonoBehaviour {

    public static int currentPath=-1;

    void OnTriggerExit(Collider other)
    {
        //Debug.Log(" player out " + this.transform.parent.name);
        if(other.gameObject.name.Equals("PlayerTrigger"))
        {
            currentPath = -1;
            Debug.Log(" player Exit  " + currentPath);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(" player in " + this.transform.parent.name);
        if (other.gameObject.name.Equals("PlayerTrigger"))
        {
            currentPath = int.Parse(this.gameObject.name);
            Debug.Log(" player Enter  " + currentPath);
        }
    }
}
