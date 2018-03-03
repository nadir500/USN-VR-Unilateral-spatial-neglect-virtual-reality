using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TableController : MonoBehaviour
{

    public CheckPointsController checkPointsController;
    public GameObject[] instantiatedTableActiveGameObjects;        // array of active instanitated object 0 - 5 (size = 6)

    public GameObject tableWrapper;
    public GameObject leapMotionCamera;
    public GameObject cameraPositionXZ;
    public AudioController audioController;
    public GameObject doneCollectingObjectsButton;
    private Object[] tablePrefabs;
    private int[] shuffeledNumbers;        // array of shuffeled number from 0-9 (to randomly select instantiated prafabs)
    private int[] shuffeledIds;        // array of shuffeled number from 0-5 (to randomly select instantiated prafabs)
    private static int shuffeledNumbersIndex = 0;
    private static int numberOfLable = 1;
    private Transform points;
    private GameObject[] pointsArr;
    private DataService dbgrabconnection;
    // Use this for initialization


    void Start()
    {
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
        for (int i = 0; i < instantiatedTableActiveGameObjects.Length; i++)
        {
            TableObject activeTableGameObject = instantiatedTableActiveGameObjects[i].GetComponent<TableObject>();

            Debug.Log(i + " " + instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().id);
            if (id.Equals(activeTableGameObject.id))
            {
                Debug.Log(instantiatedTableActiveGameObjects[i].gameObject.name);

                Collected_Objects tempCollectedObject = new Collected_Objects();
                // if (side.Equals(activeTableGameObject.side))
                if (side.Equals(activeTableGameObject.objectPosition))
                {
                    Debug.Log("ATTEMPTS " + activeTableGameObject.attempts);
                    activeTableGameObject.obj_recorded_on_pad = true;
                    activeTableGameObject.finishedRecordOnAttempt = true;
                    //dbgrabconnection.UpdateCollectedObjectOnPad(int.Parse(activeTableGameObject.id),activeTableGameObject.obj_recorded_on_pad);
                    activeTableGameObject.SetAttempts(activeTableGameObject.attempts++);
                    tempCollectedObject.SetValues(ExperementParameters.gameplay_id, int.Parse(activeTableGameObject.id), activeTableGameObject.objectPosition, BringLevelToString(int.Parse(activeTableGameObject.level)), activeTableGameObject.obj_recorded_on_pad, false, "", activeTableGameObject.attempts);

                    Debug.Log("tempCollectedObject " + tempCollectedObject.obj_recorded_after_attempt);

                    activeTableGameObject.SetCollectedObject(tempCollectedObject);
                    //  activeTableGameObject.SetAttempts(1); //first try and succeeded 

                    // dbgrabconnection.CreateCollectedObjectsRow(tempCollectedObject);
                    activeTableGameObject.canvas.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UiSprites/golden_star");
                    activeTableGameObject.canvas.GetChild(0).GetChild(0).GetComponent<Text>().enabled = false;
                   // break;
                }

                if (!activeTableGameObject.finishedRecordOnAttempt)
                {
                    activeTableGameObject.SetAttempts(activeTableGameObject.attempts++);

                }

            }
        }
    }

    void Initilize()
    {
        dbgrabconnection = new DataService("USN_Simulation.db");
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
        InvokeRepeating("LeapCameraIntialize", 1, 1);
        //  InvokeRepeating("CheckTableGameObjects", 4, 3);

    }
    void LeapCameraIntialize()
    {
        Debug.Log("LeapCameraIntialize");
        leapMotionCamera = GameObject.Find("GearVRCameraRigTEST") as GameObject;
        if (leapMotionCamera != null)
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
        newTableObject.transform.SetParent(tableWrapper.transform);
        if (!active)
        {
            newTableObject.transform.Find("Canvas").gameObject.SetActive(false);
            newTableObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Debug.Log("numberOfLable-1 = " + (numberOfLable - 1).ToString());
            instantiatedTableActiveGameObjects[numberOfLable - 1] = newTableObject;
            newTableObject.GetComponent<BoxCollider>().enabled = false;

            newTableObject.AddComponent<TableObject>();
            newTableObject.GetComponent<TableObject>().setValues((shuffeledIds[numberOfLable++]).ToString(), level.ToString(), objectPosition);


            TableObject activeTableGameObject = newTableObject.GetComponent<TableObject>();
            Collected_Objects tempCollectedObject = new Collected_Objects();
            tempCollectedObject.SetValues(ExperementParameters.gameplay_id, int.Parse(activeTableGameObject.id), activeTableGameObject.objectPosition, BringLevelToString(int.Parse(activeTableGameObject.level)), false, false, "", activeTableGameObject.attempts);

            activeTableGameObject.SetCollectedObject(tempCollectedObject);
            debugCollectedObjects(activeTableGameObject.collected_Objects);
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

    public void DoneWithTouchPad()
    {
        foreach (GameObject obj in instantiatedTableActiveGameObjects)
        {
            TableObject tObj = obj.GetComponent<TableObject>();
            tObj.canvas.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UiSprites/golden_star");
            tObj.canvas.GetChild(0).GetChild(0).GetComponent<Text>().enabled = false;
        }
        doneCollectingObjectsButton.SetActive(true);
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
        {
            return;
        }
        generateNextLevels(level + 1, activepointGroup, activepointdistance, objectPosition);
    }
    public void EnableAllGameObjectsBoxColliders()
    {
        Debug.Log("Box Colliders Disabled");
        for (int i = 0; i < instantiatedTableActiveGameObjects.Length; i++)
        {
            instantiatedTableActiveGameObjects[i].GetComponent<BoxCollider>().enabled = true;
        }
    }
    string BringLevelToString(int level)
    {
        string levelInString = "";
        switch (level)
        {
            case 1:
                {
                    return levelInString = "Personal";
                }
            case 2:
                {
                    return levelInString = "PeriPersonal";

                }
            case 3:
                {
                    return levelInString = "Far";
                }
            default:
                return "";
        }
    }

    public void RecordCollectedObjectsToDB()
    {
        Debug.Log("CheckTableGameObject");
        // int objectsDisabled = 0;
        for (int i = 0; i < instantiatedTableActiveGameObjects.Length; i++)
        {
            //if (instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().finishedRecord == true)
            //  {
            //  objectsDisabled++;
            debugCollectedObjects(instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().collected_Objects);
            // Debug.Log("For loop check v table game objects ");
            //   dbgrabconnection.CreateCollectedObjectsRow(instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().collected_Objects);
            //   CancelInvoke("CheckTableGameObjects");
            dbgrabconnection.CreateCollectedObjectsRow(instantiatedTableActiveGameObjects[i].GetComponent<TableObject>().collected_Objects);
            // }

        }

    }

    public void debugCollectedObjects(Collected_Objects collected_Objects)
    {
        Debug.Log("Collected Object from Array =  " + "game play id " + collected_Objects.gameplay_id + " obj number " + collected_Objects.obj_number + " obj Position " + collected_Objects.obj_position + " obj Field " + collected_Objects.obj_field + " obj recorded on pad " + collected_Objects.obj_recorded_on_pad + " obj collected " + collected_Objects.obj_collected + " obj collected by hand " + collected_Objects.obj_collected_by_hand + " atteMPTS " + collected_Objects.obj_recorded_after_attempt);
    }


}
