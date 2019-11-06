using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.VR;

public class RoadController : MonoBehaviour
{

    //aims to generate a road when i press the start/test Game button
    public GameObject sidewalk;             // reference to the prefab of side walk         |   assigned in the inspector to the resources/prafabs/Sidewalk gameObject
    public GameObject streetPath;           // reference to the prefab of the street path   |   assigned in the inspector to the resources/prafabs/Path gameObject
    public GameObject midWalk;              // reference to the prefab of the mid-walk      |   assigned in the inspector to the resources/prafabs/MidWalk gameObject
    public GameObject BuildingsWrapper;     // reference to the gameobject of BuildingWrapper in the hierarchy
    public CheckPointsController checkPointsController;
    public AudioController audioController;


    public GameObject yellowArrows;         // reference to the prefab of yellowArrows point of   |   assigned in the inspector to the resources/prafabs/yellowArrows gameObject
    //public CarController carController;


    public const float streetPathWidth = 5;        //  the width of pair of paths
    public const float sidewalkWidth = 5f;         //  the width of sidewalk
    public const float midwalkWidth = 1.36f;       //  the width of midwalk

    public static bool fadeout_after_crossing = false;       // if it's true then fade entire screen | if it's false don't do that  (to nadir)
    public GameObject[] roadsArray;
    string[] streetsDirections;                             // defining the street directions | manage to generate a road and Arrows (to nadir)
    private GameObject yellowArrowsFirstPath = null;        // reference to the yellow arrows that show the walk directions on the first road
    private GameObject yellowArrowsSecondPath = null;       // reference to the yellow arrows that show the walk directions on the second road if exist

    private int offsetOnXValue = 0;


    public List<GameObject> carsReferences;

    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = true;
        checkPointsController.startTheGameCheckPointReachedEvent += TurnOnAndOfYellowArrowsThenSayGo;

         Debug.Log("Date "+ System.DateTime.Now.Date);
    }

    /*
        Parameters:
        Returns: void
        Objective:
            generate the streets and the mid-walk/side-walk and set the buildings in the other side ot the street
            generate the yellow direction arrows
            if street direction from one word (left write) then it will generate the streets in one direction
            else if the street direction from more than one word then it will generate two roads

     */
    public void generateRoads()
    {
        //Assigning number of paths from the UI
        int pathGenerateIndex = 0;
        int numberOfPathsInSingleRoad = ExperimentParameters.lanes_per_direction;
//        int numberOfRoads_temp = numberOfPathsInSingleRoad / 2;
        int numberOfRoads = ExperimentParameters.numberOfRoads;

        Debug.Log("Number of Roads = " + numberOfRoads);

        carsReferences = new List<GameObject>();
        //i am using string builder to rename the roads into a correct format just to make it easy reaching them
        float lastPosition = sidewalkWidth + midwalkWidth + (streetPathWidth / 2) + streetPathWidth * (numberOfRoads);

        yellowArrowsFirstPath = Instantiate(yellowArrows, new Vector3(sidewalkWidth + (streetPathWidth / 2), -1.999f, -8.98f), Quaternion.identity);


        //Road #1 
        roadsArray = createDirection(sidewalkWidth + (streetPathWidth / 2), ref pathGenerateIndex, 0);
        Debug.Log("after generate the first road");
        Debug.Log(ExperimentParameters.streetsDirections);
        streetsDirections = ExperimentParameters.streetsDirections.Split(' ');
        if (streetsDirections.Length > 1)  //I think we can move this code to createDirection Method section
        {
            Debug.Log("generate the second road");

            Instantiate(midWalk, new Vector3(sidewalkWidth + (midwalkWidth / 2) + streetPathWidth * (numberOfRoads), -2.0f, 0.0f), Quaternion.identity);
            yellowArrowsSecondPath = Instantiate(yellowArrows, new Vector3(lastPosition, -1.99f, -8.98f), Quaternion.identity);

            //Road #2  TODO: needs to fix later after the edit 
            roadsArray = createDirection(sidewalkWidth + (streetPathWidth / 2) + midwalkWidth + (streetPathWidth * (numberOfRoads)), ref pathGenerateIndex, 2);
            Instantiate(sidewalk, new Vector3((sidewalkWidth) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), -0.0012f, 0.0f), Quaternion.identity);
        }
        else //one direction
        {
        Debug.Log("generate side walk in else condition");
            Instantiate(sidewalk, new Vector3((sidewalkWidth) + (streetPathWidth * (offsetOnXValue / 2)) + (midwalkWidth * numberOfRoads) - midwalkWidth, -0.0012f, 0.0f), Quaternion.identity);
        }

        BuildingsWrapper.transform.position = new Vector3((sidewalkWidth * 2) + (midwalkWidth * numberOfRoads) + (streetPathWidth * (offsetOnXValue / 2)), 0, 0);

    }

    public GameObject[] createDirection(float startPositionAtX, ref int pathGenerateIndex, int indexOfDirection)
    {
        //int numberOfPathsInSingleRoad = ExperimentParameters.lanes_per_direction;
        int numberOfRoads = ExperimentParameters.numberOfRoads;
        int numberOfLanes = ExperimentParameters.lanes_per_direction;
        Vector3 RoadMeasure = Vector3.zero;
        GameObject generatedRoad = null;
        GameObject[] roadsArray = new GameObject[numberOfRoads * numberOfLanes / 2];
        Debug.Log("Roads array length " + roadsArray.Length);
        streetsDirections = ExperimentParameters.streetsDirections.Split(' '); //to be able to name the streets
        for (int i = 0; i < numberOfRoads; i++)
        {
            // Vector3 RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * i), -2.0f, 0.0f);
            //           Vector3 RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * i) + (midwalkWidth * i) , -2.0f, 0.0f);

            //TODO: REFACTOR THIS CODE PLZZZZZZ
            //TODO: different roads special case didn't solve and needs to be solved.
            if (i != 0)
            {
                Debug.Log("offsetOnXValue  = " + (offsetOnXValue / 2) + " AND i = " + i);
                Instantiate(midWalk, new Vector3(sidewalkWidth + (midwalkWidth / 2) + (streetPathWidth * (offsetOnXValue / 2)) + (midwalkWidth * (i - 1))    /* (numberOfRoads)*/, -2.0f, 0.0f), Quaternion.identity);

            }
            for (int j = 0; j < (numberOfLanes / 2); j++) //generate each  road 
            {
                if (j == 0)
                {
                    RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * offsetOnXValue / 2) + (midwalkWidth * i), -2.0f, 0.0f);
                    generatedRoad = Instantiate(streetPath, RoadMeasure, Quaternion.identity) as GameObject;
                    offsetOnXValue += 2;

                }
                else
                {

                    RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * offsetOnXValue / 2) + (midwalkWidth * i), -2.0f, 0.0f);
                    generatedRoad = Instantiate(streetPath, RoadMeasure, Quaternion.identity) as GameObject;
                    offsetOnXValue += 2;
                }
                int offsetTotal = (offsetOnXValue / 2) - 1;
                Debug.Log(offsetTotal+ "<--(offsetOnXValue / 2) - 1 " + "roadsArray.Length --> " + roadsArray.Length);
                if (offsetTotal < roadsArray.Length)
                {
                    Debug.Log("Entered Condition");
                    roadsArray[offsetTotal] = generatedRoad;
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Road " + offsetOnXValue / 2);
                // stringBuilder.Append(streetsDirections[indexOfDirection] + " ");
                // stringBuilder.Append(i + 1);
                generatedRoad.name = stringBuilder.ToString();

            }
            // foreach (Transform child in generatedRoad.transform)  //<<---commented for nearest distance Edit--->>
            //   {
            // //      child.gameObject.name = pathGenerateIndex.ToString();
            //     pathGenerateIndex++;
            //  }


            //i'll take each road generated (the cars are from left to right movement) and rename it into a specific name
            //i used string builder for the performance issues
            //  StringBuilder stringBuilder = new StringBuilder();
            //   stringBuilder.Append("Road " + i);
            // stringBuilder.Append(streetsDirections[indexOfDirection] + " ");
            // stringBuilder.Append(i + 1);
            //  generatedRoad.name = stringBuilder.ToString();

        }
        return roadsArray;
    }


    void TurnOnAndOfYellowArrowsThenSayGo()
    {
        StartCoroutine(TurnOnAndOfYellowArrowsThenSayGoWithTimePeriods());
    }
    //behavior to make the arrows blink in the start of the game 
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
        // audioController.playAudioClip("DRSounds/Go");
    }
}