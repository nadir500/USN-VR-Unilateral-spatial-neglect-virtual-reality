using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour {

    //  This class is attached to each yellow point (check point)
    //  it only has on method OnTriggerEnter only with player and call the behavior method that assigned in CheckPointController class
    public delegate void behavior();
    public behavior behaviorEvent;

    void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals(value: "PlayerTrigger"))
            behaviorEvent();
    }
}
