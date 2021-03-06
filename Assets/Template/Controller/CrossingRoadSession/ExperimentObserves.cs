﻿using System;
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
    public float[] distance_nearest_car_in_lane;
    public string current_traffic_towards_flow;  //where is the player right now is it on the left side road? or the right side

    public GameObject mainCamera;               // reference to the camira in the hierarchy
    public GameObject onlineBodyView;           // reference to onlineBodyVew (just used to find the spineMid at his grandsons

    public GameObject SpineMid = null;          // reference to spineMid (Kinect creates it at runtime)

    public CarController carController;
    private bool onFrameWorking;        // true => the 30 frame per second method is working after finding the spineMid | false  => the 30 frame per second method is NOT working
    private float observeFrameRate;  //the framerate from experimental parameters 
    private float timeSinceReachTheFirstYellowPoint;  //to calculate the start point of executing OnFrame in invoke 
    private string leftSideString; //getting the left side of the ExperimentParameters.streetsDirections string to determine if the player is on the left side or right side (R/L)
    DataService _crossing_road_connection;
    ObservedData observedData;

    //Distance Parameters
    private int parentNumber;
    private int laneNumber;
    private float finalDistanceResult = 0;
    private GameObject[,] parents2DArray;


    // Use this for initialization
    void Start()
    {
        onFrameWorking = false;
        SpineMid = onlineBodyView;
        _crossing_road_connection = new DataService("USN_Simulation.db");
        checkPointsController.startTheGameCheckPointReachedEvent += Initialize;
        checkPointsController.backToMidWalkCheckPointReachedEvent += OnChangeTrafficTowardsFlow;

    }
    public void Initialize()
    {
        //initialize the framerate value (i made it like this cuz i want to reduce some operations :) )
        float frameRateInitialize = float.Parse(ExperimentParameters.observeFrameRate);
        observeFrameRate = frameRateInitialize / 30; //now getting the invoke reapeting rate 

        //those arrays are here for recording the data to DB every 2 invokes (performance matter) 
        playerPositions = new Vector3[3];  //player position array for recording 
        playerHeadRotations = new float[3]; //player head rotation as array for recording purpose 
        traffic_towards_flow = new string[3]; //recording the current road the player is on
        isLookingAtCar = new bool[3]; //is he looking to a car
        current_time_span = new float[3]; //time span since OnFrame() started 
        is_hit_by_car = new bool[3];
        distance_nearest_car_in_lane = new float[3];

        //getting the initial state of the first road 
        leftSideString = ExperimentParameters.streetsDirections.Split(' ')[0][0].ToString();
        current_traffic_towards_flow = leftSideString;
        //time since touching the first yellow point
        timeSinceReachTheFirstYellowPoint = Time.time;

        parents2DArray = carController.parentsWithCars2DArrayRefernces;

        //invoke the method
        InvokeRepeating("searchOnPlayer", 1f, observeFrameRate);
    }

    /*****temp variables****/
    float angle;
//    int lastAngle = 90;
    //int currentAngle = 90;
    long frameIndex = 0;
    /***********************/
    //int frame = 0;      // initialize with zero
    void OnFrame()
    {
        //recording the data 
        angle = mainCamera.transform.localRotation.eulerAngles.y;
        playerPositions[frameIndex] = SpineMid.transform.position;
        playerHeadRotations[frameIndex] = angle;
        traffic_towards_flow[frameIndex] = current_traffic_towards_flow;
        current_time_span[frameIndex] = Mathf.Abs(Mathf.Round((Time.time - timeSinceReachTheFirstYellowPoint) * 1000) / 1000);
        isLookingAtCar[frameIndex] = CarMove.numberOfRenderedCars > 0;
        is_hit_by_car[frameIndex] = checkPointsController.isHitByCar;
        distance_nearest_car_in_lane[frameIndex] = finalDistanceResult;

        frameIndex++;
        if (frameIndex == 2)   // you can use this as the index of the lists
        {
            //            Debug.Log("ON FRAME ");
            observedData = new ObservedData(playerPositions, playerHeadRotations, isLookingAtCar, traffic_towards_flow, current_time_span, is_hit_by_car, distance_nearest_car_in_lane);
            //connection to database in a thread 
            //  Debug.Log("OnFRAME");
            //StartCoroutine(ConnectToDB());
            Thread connectionDBThread = new Thread(() => ConnectionToDB());

            connectionDBThread.Start();
            if (!connectionDBThread.IsAlive)
            {
                //  Debug.Log("ABORT THREAD");
                connectionDBThread.Abort();
            }

            // Debug.Log("thread status =" + connectionDBThread.IsAlive);

            //connectionDBThread.Join();


            frameIndex = 0;     // back to zero after each send
        }


    }

    private void ConnectionToDB()
    {
        // Debug.Log("ConnectionToDB METHOD HERE!!!");
        _crossing_road_connection.CreateRoadCrossingData(observedData);
    }

    //check the player if the kinect sees him then we're ready to record out movements data 
    void searchOnPlayer()
    {
        try
        {

            if (checkPointsController.isFinishedCrossing)
            {
                SpineMid = null;
                //_crossing_road_connection.CloseConnection();

            }
            if (SpineMid == null)  //kinect failed in seeing the patient 
            {
                if (onFrameWorking)
                {
                    onFrameWorking = false;
                    //  Debug.Log("Cencel Invoke On Frame");
                    CancelInvoke("OnFrame");
                }
                if (onlineBodyView != null)
                {
                    // SpineMid = onlineBodyView;  //reference from checkpoint controller 
                    // SpineMid = GameObject.Find("SpineMid").transform;
                    //  onlineBodyView.transform.GetChild(0).name = "Player";
                }

            }

            else if (!onFrameWorking) //kinect succeed otherwise 
            {
                //Debug.Log("Observe frames = " + observeFrameRate);
                InvokeRepeating("OnFrame", 0.0f, observeFrameRate);
                onFrameWorking = true;
            }

        }
        catch (System.Exception e)
        {
            //nothing here to see
            Debug.Log(e.Message);
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

    public void PassingAndProcessingDistanceValues(GameObject laneHitBox)
    {


        //processing parent and lane and get cars values 
        if (laneHitBox.name == "1" || laneHitBox.name == "2")
        {
            parentNumber = int.Parse(laneHitBox.transform.parent.name.Split(' ')[1]) - 1;
            laneNumber = int.Parse(laneHitBox.name);

            Debug.Log("Parent Number = " + parentNumber);
            Debug.Log("Lane Number = " + laneNumber);

            InvokeRepeating("DistanceNearestCar", 0.0f, observeFrameRate);
        }
        else
        {
            Debug.Log("invoke distance function cenceled");
            CancelInvoke("DistanceNearestCar");
        }
        //invoke repeat distane measure 


    }

    public void DistanceNearestCar(/*some parameters */)
    {
        //determine if the car on right or left side of the road 

        carDirectionWithPlayer();
        //if for making sure which cars are we distancing 

        Debug.Log("Entered Method");

    }

    public void carDirectionWithPlayer()  //needs to check all cases 
    {
        int j = parentNumber;
        List<GameObject> chosenCars = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            if (parents2DArray[j, i].activeInHierarchy)
            {
                if (laneNumber == 1 && i % 2 == 0) //even for the first lane 
                {
                    var relativePoint = SpineMid.transform.InverseTransformPoint(parents2DArray[j, i].transform.position);
                    // Debug.Log("relativePoint = "+ relativePoint);
                    if (relativePoint.z > 0.0 && current_traffic_towards_flow == "L")
                    {

                        print(parents2DArray[j, i].name + " is to the left");
                        chosenCars.Add(parents2DArray[j, i]);
                        CalculateDistances(chosenCars);
                    }

                    else
                    {
                        if (relativePoint.z < 0.0 && current_traffic_towards_flow == "R")
                        {
                            chosenCars.Add(parents2DArray[j, i]);
                            print(parents2DArray[j, i].name + "  is to the right");
                            CalculateDistances(chosenCars);

                        }
                    }

                    // Debug.Log("ACTIVE in HEIR for first lane  = " + parents2DArray[j, i].name);
                }
                else
                {
                    if (laneNumber == 2 && i % 2 != 0)  //Second Lane
                    {
                        var relativePoint = SpineMid.transform.InverseTransformPoint(parents2DArray[j, i].transform.position);
                        // Debug.Log("relativePoint = "+ relativePoint);
                        if (relativePoint.z > 0.0 && current_traffic_towards_flow == "L")
                        {

                            print(parents2DArray[j, i].name + " is to the left");
                            chosenCars.Add(parents2DArray[j, i]);
                            CalculateDistances(chosenCars);
                        }

                        else
                        {
                            if (relativePoint.z < 0.0 && current_traffic_towards_flow == "R")
                            {
                                chosenCars.Add(parents2DArray[j, i]);
                                print(parents2DArray[j, i].name + "  is to the right");
                                CalculateDistances(chosenCars);

                            }
                        }
                        // Debug.Log("ACTIVE in HEIR for second lane = " + parents2DArray[j, i].name);
                    }
                }
            }
        }
        CalculateDistances(chosenCars);
    }

    public void CalculateDistances(List<GameObject> carsChosen)
    {
        List<float> distances = new List<float>();
        for (int i = 0; i < carsChosen.Count; i++)
        {
            if (carsChosen[i] != null)
            {
                distances.Add(Vector3.Distance(SpineMid.transform.position, carsChosen[i].transform.position));
            }
        }
        distances.Sort();
        Debug.Log("Smallest distance in this try is ===== " + distances[0]);
        finalDistanceResult = distances[0];
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


