using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TableController : MonoBehaviour
{

    public CheckPointsController checkPointsController;

    public GameObject tableWrapper;
    public GameObject leapMotionCamera;
    public GameObject cameraPositionXZ;

    private Object[] tablePrefabs;
    private GameObject[] instantiatedTableActiveGameObjects;
    private int[] shuffeledNumbers;
    private static int shuffeledNumbersIndex = 0;
    private static int numberOfLable = 1;
    private Transform points;
    private GameObject[] pointsArr;
    // Use this for initialization


    void Start()
    {
        DataService dbgrabconnection = new DataService("USN_Simulation.db");
        Collected_Objects collected_Objects = new Collected_Objects(1,1,1,"e","2",true,true,"222");
        dbgrabconnection.CreateCollectedObjects(collected_Objects);
        instantiatedTableActiveGameObjects = new GameObject[6];


        if (checkPointsController != null)
            checkPointsController.otherSideCheckPointReachedEvent += Initilize;
        else
            Initilize();
    }

    public void tableObjectSelectedByCalculator(string id, string side)
    {
        for(int i = 0; i < instantiatedTableActiveGameObjects.Length; i++)
        {
            if(id.Equals(instantiatedTableActiveGameObjects[i]))
            {
                Debug.Log(instantiatedTableActiveGameObjects[i].gameObject.name);
            }
        }
    }
    void Initilize()
    {
        tablePrefabs = Resources.LoadAll("Prefabs/TableObjects");
        tableWrapper.SetActive(true);
        points = tableWrapper.transform.Find("Points");

        int[] indeces = new int[tablePrefabs.Length];
        for (int i = 0; i < indeces.Length; indeces[i] = i++) ;
        System.Random rnd = new System.Random();
        shuffeledNumbers = Enumerable.Range(0, indeces.Length - 1).OrderBy(r => rnd.Next()).ToArray();
        generateFirstLevel();
        InvokeRepeating("LeapCameraIntialize",1, 1);


    }
    void LeapCameraIntialize()
    {
        Debug.Log("LeapCameraIntialize");
        leapMotionCamera = GameObject.Find("GearVRCameraRigTEST(Clone)") as GameObject;
        if(leapMotionCamera != null)
        {
            leapMotionCamera.transform.position = new Vector3(cameraPositionXZ.transform.position.x, ExperementParameters.lengthOfPatient / 100 - 1.5f, cameraPositionXZ.transform.position.z);      
            CancelInvoke("LeapCameraIntialize");
            Debug.Log("found");
        }
        else
        {
            Debug.Log("not found");
        }
    }
    void generateTableObject(int level, string dist, string direc, int group, bool active)
    {
        string name = "Lvl" + level + "_" + direc + "_Group" + group + "_" + dist;
        //Debug.Log(name);
        Transform parent = points.transform.Find(name);
        int childIndex = Random.Range(0, parent.childCount);
        GameObject newTableObject = Instantiate(tablePrefabs[shuffeledNumbers[shuffeledNumbersIndex]], parent.GetChild(childIndex).position, Quaternion.identity) as GameObject;
        if (!active)
            newTableObject.transform.Find("Canvas").gameObject.SetActive(false);
        else
        {
            Debug.Log("numberOfLable-1 = " + (numberOfLable - 1).ToString());
            instantiatedTableActiveGameObjects[numberOfLable-1] = newTableObject;

            newTableObject.AddComponent<TableObject>();
            newTableObject.GetComponent<TableObject>().setValues(shuffeledNumbersIndex++.ToString(), level.ToString(), direc);
        }


    }
    void generateFirstLevel()
    {
        int rightSideFirstLevelGroup = (int)UnityEngine.Mathf.Round(UnityEngine.Random.Range(0, 1));//Random.Range(0, 1);
        int leftSideFirstLevelGroup = 1 - rightSideFirstLevelGroup;
        generateTableObject(1, "Far", "Left", leftSideFirstLevelGroup, true);
        generateTableObject(1, "Far", "Right", rightSideFirstLevelGroup, true);

        generateNextLevels(2, leftSideFirstLevelGroup, (((int)UnityEngine.Mathf.Round(Random.value) - 0.1) > 0) ? "Far" : "Near", "Left");
        generateNextLevels(2, rightSideFirstLevelGroup, (((int)UnityEngine.Mathf.Round(Random.value) + 0.1) > 0) ? "Far" : "Near", "Right");

    }

    void generateNextLevels(int level, int lastActivePointGroup, string LastActivePointDistance, string direct)
    {

        int activepointGroup = (int)UnityEngine.Mathf.Round(Random.value);
        int dummyPointGroup = 1 - activepointGroup;
        string activepointdistance;
        string dummyPointDistance = (((int)UnityEngine.Mathf.Round(Random.value)) > 0) ? "Far" : "Near";

        if (activepointGroup == lastActivePointGroup)
            activepointdistance = "Far";
        else
            activepointdistance = (((int)UnityEngine.Mathf.Round(Random.value)) > 0) ? "Far" : "Near";
        generateTableObject(level, activepointdistance, direct, activepointGroup, true);
        generateTableObject(level, dummyPointDistance, direct, dummyPointGroup, false);

        if (level == 3)
            return;
        generateNextLevels(level + 1, activepointGroup, activepointdistance, direct);
    }

    //Debug.Log(this.transform.FindChild("Lvl" + level + "_" + direc + "_Group" + group + "_" + dist).name);



}
