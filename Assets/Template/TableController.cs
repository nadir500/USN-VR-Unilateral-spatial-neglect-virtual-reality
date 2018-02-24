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
    private GameObject[] instantiatedTableActiveGameObjects;        // array of active instanitated object 0 - 5 (size = 6)
    private int[] shuffeledNumbers;        // array of shuffeled number from 0-9 (to randomly select instantiated prafabs)
    private int [] shuffeledIds;        // array of shuffeled number from 0-5 (to randomly select instantiated prafabs)
    private static int shuffeledNumbersIndex = 0;
    private static int numberOfLable = 1;
    private Transform points;
    private GameObject[] pointsArr;
    private DataService dbgrabconnection;
    // Use this for initialization


    void Start()
    {
         dbgrabconnection = new DataService("USN_Simulation.db");
        instantiatedTableActiveGameObjects = new GameObject[6];
        shuffeledIds = new int[6];


        if (checkPointsController != null)
            checkPointsController.otherSideCheckPointReachedEvent += Initilize;
        else
            Initilize();
    }

    public void tableObjectSelectedByCalculator(string id, string side)
    {
        Debug.Log("id = " + id);
        for(int i = 0; i < instantiatedTableActiveGameObjects.Length; i++)
        {
             TableObject activeTableGameObject = instantiatedTableActiveGameObjects[i].GetComponent<TableObject>();
            
            Debug.Log(i +" " + instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().id);
            if(id.Equals(activeTableGameObject.id))
            {
                Debug.Log(instantiatedTableActiveGameObjects[i].gameObject.name);

                if(side.Equals(activeTableGameObject.objectPosition))
                {
                activeTableGameObject.obj_recorded_on_pad = true;
                dbgrabconnection.UpdateCollectedObjectOnPad(int.Parse(activeTableGameObject.id),activeTableGameObject.obj_recorded_on_pad);
                activeTableGameObject.canvas.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Textures/UiSprites/golden_star") as Sprite;
                activeTableGameObject.canvas.GetChild(0).GetChild(0).GetComponent<Text>().enabled = false;
                break;

                }
            }
        }
    }
    void Initilize()
    {
        tablePrefabs = Resources.LoadAll("Prefabs/TableObjects");
        tableWrapper.SetActive(true);
        points = tableWrapper.transform.Find("Points");

        shuffeledNumbers = new int[tablePrefabs.Length];
        //for (int i = 0; i < indeces.Length; indeces[i] = i++) ;
        System.Random rnd = new System.Random();
        shuffeledNumbers = Enumerable.Range(0, shuffeledNumbers.Length - 1).OrderBy(r => rnd.Next()).ToArray();

        shuffeledIds = new int[8];
        //for (int i = 1; i < ids.Length +1; ids[i-1] = i++) ;
        shuffeledIds = Enumerable.Range(1, shuffeledIds.Length).OrderBy(r => rnd.Next()).ToArray();


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
    void generateTableObject(int level, string dist, string objectPosition, int group, bool active)
    {
        string name = "Lvl" + level + "_" + objectPosition + "_Group" + group + "_" + dist;
        //Debug.Log(name);
        Transform parent = points.transform.Find(name);
        int childIndex = Random.Range(0, parent.childCount);
        GameObject newTableObject = Instantiate(tablePrefabs[shuffeledNumbers[shuffeledNumbersIndex++]], parent.GetChild(childIndex).position, Quaternion.identity) as GameObject;
        if (!active)
            newTableObject.transform.Find("Canvas").gameObject.SetActive(false);
        else
        {
            Debug.Log("numberOfLable-1 = " + (numberOfLable - 1).ToString());
            instantiatedTableActiveGameObjects[numberOfLable -1] = newTableObject;
            newTableObject.AddComponent<TableObject>();
            newTableObject.GetComponent<TableObject>().setValues((shuffeledIds[numberOfLable++]).ToString(), level.ToString(), objectPosition);
           
            TableObject tempTableObject = newTableObject.GetComponent<TableObject>();

            Collected_Objects tempCollectedObject = new Collected_Objects();
            tempCollectedObject.SetValues(ExperementParameters.gameplay_id,int.Parse(tempTableObject.id),tempTableObject.objectPosition,tempTableObject.level,false,false,"");
            dbgrabconnection.CreateCollectedObjectsRow(tempCollectedObject);
        }


    }
    void generateFirstLevel()
    {
        int rightSideFirstLevelGroup = (int)UnityEngine.Mathf.Round(UnityEngine.Random.Range(0, 1));//Random.Range(0, 1);
        int leftSideFirstLevelGroup = 1 - rightSideFirstLevelGroup;
        generateTableObject(1, "Far", "left", leftSideFirstLevelGroup, true);
        generateTableObject(1, "Far", "right", rightSideFirstLevelGroup, true);

        generateNextLevels(2, leftSideFirstLevelGroup, (((int)UnityEngine.Mathf.Round(Random.value) - 0.1) > 0) ? "Far" : "Near", "left");
        generateNextLevels(2, rightSideFirstLevelGroup, (((int)UnityEngine.Mathf.Round(Random.value) + 0.1) > 0) ? "Far" : "Near", "right");

    }

    void generateNextLevels(int level, int lastActivePointGroup, string LastActivePointDistance, string objectPosition)
    {

        int activepointGroup = (int)UnityEngine.Mathf.Round(Random.value);
        int dummyPointGroup = 1 - activepointGroup;
        string activepointdistance;
        string dummyPointDistance = (((int)UnityEngine.Mathf.Round(Random.value)) > 0) ? "Far" : "Near";

        if (activepointGroup == lastActivePointGroup)
            activepointdistance = "Far";
        else
            activepointdistance = (((int)UnityEngine.Mathf.Round(Random.value)) > 0) ? "Far" : "Near";
        generateTableObject(level, activepointdistance, objectPosition, activepointGroup, true);
        generateTableObject(level, dummyPointDistance, objectPosition, dummyPointGroup, false);

        if (level == 3)
            return;
        generateNextLevels(level + 1, activepointGroup, activepointdistance, objectPosition);
    }




}
