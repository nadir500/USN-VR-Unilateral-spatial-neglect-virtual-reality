using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveLeapMotionCameraWithKinekt : MonoBehaviour
{

    public CheckPointsController checkPointsController;
    public Transform kinectHead;
    public Transform tableWarrper;

    private bool startTracking = false;
    void Start()
    {
        checkPointsController.otherSideCheckPointReachedEvent += Initilize;
		//Debug.Log("start tablePosition = " + tableWarrper.position.ToString());
    }
    // Use this for initialization
    void Initilize()
    {
        kinectHead.GetChild(0).GetComponent<Camera>().enabled = false;
        lastX = kinectHead.localPosition.x;
        lastY = kinectHead.localPosition.y;
        lastZ = kinectHead.localPosition.z;
//		Debug.Log("start tablePosition = " + kinectHead.localPosition.ToString());
        startTracking = true;

    }

    // Update is called once per frame

    float lastX;
    float lastY;
    float lastZ;
    float movementRatio = 0.2f;
    void Update()
    {
        if (startTracking)
        {
            float deltaX;
            float deltaY;
            float deltaZ;
            float headX = kinectHead.localPosition.x;
            float headY = kinectHead.localPosition.y;
            float headZ = kinectHead.localPosition.z;

            deltaX = (headX - lastX) * movementRatio;
            deltaY = (headY - lastY) * movementRatio;
            deltaZ = (headZ - lastZ) * movementRatio;
        	/* Debug.Log("headX = " + headX);
            Debug.Log("headY = " + headY);
            Debug.Log("headZ = " + headZ);
            Debug.Log("lastX = " + lastX);
            Debug.Log("lastY = " + lastY);
            Debug.Log("lastZ = " + lastZ);
            Debug.Log("deltaX = " + deltaX);
            Debug.Log("deltaY = " + deltaY);
            Debug.Log("deltaZ = " + deltaZ);
            Debug.Log("head position = " + kinectHead.position.ToString());
		*/
            tableWarrper.localPosition = new Vector3(tableWarrper.localPosition.x + deltaX, tableWarrper.localPosition.y + deltaY, tableWarrper.localPosition.z/* - deltaZ*/);
           // Debug.Log("tablePosition = " + tableWarrper.position.ToString());

            lastX = headX;
            lastY = headY;
            lastZ = headZ;
        }

    }
}
