using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
public class TableGrabSceneController : MonoBehaviour
{


    AudioSource audioSource;
    GameObject fakefadechild;
    GameObject kinectCamera;
    GameObject lastKinektCamera;

    void Start()
    {
        VRSettings.enabled = true;
        kinectCamera = GameObject.Find("Camera") as GameObject;//Camera.main.gameObject;
        fakefadechild = GameObject.Find("FadeFakeChildKinect") as GameObject;
        backToOtherSideRemoveFade();

    }
    private void backToOtherSideRemoveFade()
    {
        lastKinektCamera = kinectCamera;
        InvokeRepeating("SearchKineckCamera", 0, 0.09f);
    }
    private void SearchKineckCamera()
    {

        GameObject tempKinectCamera = GameObject.Find("CenterEyeAnchor") as GameObject;
        kinectCamera = (tempKinectCamera != null) ? tempKinectCamera : kinectCamera;

        GameObject fakeFadeChildTemp = GameObject.Find("FadeFakeChildLeap") as GameObject;
        fakefadechild = (fakeFadeChildTemp != null) ? fakeFadeChildTemp : fakefadechild;

        if (lastKinektCamera != kinectCamera && fakefadechild != fakeFadeChildTemp)
            CancelInvoke("SearchKineckCamera");
    }
    void LateUpdate()
    {
        Debug.Log("Camera Current Parent " + fakefadechild.transform.parent.transform.parent.gameObject.name);

    }
}

