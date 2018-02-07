using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RoadController : MonoBehaviour
{


    public GameObject sidewalk;             // reference to the prafab of side walk         |   assigned in the inspector to the resources/prafabs/Sidewalk gameObject
    public GameObject streatPath;           // reference to the prefab of the street path   |   assigned in the inspector to the resources/prafabs/Path gameObject
    public GameObject midWalk;              // reference to the prefab of the mid-walk      |   assigned in the inspector to the resources/prafabs/MidWalk gameObject
    public GameObject BuildingsWrapper;     // reference to the gameobject of BuildingWrapper in the hierarchy
    public CheckPointsController checkPointsController;
    public AudioController audioController;


    public GameObject yellowArrows;         // reference to the prafab of yellowArrows point of   |   assigned in the inspector to the resources/prafabs/yellowArrows gameObject
    //public CarController carController;


    public const float streetPathWidth = 5;        //  the width of pair of paths
    public const float sidewalkWidth = 5f;         //  the width of sidewalk
    public const float midwalkWidth = 1.36f;       //  the width of midwalk

    public static bool fadeout_after_crossing;       // if it's true then fade entire screen | if it's false don't do that  (to nadir)
    string[] streetsDirections;                             // defining the street directions | manage to generate a road and Arrows (to nadir)
    private GameObject yellowArrowsFirstPath = null;
    private GameObject yellowArrowsSecondPath = null;

    AudioSource audioSource;


    public List<GameObject> carsReferences;

    void Start()
    {
        checkPointsController.startTheGameCheckPointReachedEvent += TurnOnAndOfYellowArrowsThenSayGo;
    }
    public void generateRoads()
    {
        //Assigning number of paths from the UI
        int pathGenerateIndex = 0;
        int numberOfPathsInSingleRoad = ExperementParameters.lanes_per_direction;
        carsReferences = new List<GameObject>();
        //i am using string builder to rename the roads into a correct format just to make it easy reaching them
        float lastPosition = sidewalkWidth + midwalkWidth + (streetPathWidth / 2) + streetPathWidth * (numberOfPathsInSingleRoad / 2);

        yellowArrowsFirstPath = Instantiate(yellowArrows, new Vector3(sidewalkWidth + (streetPathWidth  / 2), -1.99f, -8.98f), Quaternion.identity);
            //if (ExperementParameters.streetsDirections.Split()[0].Equals("Right"))
            //yellowArrowsFirstPath.transform.localScale = new Vector3(1, 1, -1);
            //yellowArrowsFirstPath.transform.localEulerAngles = new Vector3(0, -90, 0);
   
        //Road #1
        createDirection(sidewalkWidth + (streetPathWidth / 2), ref pathGenerateIndex, 0);

        if (ExperementParameters.streetsDirections.Length > 1)
        {
            Instantiate(midWalk, new Vector3(sidewalkWidth + (midwalkWidth / 2) + streetPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
            yellowArrowsSecondPath = Instantiate(yellowArrows, new Vector3(lastPosition, -1.99f, -8.98f), Quaternion.identity);
            if (ExperementParameters.streetsDirections.Equals("Left To Right"))
                //yellowArrowsSecondPath.transform.localScale = new Vector3(1, 1, -1);
                yellowArrowsFirstPath.transform.localEulerAngles = new Vector3(0, -90, 0);
            //Road #2
            createDirection(sidewalkWidth + (streetPathWidth / 2) + midwalkWidth + (streetPathWidth * (numberOfPathsInSingleRoad / 2)), ref pathGenerateIndex, 2);
        }
        Instantiate(sidewalk, new Vector3((sidewalkWidth) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), -0.0012f, 0.0f), Quaternion.identity);

        BuildingsWrapper.transform.position = new Vector3((sidewalkWidth * 2) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), 0, 0);
    }

    public void createDirection(float startPositionAtX, ref int pathGenerateIndex, int indexOfDirection)
    {
        int numberOfPathsInSingleRoad = ExperementParameters.lanes_per_direction;
        
        streetsDirections = ExperementParameters.streetsDirections.Split(' '); //to be able to name the streets
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Vector3 RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad = Instantiate(streatPath, RoadMeasure, Quaternion.identity) as GameObject;

            foreach (Transform child in generatedRoad.transform)
            {
                child.gameObject.name = pathGenerateIndex.ToString();
                pathGenerateIndex++;
            }

            //i'll take each road generated (the cars are from left to right movement) and rename it into a specific name
            //i used string builder for the performance issues
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Road ");
            stringBuilder.Append(streetsDirections[indexOfDirection] + " ");
            stringBuilder.Append(i + 1);
            generatedRoad.name = stringBuilder.ToString();
        }
    }


    void TurnOnAndOfYellowArrowsThenSayGo()
    {
        StartCoroutine(TurnOnAndOfYellowArrowsThenSayGoWithTimePeriods());
    }
    IEnumerator TurnOnAndOfYellowArrowsThenSayGoWithTimePeriods()
    {
        for (int i = 0; i < 3; i++)
        {
            yellowArrowsFirstPath.SetActive(true);
            if (yellowArrowsSecondPath != null)
                yellowArrowsSecondPath.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            yellowArrowsFirstPath.SetActive(false);
            if (yellowArrowsSecondPath != null)
                yellowArrowsSecondPath.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        audioController.playAudioClip("Go");
    }
}