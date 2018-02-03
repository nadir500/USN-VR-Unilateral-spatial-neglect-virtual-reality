using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour
{

    public CheckPointsController checkPointsController;

    private List<Vector3> playerPositions;      // took from spineMid GameObject
    private List<Vector2> playerHeadRotations;  // took from the camira
    private List<bool> isLookingAtCar;          // took from each car

    public GameObject mainCamera;               // reference to the camira in the hierarachy
    public GameObject onlineBodyView;           // reference to onlineBodyVew (just used to find the spineMid at his grandsons
    private Transform SpineMid = null;          // reference to spineMid (kinekt creates it at runtime)
    private bool onFrameWorking;        // true => the 30 frame per second method is working after finding the spineMid | false  => the 30 frame per second method is NOT working


    /***************this sould be removed by nadir prevez****************/
    public List<GameObject> carsReferences;
    string[] roadtype;

    DataService _crossing_road_connection;
    ObservedData observedData;
    //public GameObject car;
    /********************************************************************/
    // Use this for initialization
    void Start()
    {
        observedData = new ObservedData();
        _crossing_road_connection = new DataService("USN_Simulation.db");
        checkPointsController.startTheGameCheckPointReachedEvent += Initilize;
        checkPointsController.backToMidWalkCheckPointReachedEvent += observedData.OnChangeTrafficTowardsFlow;


    }
    public void Initilize()
    {
        observedData.traffic_towards_flow = ExperementParameters.streetsDirections.Split(' ')[0];
        onFrameWorking = false;
        playerPositions = new List<Vector3>();
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
       float distanceCar= CheckDistanceBetweenPlayerAndNearestCar();
        _crossing_road_connection.CreateRoadCrossingData(observedData.traffic_towards_flow, Mathf.RoundToInt(Time.deltaTime * 1000),distanceCar,observedData.isLookingAtCar,false, checkPointsController.isHitByCar);

        //currentAngle = (int)angle;
        //if (currentAngle != lastAngla)
        //{
        //    //MyConsol.log(currentAngle.ToString());
        //    lastAngla = currentAngle;
        //}

        playerPositions.Add(SpineMid.position);

        // CheckDistanceBetweenPlayerAndNearestCar();

        Debug.Log("CheckDistanceBetweenPlayerAndNearestCar " + CheckDistanceBetweenPlayerAndNearestCar());

        //data dabse connection
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
                //else
                //    Debug.Log("no children");

                //if(SpineMid != null)
                //    Debug.Log(" on framework will start in next iteration");
                //else
                //    Debug.Log(" on SpineMid NOT found");
            }
            else if (!onFrameWorking)
            {
                InvokeRepeating("onFrame", 1f, 0.0333f);
                //onFrameWorking = true;
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


    class ObservedData
    {
       public List<Vector3> position;
       public float angle;
      public  bool isLookingAtCar;
       public string traffic_towards_flow;
        public ObservedData()
        {
            this.position = new List<Vector3>();
            this.angle=0;
            this.isLookingAtCar=false;
            this.traffic_towards_flow="";
        }
        public ObservedData(List<Vector3> position, float angle, bool isLookingAtCar, string traffic_towards_flow)
        {
            this.position = position;
            this.angle = angle;
            this.isLookingAtCar = isLookingAtCar;
            //this.traffic_towards_flow = traffic_towards_flow;
        }
        public void OnChangeTrafficTowardsFlow()
        {
            if (this.traffic_towards_flow.Equals(value: "Left"))
            {
                this.traffic_towards_flow = "Right";
            }
            else
            {
                this.traffic_towards_flow = "Left";
            }
        }

    }
}
