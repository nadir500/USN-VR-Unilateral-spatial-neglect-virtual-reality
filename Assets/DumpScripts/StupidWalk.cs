using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidWalk : MonoBehaviour {

   /* public Transform vrCamera;

    public float targetAngle;

    public float speed = 3.0f;

    public bool moveForward;

    private CharacterController cc;*/
	// Use this for initialization
    float spanTime ; 
	void Start () {
      //  cc = GetComponent<CharacterController>();

        spanTime = Time.time;
        Debug.Log("TIIIIIIIIIIIIME " + spanTime);
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("TIME IN GAME " + (Mathf.Round((Time.time - spanTime) *1000))); 
	/*	if(vrCamera.eulerAngles.x >= -20.0f && vrCamera.eulerAngles.x <= 20.0f)
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }
        if(moveForward)
        {
            Vector3 forward = vrCamera.TransformDirection(Vector3.forward);
            cc.SimpleMove(forward * speed);
        }*/
	}
}
