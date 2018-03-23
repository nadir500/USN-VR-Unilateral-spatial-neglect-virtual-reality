using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StupidWalk : MonoBehaviour {

   /* public Transform vrCamera;

    public float targetAngle;

    public float speed = 3.0f;

    public bool moveForward;

    private CharacterController cc;*/
	// Use this for initialization
    float spanTime ; 
    float result;
    
	void Start () {
      //  cc = GetComponent<CharacterController>();
    Debug.Log("Size " +  this.gameObject.GetComponent<Renderer>().bounds.size);
	}
	
	// Update is called once per frame
	void Update () {
      
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

    void threads()
    {
        result = Mathf.Round((Time.time - spanTime) *1000) /1000;

    }
}
