using System;
using UnityEngine;


    // This class is tha manager of the check points their events
    //  it create the checkpoints in checkPoints with the positions calculated in Start()->generateCheckPoints() method by the variables in RoadController
    //  then manualy assign the currect method to the behavior event at each checkpoint
    //  each metho is liked to a "global" event and trigger it to aware each class lined to that event

public class CheckPointsController : MonoBehaviour {
    public GameObject yellowPoint;          // reference to the prafab of yellow point of   |   assigned in the inspector to the resources/prafabs/yellowPoint gameObject
    private GameObject[] checkPoints;       // array of referenses to each checkpoint instantiated
                                            // 0=> checkpoint at first sidewalk to be sure that player is ready to crossing the roads
                                            // 1=> checkpoint at the close side of mid-walk to start the fade after crossing the first direction
                                            // 2=> checkpoint at the far side of mid-walk to be sure that is the player on the mid-walk after polling the table of kinekt
                                            // 3=> checkpoint at the other side

    public delegate void checkPointsReached();
    public checkPointsReached startTheGameCheckPointReachedEvent;
    public checkPointsReached midWalkCheckPointReachedEvent;
    public checkPointsReached backToMidWalkCheckPointReachedEvent;
    public checkPointsReached otherSideCheckPointReachedEvent;

    /********************This should be removed by(it was "my" --Edited by nadir pervez :p --) Mr nadir prevez*****************/
    Fading fadeObject;
    LayerMask uiMask = (1 << 5);
    /******************************************************************************/
    Transform KVR;

    // Use this for initialization
    void Start () {
        checkPoints = new GameObject[4];
        initilizeCheckPoints();
        KVR = GameObject.Find("OnlineBodyView").transform;
    }

    private void initilizeCheckPoints()
    {
        checkPoints[0] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + 0.5f, -0.5f, -8.98f), Quaternion.identity);
        checkPoints[0].GetComponent<CheckPoints>().behaviorEvent += startTheGame;
        checkPoints[0].SetActive(true);
        checkPoints[1] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth / 2) + RoadController.streetPathWidth * (ExperementParameters.numberOfPathsPerStreet / 2) - 0.35f, -0.5f, -8.98f), Quaternion.identity);
        checkPoints[1].GetComponent<CheckPoints>().behaviorEvent += reachedToTheMidWalk;
        checkPoints[1].SetActive(false);
        checkPoints[2] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth / 2) + RoadController.streetPathWidth * (ExperementParameters.numberOfPathsPerStreet / 2) + 0.35f, -0.5f, -8.98f), Quaternion.identity);
        checkPoints[2].GetComponent<CheckPoints>().behaviorEvent += backToMidWalk;
        checkPoints[2].SetActive(false);
        checkPoints[3] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth) + RoadController.streetPathWidth * (ExperementParameters.numberOfPathsPerStreet) + 0.25f, -0.5f, -8.98f), Quaternion.identity);
        checkPoints[3].GetComponent<CheckPoints>().behaviorEvent += reachedToOtherSide;
        checkPoints[3].SetActive(false);
    }

    // turn of the first sidewalk checkpoint
    // turn on the second side checkpoint
    // TODO(0): start generating the cars after calling this event

    public void startTheGame()
    {
        Debug.Log("startTheGame");
        checkPoints[0].SetActive(false);
        checkPoints[1].SetActive(true);
        if (startTheGameCheckPointReachedEvent != null)
        {
            startTheGameCheckPointReachedEvent();
        }
    }

    // turn of the midwalk close side checkpoint
    // turn on midwalk close side checkpoint
    // TODO(0): start fade
    public void reachedToTheMidWalk()
    {
        Debug.Log("reachedToTheMidWalk");
        StartCoroutine(fadeObject.playSound("Stop"));
        checkPoints[1].SetActive(false);

        RoadController.fadeout_after_crossing = false;

        fadeObject.BeginFade(2);  //fade entirely and wait for re-positioning 
        Camera.main.cullingMask = uiMask;
        KVR.localPosition = new Vector3(KVR.localPosition.x - 6.39f, KVR.localPosition.y, KVR.localPosition.z);

        checkPoints[2].SetActive(true);

        // Trigger The Event for each on who linked ot it

        if (startTheGameCheckPointReachedEvent != null)
            midWalkCheckPointReachedEvent();
    }
    // turn of the midwalk far side checkpoint
    // turn on other sidewalk checkpoint
    // TODO(0): stop the fade
    public void backToMidWalk()
    {
        Debug.Log("backToMidWalk");
        checkPoints[2].SetActive(false);
        //سلوك
        checkPoints[3].SetActive(true);

        if(backToMidWalkCheckPointReachedEvent != null)
            backToMidWalkCheckPointReachedEvent();
    }

    // turn of the sidewalk checkpoint
    public void reachedToOtherSide()
    {
        Debug.Log("reachedToOtherSide");
        checkPoints[3].SetActive(false);

        if(otherSideCheckPointReachedEvent != null)
            otherSideCheckPointReachedEvent();
    }
}
