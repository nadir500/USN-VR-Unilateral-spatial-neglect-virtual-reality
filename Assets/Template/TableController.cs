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


        if (checkPointsController != null)
            checkPointsController.otherSideCheckPointReachedEvent += Initilize;
        else
            Initilize();
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
        GameObject newTableObject = Instantiate(tablePrefabs[shuffeledNumbers[shuffeledNumbersIndex++]], parent.GetChild(childIndex).position, Quaternion.identity) as GameObject;
        if (!active)
            newTableObject.transform.Find("Canvas").gameObject.SetActive(false);
        else if (direc.Equals("Right"))
        {
            newTableObject.transform.Find("Canvas").localEulerAngles = new Vector3(0, 270, 0);
            newTableObject.transform.Find("Canvas").GetChild(0).localScale = new Vector3(1, 1, 1);
            newTableObject.transform.Find("Canvas").GetChild(0).GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
        newTableObject.transform.Find("Canvas").GetChild(0).GetChild(0).GetComponent<Text>().text = numberOfLable++.ToString();

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
//{
//    int i = 0;
//    foreach (Transform level in points)         //lvl1 lvl2 lvl3
//    {
//        pointsPositions[i] = new Vector3[level.childCount][][][];
//        int j = 0;
//        foreach (Transform direction in level)        //right and left
//        {
//            pointsPositions[i][j] = new Vector3[direction.childCount][][];
//            int k = 0;
//            foreach (Transform group in direction)
//            {
//                pointsPositions[i][j][k] = new Vector3[group.childCount][];
//                int l = 0;
//                foreach (Transform dist in group)
//                {
//                    pointsPositions[i][j][k][l] = new Vector3[dist.childCount];
//                    int p = 0;
//                    foreach (Transform point in dist)
//                    {
//                        pointsPositions[i][j][k][l][p] = point.position;
//                    }
//                    p++;
//                }
//                k++;
//            }
//            j++;
//        }
//        i++;
//    }
//}

//int[] indeces = new int[tablePrefabs.Length];
//Vector3[] usedPositions = new Vector3[6];

//for (int i = 0; i < indeces.Length; indeces[i] = i++) ;
//System.Random rnd = new System.Random();
//int[] shuffeledNumbers = Enumerable.Range(0, indeces.Length-1).OrderBy(r => rnd.Next()).ToArray();
//for (int i = 0; i < 3; i++)
//{
//    for(int j = 0; j < 2; j++)
//    {
//        Vector3 pos = pointsPositions[i][j][Random.Range(0, pointsPositions[i][j].Length)];
//        Instantiate(tablePrefabs[shuffeledNumbers[j + (i*2)]], pos, Quaternion.identity);
//        usedPositions[j + (i * 2)] = pos;
//    }
//}