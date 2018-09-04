using System;
using UnityEngine;


// This class is tha manager of the check points their events
//  it creates the checkpoints in checkPoints with the positions calculated in Start()->generateCheckPoints() method by the variables in RoadController
//  then manually assign the current method to the behavior event at each checkpoint
//  each method is liked to a "global" event and trigger it to aware each class lined to that event

public class CheckPointsController : MonoBehaviour
{
    public GameObject yellowPoint;          // reference to the prefab of yellow point of   |   assigned in the inspector to the resources/prafabs/yellowPoint gameObject
    private GameObject[] checkPoints;       // array of references to each checkpoint instantiated
                                            // 0=> checkpoint at first sidewalk to be sure that player is ready to crossing the roads
                                            // 1=> checkpoint at the close side of mid-walk to start the fade after crossing the first direction
                                            // 2=> checkpoint at the far side of mid-walk to be sure that is the player on the mid-walk after polling the table of kinekt
                                            // 3=> checkpoint at the other side

    public delegate void checkPointsReached();
    public checkPointsReached startTheGameCheckPointReachedEvent;
    public checkPointsReached midWalkCheckPointReachedEvent;
    public checkPointsReached backToMidWalkCheckPointReachedEvent;
    public checkPointsReached otherSideCheckPointReachedEvent;
    public checkPointsReached backToOtherSideCheckPointReachedEvent;
    public checkPointsReached crossingRoadPhaseFinishedEvent;
    public bool isHitByCar = false;
    public AudioController audioController;
    public GameObject serverNetworkController;
    public GameObject LeapEventSystem;
    public GameObject UIEventSystem;
    public RoadController roadController;
    private int number_of_accidents_per_player = 0;
    private DataService _sqlite_connection_gamoplay;
    CrossingRoad crossingRoad;

    /* those variables represent some of the project features that we didn't make a prefect result from it*/
    Fading fadeController;
    LayerMask uiMask = (1 << 5);
    /***********************************END*******************************************/
    public GameObject KVR;
    GameClient gameClientController;

    // Use this for initialization
    public void StartAfterMainMenu()
    {
        checkPoints = new GameObject[4];
        ObjectsInitializer();
        initializeCheckPoints();
        InitializeCar();
    }
    private void ObjectsInitializer()
    {
        gameClientController = GameObject.Find("GameClient").GetComponent<GameClient>();  //for sending Data to server
        crossingRoad = GameObject.Find("PlayerTrigger").GetComponent<CrossingRoad>();  //for making an event to it with its trigger
        fadeController = GameObject.Find("FadeController").GetComponent<Fading>();
        _sqlite_connection_gamoplay = new DataService("USN_Simulation.db");

    }
    private void initializeCheckPoints()
    {
        //the comments in this method represents the test units of the checkpoints events (it's for fast testing if everything works fine)
        checkPoints[0] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth - 0.45f, -0.5f, -8.98f), Quaternion.identity);
        checkPoints[0].GetComponent<CheckPoints>().behaviorEvent += startTheGame;
        checkPoints[0].SetActive(true);

        //  checkPoints[1] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + 0.5f, -0.5f, -8.98f), Quaternion.identity);
        Debug.Log(ExperimentParameters.streetsDirections);
        string[] streetsDirections = ExperimentParameters.streetsDirections.Split(' ');
        if (streetsDirections.Length > 1)
        {
            Debug.Log("Parameters Condition Exceeds Length ");
            checkPoints[1] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth / 2) + RoadController.streetPathWidth * (ExperimentParameters.lanes_per_direction / 2) - 0.35f, -0.5f, -8.98f), Quaternion.identity);
            checkPoints[1].GetComponent<CheckPoints>().behaviorEvent += reachedToTheMidWalk;
            checkPoints[1].SetActive(false);

            // checkPoints[2] = Instantiate(yellowPoint,  new Vector3(RoadController.sidewalkWidth + 0.5f, -0.5f, -8.98f), Quaternion.identity);
            checkPoints[2] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth / 2) + RoadController.streetPathWidth * (ExperimentParameters.lanes_per_direction / 2) + 0.35f, -0.5f, -8.98f), Quaternion.identity);

            checkPoints[2].GetComponent<CheckPoints>().behaviorEvent += backToMidWalk;
            checkPoints[2].SetActive(false);

            //the sidewalk 1st ball
            //checkPoints[3] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + 0.5f, -0.5f, -8.98f), Quaternion.identity);
            checkPoints[3] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth) + RoadController.streetPathWidth * (ExperimentParameters.lanes_per_direction) + 0.25f, -0.5f, -8.98f), Quaternion.identity);

            checkPoints[3].GetComponent<CheckPoints>().behaviorEvent += reachedToOtherSide;
            checkPoints[3].SetActive(false);

        }
        else    // single direction experiment
        {
            checkPoints[3] = Instantiate(yellowPoint, Vector3.zero, Quaternion.identity);
            //checkPoints[3].transform.position = new Vector3( roadController.BuildingsWrapper.transform.position.x, -0.5f, -8.98f);
            checkPoints[3].GetComponent<CheckPoints>().behaviorEvent += crossingRoadPhaseFinished;
            checkPoints[3].SetActive(true);
            // checkPoints[1] = Instantiate(yellowPoint, new Vector3(RoadController.sidewalkWidth + (RoadController.midwalkWidth / 2) + RoadController.streetPathWidth * (ExperimentParameters.lanes_per_direction / 2) + 0.35f, -0.5f, -8.98f), Quaternion.identity);

            //   checkPoints[1].GetComponent<CheckPoints>().behaviorEvent += crossingRoadPhaseFinished;
            //  checkPoints[1].SetActive(false);
        }
        // serverNetworkController.transform.position = checkPoints[3].transform.position;



    }
    //Initialize the same way with checkpoints array 
    private void InitializeCar()
    {
        //Initialize the event from CrossingRoad Class 
        crossingRoad.WhenHitByCar += Accident;
    }
    // turn of the first sidewalk checkpoint
    // turn on the second side checkpoint
    // TODO(0): start generating the cars after calling this event(DONE)
    public void crossingRoadPhaseFinished()
    {
        isHitByCar = false;

        if (crossingRoadPhaseFinishedEvent != null)
        {
            crossingRoadPhaseFinishedEvent();
        }
        //    UnityEngine.PlayerPrefs.SetInt("isSettingsChanged", 0);

        //connect to DB 
        _sqlite_connection_gamoplay.UpdateGameplayCrossingStatus(number_of_accidents_per_player, ExperimentParameters.gameplay_id);

        Application.LoadLevel(0);
    }

    public void startTheGame()
    {
        Debug.Log("startTheGame");
        checkPoints[0].SetActive(false);
        checkPoints[3].transform.position = new Vector3( roadController.BuildingsWrapper.transform.position.x - 5, -0.5f, -8.98f);

        //        checkPoints[1].SetActive(true);  //changed it :p 
        //do not fade By default
        // RoadController.fadeout_after_crossing = false;
        // gameClientController.SendDataToServer(RoadController.fadeout_after_crossing);  //Intialize the bool Value To the Button Server
        UnityEngine.PlayerPrefs.SetInt("isSettingsChanged", 0);

        if (startTheGameCheckPointReachedEvent != null)
        {
            startTheGameCheckPointReachedEvent();
        }
    }

    // turn of the midwalk close side checkpoint
    // turn on midwalk close side checkpoint
    // TODO(0): start fade (DONE)
    public void reachedToTheMidWalk()
    {
        Debug.Log("reachedToTheMidWalk");
        isHitByCar = false; //initializing the after_collision_frame again when i reach the midwalk  

        audioController.playAudioClip("DRSounds/StepBackward", 0, -1);
        checkPoints[1].SetActive(false);
        //Begin the Phase 2 fade 
        //now fade and show the loading screen
        RoadController.fadeout_after_crossing = true;
        //sending the current value to the server

        fadeController.BeginFade(2);  //fade entirely and wait for re-positioning 
        //seeing  JUST the UI From Camera 
        Camera.main.cullingMask = uiMask;
        KVR.transform.localPosition = new Vector3(KVR.transform.localPosition.x + 6.39f, KVR.transform.localPosition.y, KVR.transform.localPosition.z);
        checkPoints[2].SetActive(true);
        // Trigger The Event for each on who linked ot it
        if (midWalkCheckPointReachedEvent != null)
            midWalkCheckPointReachedEvent();
    }
    // turn of the midwalk far side checkpoint
    // turn on other sidewalk checkpoint
    // TODO(0): stop the fade (DONE)
    public void backToMidWalk()
    {
        Debug.Log("backToMidWalk");
        checkPoints[2].SetActive(false);
        audioController.playAudioClip("DRSounds/Stop", 0, -1);
        checkPoints[3].SetActive(true);
        // RoadController.fadeout_after_crossing = true;
        // gameClientController.SendDataToServer(RoadController.fadeout_after_crossing); JUNK CODE 
        if (backToMidWalkCheckPointReachedEvent != null)
            backToMidWalkCheckPointReachedEvent();
    }

    // turn of the sidewalk checkpoint
    public void reachedToOtherSide()
    {
        RoadController.fadeout_after_crossing = true;
        fadeController.BeginFade(2);  //fade entirely and wait for re-positioning 
                                      //RoadController.fadeout_after_crossing = true;
                                      //seeing the JUST the UI From Camera 
        Camera.main.cullingMask = uiMask;
        audioController.playAudioClip("DRSounds/StepBackward", 0, -1);

        Debug.Log("reachedToOtherSide");
        isHitByCar = false;
        //checkPoints[3].SetActive(false);
        KVR.transform.localPosition = new Vector3(KVR.transform.localPosition.x + 5.39f, KVR.transform.localPosition.y, KVR.transform.localPosition.z);
        if (otherSideCheckPointReachedEvent != null)
            otherSideCheckPointReachedEvent();
        checkPoints[3].GetComponent<CheckPoints>().behaviorEvent -= reachedToOtherSide;
        checkPoints[3].GetComponent<CheckPoints>().behaviorEvent += BackToTheOtherSide;
    }
    public void BackToTheOtherSide()
    {
        serverNetworkController.SetActive(true);
        audioController.playAudioClip("DRSounds/Stop", 0, -1);

        checkPoints[3].SetActive(false);
        //do not make any fade (not until our phase 3)
        RoadController.fadeout_after_crossing = true;
        //sending the actual value to the server

        gameClientController.SendDataToServer(RoadController.fadeout_after_crossing);

        if (backToOtherSideCheckPointReachedEvent != null)
            backToOtherSideCheckPointReachedEvent();
    }
    //On Trigger Enter With any car 
    public void Accident()
    {

        //continuos fade after passing 1 to the method 
        fadeController.BeginFade(1);
        //it will be useful for entering the data from Experiment Observe Class
        isHitByCar = true;

        //when accident occur successful crosses zero and accidents has values
        Debug.Log("ACCIDENT HAPPENED");
        number_of_accidents_per_player++;

    }
    void EnableLeapEventSystem()
    {
        LeapEventSystem.SetActive(true);
    }
}
