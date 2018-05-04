﻿using System.Collections;
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

    public static bool fadeout_after_crossing = false ;       // if it's true then fade entire screen | if it's false don't do that  (to nadir)
    string[] streetsDirections;                             // defining the street directions | manage to generate a road and Arrows (to nadir)
    private GameObject yellowArrowsFirstPath = null;        // reference to the yellow arrows that show the walk directions on the first road
    private GameObject yellowArrowsSecondPath = null;       // reference to the yellow arrows that show the walk directions on the second road if exist



    public List<GameObject> carsReferences;

    void Start()
    {
        VRSettings.enabled = true;
        checkPointsController.startTheGameCheckPointReachedEvent += TurnOnAndOfYellowArrowsThenSayGo;
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
        carsReferences = new List<GameObject>();
        //i am using string builder to rename the roads into a correct format just to make it easy reaching them
        float lastPosition = sidewalkWidth + midwalkWidth + (streetPathWidth / 2) + streetPathWidth * (numberOfPathsInSingleRoad / 2);

        yellowArrowsFirstPath = Instantiate(yellowArrows, new Vector3(sidewalkWidth + (streetPathWidth  / 2), -1.999f, -8.98f), Quaternion.identity);
            
   
        //Road #1
        createDirection(sidewalkWidth + (streetPathWidth / 2), ref pathGenerateIndex, 0);
        Debug.Log("after generate the first road");
        Debug.Log(ExperimentParameters.streetsDirections);
        streetsDirections = ExperimentParameters.streetsDirections.Split(' ');
        if (streetsDirections.Length > 1)
        {
            Debug.Log("generate the second road");
            Instantiate(midWalk, new Vector3(sidewalkWidth + (midwalkWidth / 2) + streetPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
            yellowArrowsSecondPath = Instantiate(yellowArrows, new Vector3(lastPosition, -1.99f, -8.98f), Quaternion.identity);
           
            //Road #2
            createDirection(sidewalkWidth + (streetPathWidth / 2) + midwalkWidth + (streetPathWidth * (numberOfPathsInSingleRoad / 2)), ref pathGenerateIndex, 2);
        Instantiate(sidewalk, new Vector3((sidewalkWidth) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), -0.0012f, 0.0f), Quaternion.identity);
        }
        else
        Instantiate(sidewalk, new Vector3((sidewalkWidth)  + streetPathWidth * (numberOfPathsInSingleRoad/2), -0.0012f, 0.0f), Quaternion.identity);
        
        Debug.Log("generate side walk");

        BuildingsWrapper.transform.position = new Vector3((sidewalkWidth * 2) + streetPathWidth * (numberOfPathsInSingleRoad/2), 0, 0);
    }

    public void createDirection(float startPositionAtX, ref int pathGenerateIndex, int indexOfDirection)
    {
        int numberOfPathsInSingleRoad = ExperimentParameters.lanes_per_direction;
        
        streetsDirections = ExperimentParameters.streetsDirections.Split(' '); //to be able to name the streets
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Vector3 RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad = Instantiate(streetPath, RoadMeasure, Quaternion.identity) as GameObject;

            foreach (Transform child in generatedRoad.transform)
            {
                child.gameObject.name = pathGenerateIndex.ToString();
                pathGenerateIndex++;
            }
            //i'll take each road generated (the cars are from left to right movement) and rename it into a specific name
            //i used string builder for the performance issues
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Road " + i);
           // stringBuilder.Append(streetsDirections[indexOfDirection] + " ");
           // stringBuilder.Append(i + 1);
            generatedRoad.name = stringBuilder.ToString();
        }
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
        audioController.playAudioClip("DRSounds/Go");
    }
}