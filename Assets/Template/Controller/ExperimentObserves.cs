using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour {

    CheckPointsController checkPointsController;
    List<ObservedData> observedDataList;
    bool isLookingAtCar;

    public GameObject mainCamera;               // reference to the camira in the hierarachy
    public GameObject onlineBodyView;           // reference to onlineBodyVew (just used to find the spineMid at his grandsons
    private Transform SpineMid = null;          // reference to spineMid (kinekt creates it at runtime)
    private bool onFrameWorking;        // true => the 30 frame per second method is working after finding the spineMid | false  => the 30 frame per second method is NOT working


    /***************this sould be removed by nadir prevez****************/
    public List<GameObject> carsReferences;
    string[] roadtype;
    //public GameObject car;
    /********************************************************************/
    // Use this for initialization
    void Start()
    {
        observedDataList = new List<ObservedData>();
        checkPointsController.startTheGameCheckPointReachedEvent += Initilize;
    }
    public void Initilize () {
        onFrameWorking = false;
        
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
        //currentAngle = (int)angle;
        //if (currentAngle != lastAngla)
        //{
        //    //MyConsol.log(currentAngle.ToString());
        //    lastAngla = currentAngle;
        //}
        isLookingAtCar = (CarController.numberOfRenderdCars > 0) ? true : false;
        observedDataList.Add(new ObservedData(SpineMid.position, angle, isLookingAtCar));
      
       // CheckDistanceBetweenPlayerAndNearestCar();

        Debug.Log("CheckDistanceBetweenPlayerAndNearestCar " + CheckDistanceBetweenPlayerAndNearestCar());

        isLookingAtCar = false;
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
                onFrameWorking = true;
            }

        }
        catch (System.Exception e)
        {
            //MyConsol.log(e.Message.ToString());
        }
    }


    public float  CheckDistanceBetweenPlayerAndNearestCar()
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
        private Vector3 playerPositions;      // took from spineMid GameObject
        private float playerHeadRotations;  // took from the camira
        bool isLookingAtCar;          // took from each car

        public ObservedData(Vector3 playerPositions, float playerHeadRotations, bool isLookingAtCar)
        {
            this.playerPositions = playerPositions;
            this.playerHeadRotations = playerHeadRotations;
            this.isLookingAtCar = isLookingAtCar;
        }

    }
}
