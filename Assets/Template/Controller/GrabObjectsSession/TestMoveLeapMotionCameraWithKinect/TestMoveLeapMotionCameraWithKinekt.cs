using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveLeapMotionCameraWithKinekt : MonoBehaviour
{

    public Transform kinectHead;
    public Transform tableWarrper;

    private bool startTracking = false;
    void Start()
    {
         Initilize();
        //Debug.Log("start tablePosition = " + tableWarrper.position.ToString());
    }
    // Use this for initialization
    void Initilize()
    {
        DisableKinectCamera();
        lastX = kinectHead.localPosition.x;
        lastY = kinectHead.localPosition.y;
        lastZ = kinectHead.localPosition.z;
        //		Debug.Log("start tablePosition = " + kinectHead.localPosition.ToString());
        startTracking = true;

    }
    void DisableKinectCamera()
    {
        Debug.Log("KINECt HEAD " + kinectHead.gameObject.name);
        kinectHead.GetChild(0).gameObject.SetActive(false);
    }
    // Update is called once per frame

    float lastX;
    float lastY;
    float lastZ;
    float movementRatio = 0.1f;
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
            tableWarrper.localPosition = new Vector3(tableWarrper.localPosition.x + deltaX, tableWarrper.localPosition.y + deltaY, tableWarrper.localPosition.z/* - deltaZ*/);
            lastX = headX;
            lastY = headY;
            lastZ = headZ;
        }

    }
}
