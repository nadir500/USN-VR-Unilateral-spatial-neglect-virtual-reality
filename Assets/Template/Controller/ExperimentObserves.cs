using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour
{

    public CheckPointsController checkPointsController;

    private List<Vector3> playerPositions;      //taken from spineMid GameObject position
    private List<float> playerHeadRotations;    // taken from the camira
    public List<bool> isLookingAtCar;           // taken from CarMove class
    public List<string> traffic_towards_flow;   // 
    public List<float> current_time_span;
    public List<bool> is_hit_by_car;
    public string current_traffic_towards_flow;

    public GameObject mainCamera;               // reference to the camira in the hierarachy
    public GameObject onlineBodyView;           // reference to onlineBodyVew (just used to find the spineMid at his grandsons
    private Transform SpineMid = null;          // reference to spineMid (kinekt creates it at runtime)
    private bool onFrameWorking;        // true => the 30 frame per second method is working after finding the spineMid | false  => the 30 frame per second method is NOT working


    /***************this sould be removed by nadir prevez****************/
    public List<GameObject> carsReferences;
    string[] roadtype;
    DataService _crossing_road_connection;
    ObservedData observedData;
    public float startTimeofReachingTheFirstBall;
    //public GameObject car;
    /********************************************************************/
    // Use this for initialization
    void Start()
    {

        onFrameWorking = false;
        playerPositions = new List<Vector3>();
        playerHeadRotations = new List<float>();
        traffic_towards_flow = new List<string>();
        isLookingAtCar = new List<bool>();
        current_time_span = new List<float>();
        is_hit_by_car = new List<bool>();
        _crossing_road_connection = new DataService("USN_Simulation.db");
        checkPointsController.startTheGameCheckPointReachedEvent += Initilize;
        checkPointsController.backToMidWalkCheckPointReachedEvent += OnChangeTrafficTowardsFlow;


    }
    public void Initilize()
    {
        //current_traffic_towards_flow = ExperementParameters.streetsDirections.Split(' ')[0];
        string leftSideString = ExperementParameters.streetsDirections.Split(' ')[0][0].ToString();
        current_traffic_towards_flow = leftSideString;

        startTimeofReachingTheFirstBall = Time.time;
        Debug.Log("StartFire EVENT TIME  " + startTimeofReachingTheFirstBall );
        InvokeRepeating("searchOnPlayer", 1f, 0.0333f);
    }

    /*****temp varialbes****/
    float angle;
    int lastAngla = 90;
    int currentAngle = 90;
    long frameIndex = 0;
    /***********************/
    int frame = 0;
    void onFrame()
    {
        angle = mainCamera.transform.localRotation.eulerAngles.y;
        playerPositions.Add(SpineMid.position);
        playerHeadRotations.Add(angle);
        traffic_towards_flow.Add(current_traffic_towards_flow);
        current_time_span.Add((Mathf.Round((Time.time - startTimeofReachingTheFirstBall) *1000)));

        isLookingAtCar.Add(CarMove.numberOfRenderdCars > 0);
        is_hit_by_car.Add(checkPointsController.isHitByCar);

        frameIndex++;
        if (frameIndex == 20)
        {
            Debug.Log("600 frame reached");
            observedData = new ObservedData(playerPositions, playerHeadRotations, isLookingAtCar, traffic_towards_flow, current_time_span, is_hit_by_car);
            // send observedData to database here
            // _crossing_road_connection.CreateRoadCrossingData(observedData/*traffic_towards_flow, Mathf.RoundToInt(Time.time * 1000),
            //   0,isLookingAtCar,false, checkPointsController.isHitByCar,playerPositions,playerHeadRotations*/);
            /* Debug.Log(" playerPositions count:  " + playerPositions.Count);
             Debug.Log(" playerHeadRotations count:  " + playerHeadRotations.Count);
             Debug.Log(" traffic_towards_flow count:  " + traffic_towards_flow.Count);
             Debug.Log(" isLookingAtCar count:  " + isLookingAtCar.Count);
             playerPositions = new List<Vector3>();
             playerHeadRotations = new List<float>();
             traffic_towards_flow = new List<string>();
             isLookingAtCar = new List<bool>();
             current_time_span = new List<int>();
             is_hit_by_car = new List<bool>();*/
            frameIndex = 0;
        }
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

            }
            else if (!onFrameWorking)
            {
                InvokeRepeating("onFrame", 1f, 0.0333f);
                onFrameWorking = true;
            }
        }
        catch (System.Exception e)
        {
            //MyConsol.log(e.Message.ToString());
        }
    }


    public float CheckDistanceBetweenPlayerAndNearestCar()
    {
        Debug.Log("player on path current path variable " + PlayerOnPlath.currentPath + "and index " + carsReferences.Count);
        if ((PlayerOnPlath.currentPath != -1)
            && (carsReferences[PlayerOnPlath.currentPath] != null))
        {

            roadtype = carsReferences[PlayerOnPlath.currentPath].transform.parent.gameObject.name.Split(' ');
            Debug.Log("ROAD TYPE " + roadtype[1]);
            Debug.Log("CARRRRRRRR " + carsReferences[PlayerOnPlath.currentPath].name);
            if ((carsReferences[PlayerOnPlath.currentPath].transform.position.z < SpineMid.transform.position.z) && (roadtype[1].Equals(value: "Left"))
            || ((carsReferences[PlayerOnPlath.currentPath].transform.position.z > SpineMid.transform.position.z) && (roadtype[1].Equals(value: "Right"))))

                return Vector3.Distance(SpineMid.transform.position, carsReferences[PlayerOnPlath.currentPath].transform.position);
            else
                return -1.0f;
        }
        else
            return -1.0f;

    }

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


}


