using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour {

    private List<Vector3> playerPositions;
    private List<Vector2> playerHeadRotations;

    public GameObject mainCamera;
    public GameObject onlineBodyView;
    private Transform SpineMid = null;

    //public GameObject car;

    bool onFrameWorking;
    // Use this for initialization
    public void Initilize () {
        onFrameWorking = false;
        playerPositions = new List<Vector3>();
        playerHeadRotations = new List<Vector2>();


        InvokeRepeating("searchOnPlayer", 1f, 1f);
    }

    /*****temp varialbes****/
    float angle;
    int lastAngla = 90;
    int currentAngle = 90;
    long frameIndex = 0;
    /***********************/
    void onFrame()
    {
        Debug.Log("frame");
        angle = mainCamera.transform.localRotation.eulerAngles.y;
        playerHeadRotations.Add(new Vector2(frameIndex, angle));
        currentAngle = (int)angle;
        if (currentAngle != lastAngla)
        {
            //MyConsol.log(currentAngle.ToString());
            lastAngla = currentAngle;
        }
        
        playerPositions.Add(SpineMid.position);
        Debug.Log("pos = "+ SpineMid.position);

        //Vector3 screenPoint = mainCamera.GetComponent<Camera>().WorldToViewportPoint(car.transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //Debug.Log("x = "+ screenPoint.x+","+ "y = " + screenPoint.y + "," + "z = " + screenPoint.z);
        //if (onScreen)
        //      Debug.Log(" i can see a fucken car");

        frameIndex++;
    }
    void searchOnPlayer()
    {
        Debug.Log("searchOnPlayer");
        try
        {
            if (SpineMid == null)
            {
                if (onFrameWorking)
                {
                    onFrameWorking = false;
                    
                    CancelInvoke("onFrameWorking");
                }
                if (onlineBodyView.transform.GetChild(0) != null)
                {
                    SpineMid = onlineBodyView.transform.GetChild(0).transform.Find("SpineMid");
                    onlineBodyView.transform.GetChild(0).name = "Player";
                }
                else
                    Debug.Log("no children");

                if(SpineMid != null)
                    Debug.Log(" on framework will start in next iteration");
                else
                    Debug.Log(" on SpineMid NOT found");
            }
            else if (!onFrameWorking)
            {
                Debug.Log(" on framework will start");
                InvokeRepeating("onFrame", 1f, 0.0333f);
                onFrameWorking = true;
            }

        }
        catch (System.Exception e)
        {
            //MyConsol.log(e.Message.ToString());
        }
    }
}
