using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour
{
    /* this is where we can record our data about the patient crossing the road session (not the grabbing process)
       
     */
    public CheckPointsController checkPointsController;

    private Vector3[] playerPositions;      //taken from spineMid GameObject position
    private float[] playerHeadRotations;    // taken from the camira
    public bool[] isLookingAtCar;           // taken from CarMove class
    public string[] traffic_towards_flow;   //determine which direction the player is on 
    public float[] current_time_span;  //current time capturing the data 
    public bool[] is_hit_by_car;        //current state of the player when recording the data (is hit by a car ot not)
    public string current_traffic_towards_flow;  //where is the player right now is it on the left side road? or the right side

    public GameObject mainCamera;               // reference to the camira in the hierarchy
    public GameObject onlineBodyView;           // reference to onlineBodyVew (just used to find the spineMid at his grandsons
    private Transform SpineMid = null;          // reference to spineMid (Kinect creates it at runtime)
    private bool onFrameWorking;        // true => the 30 frame per second method is working after finding the spineMid | false  => the 30 frame per second method is NOT working
    private float observeFrameRate;  //the framerate from experimental parameters 
    private float timeSinceReachTheFirstYellowPoint;  //to calculate the start point of executing OnFrame in invoke 
    private string leftSideString; //getting the left side of the ExperimentParameters.streetsDirections string to determine if the player is on the left side or right side (R/L)
    DataService _crossing_road_connection;
    ObservedData observedData;


    // Use this for initialization
    void Start()
    {
        onFrameWorking = false;
        _crossing_road_connection = new DataService("USN_Simulation.db");
        checkPointsController.startTheGameCheckPointReachedEvent += Initialize;
        checkPointsController.backToMidWalkCheckPointReachedEvent += OnChangeTrafficTowardsFlow;
    }
    public void Initialize()
    {
        //initialize the framerate value (i made it like this cuz i want to reduce some operations :) )
        float frameRateInitialize = float.Parse(ExperimentParameters.observeFrameRate);
        observeFrameRate = 1 / frameRateInitialize; //now getting the invoke reapeting rate 

        //those arrays are here for recording the data to DB every 2 invokes (performance matter) 
        playerPositions = new Vector3[3];  //player position array for recording 
        playerHeadRotations = new float[3]; //player head rotation as array for recording purpose 
        traffic_towards_flow = new string[3]; //recording the current road the player is on
        isLookingAtCar = new bool[3]; //is he looking to a car
        current_time_span = new float[3]; //time span since OnFrame() started 
        is_hit_by_car = new bool[3];
        //getting the initial state of the first road 
        leftSideString = ExperimentParameters.streetsDirections.Split(' ')[0][0].ToString();
        current_traffic_towards_flow = leftSideString;
        //time since touching the first yellow point
        timeSinceReachTheFirstYellowPoint = Time.time;
        //invoke the method
        InvokeRepeating("searchOnPlayer", 1f, observeFrameRate);
    }

    /*****temp variables****/
    float angle;
    int lastAngle = 90;
    int currentAngle = 90;
    long frameIndex = 0;
    /***********************/
    int frame = 0;      // initialize with zero
    void OnFrame()
    {
        //recording the data 
        angle = mainCamera.transform.localRotation.eulerAngles.y;
        playerPositions[frameIndex] = SpineMid.position;
        playerHeadRotations[frameIndex] = angle;
        traffic_towards_flow[frameIndex] = current_traffic_towards_flow;
        current_time_span[frameIndex] = Mathf.Abs(Mathf.Round((Time.time - timeSinceReachTheFirstYellowPoint) * 1000) / 1000);
        isLookingAtCar[frameIndex] = CarMove.numberOfRenderedCars > 0;
        is_hit_by_car[frameIndex] = checkPointsController.isHitByCar;

        frameIndex++;
        if (frameIndex == 2)   // you can use this as the index of the lists
        {
            Debug.Log("ON FRAME ");
            observedData = new ObservedData(playerPositions, playerHeadRotations, isLookingAtCar, traffic_towards_flow, current_time_span, is_hit_by_car);
            //connection to database in a thread 
            Thread connectionDBThread = new Thread(() => ConnectionToDB());

            connectionDBThread.Start();
            if (!connectionDBThread.IsAlive)
            {
                connectionDBThread.Abort();
            }
            frameIndex = 0;     // back to zero after each send
        }

    }
    private void ConnectionToDB()
    {
        _crossing_road_connection.CreateRoadCrossingData(observedData);
    }

    //check the player if the kinect sees him then we're ready to record out movements data 
    void searchOnPlayer()
    {
        try
        {
            if (SpineMid == null)  //kinect failed in seeing the patient 
            {
                if (onFrameWorking)
                {
                    onFrameWorking = false;

                    CancelInvoke("OnFrame");
                }
                if (onlineBodyView.transform.GetChild(0) != null)
                {
                    SpineMid = onlineBodyView.transform.GetChild(0).transform.Find("SpineMid");
                    onlineBodyView.transform.GetChild(0).name = "Player";
                }

            }
            else if (!onFrameWorking) //kinect succeed otherwise 
            {
                InvokeRepeating("OnFrame", 0.0f, observeFrameRate);
                onFrameWorking = true;
            }
        }
        catch (System.Exception e)
        {
            //nothing here to see
        }
    }
    //decide when recording our data which direction is the road (L --> from left to right, R --> from right to left) 
    public void OnChangeTrafficTowardsFlow()
    {
        if (this.current_traffic_towards_flow.Equals(value: "L"))
        {
            this.current_traffic_towards_flow = "R";
        }
        else
        {
            this.current_traffic_towards_flow = "L";
        }
    }
    
    /*************** you're entering the junk area code ^_^ ****************/

    /* those variables represent some of the project features that we didn't make a prefect result from it when recording our data*/
    public List<GameObject> carsReferences;
    string[] roadtype;
    /******************************END**************************************/
    /* code about calculating the distance between cars and the patient not executing in the main project   */
    public float CheckDistanceBetweenPlayerAndNearestCar()
    {
        Debug.Log("player on path current path variable " + PlayerOnPlath.currentPath + "and index " + carsReferences.Count);
        if ((PlayerOnPlath.currentPath != -1)
            && (carsReferences[PlayerOnPlath.currentPath] != null))
        {

            roadtype = carsReferences[PlayerOnPlath.currentPath].transform.parent.gameObject.name.Split(' ');
            if ((carsReferences[PlayerOnPlath.currentPath].transform.position.z < SpineMid.transform.position.z) && (roadtype[1].Equals(value: "Left"))
            || ((carsReferences[PlayerOnPlath.currentPath].transform.position.z > SpineMid.transform.position.z) && (roadtype[1].Equals(value: "Right"))))

                return Vector3.Distance(SpineMid.transform.position, carsReferences[PlayerOnPlath.currentPath].transform.position);
            else
                return -1.0f;
        }
        else
            return -1.0f;

    }
}


