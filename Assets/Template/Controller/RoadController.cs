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


    public const float streetPathWidth = 5;        //  the width of pair of paths
    public const float sidewalkWidth = 5f;         //  the width of sidewalk
    public const float midwalkWidth = 1.36f;       //  the width of midwalk
    private Vector3 RoadMeasure;

    public static bool fadeout_after_crossing = true;       // ?? (to nadir)
    private BoxCollider checkPointBoxCollider;              // ?? (to nadir)

    private GameObjectHandler car_handler1;                 // ?? (to nadir)
    string[] streetsDirections;                             // ?? (to nadir)



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
        int numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        carsReferences = new List<GameObject>();
        car_handler1 = new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject, numberOfPathsInSingleRoad, true, "");//making a prefab copy with a number enough to coer a whole one path 
                                                                                                                               //i am using string builder to rename the roads into a correct format just to make it easy reaching them

        float lastPosition = sidewalkWidth + midwalkWidth + (streetPathWidth/4) +streetPathWidth * (numberOfPathsInSingleRoad / 2);

        yellowArrowsFirstPath = Instantiate(yellowArrows, new Vector3(4.7f + (streetPathWidth * numberOfPathsInSingleRoad / 4), -1.99f, -8.98f), Quaternion.identity);
        if (ExperementParameters.streetsDirections.Split()[0].Equals("Right"))
            yellowArrowsFirstPath.transform.localScale = new Vector3(1, 1, -1);

        //Road #1
        createDirection(sidewalkWidth + (streetPathWidth / 2), ref pathGenerateIndex, 0);

        if (ExperementParameters.streetsDirections.Length > 1)
        {

            Instantiate(midWalk, new Vector3(sidewalkWidth + (midwalkWidth / 2) + streetPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);

            yellowArrowsSecondPath = Instantiate(yellowArrows, new Vector3(lastPosition, -1.99f, -8.98f), Quaternion.identity);
            if (ExperementParameters.streetsDirections.Equals("Left To Right"))
                yellowArrowsSecondPath.transform.localScale = new Vector3(1, 1, -1);

            //Road #2
            createDirection(sidewalkWidth + (streetPathWidth / 2) + midwalkWidth + (streetPathWidth * (numberOfPathsInSingleRoad / 2)), ref pathGenerateIndex, 2);

        }
        Instantiate(sidewalk, new Vector3((sidewalkWidth) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), -0.0012f, 0.0f), Quaternion.identity);

        BuildingsWrapper.transform.position = new Vector3((sidewalkWidth * 2) + (midwalkWidth) + streetPathWidth * (numberOfPathsInSingleRoad), 0, 0);

        checkPointBoxCollider = midWalk.AddComponent<BoxCollider>();
        checkPointBoxCollider.size = new Vector3(14.5f, 0.46f, 10);
        checkPointBoxCollider.isTrigger = true;


    }

    public void createDirection(float startPositionAtX, ref int pathGenerateIndex, int indexOfDirection)
    {
        int numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        streetsDirections = ExperementParameters.streetsDirections.Split(' ');

        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            RoadMeasure = new Vector3(startPositionAtX + (streetPathWidth * i), -2.0f, 0.0f);
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

            //now i am instantiating the cars after preparing them in the line '31'
            Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x, RoadMeasure.y, RoadMeasure.z + 150), //here i'll take the road position from line 37 as the position of the generated  cars and the parent  is of course the road 
            RoadMeasure.z = 500 //do not be alerted by this parameter i'll remove it later 
            , generatedRoad //the road game object
            , car_handler1); //passing the handler to summon a function that make a new gameobject to the scene (cars)
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

    /*we need to instantiate the cars in the scene with the perfect positions on the road when generating it */
    public void Instantiate_Cars_FastRoad(
                                Vector3 beginPoint, //the generated road from lines 31 59
                                               float endPoint, GameObject roadParent  //the road gameObject that is generated
                                                    , GameObjectHandler carObjectHandler) //the handler from object pooling class
    {
        //here everytime i am taking the gameObject.name of the road and spliting it then taking the index [1] to know which direction this road is    
        string[] roadType = roadParent.name.Split(' ');

        for (int i = 0; i < 2; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
            //and back cars which is from right to left direction
            if (roadType[1].Equals(value: "Left"))  //from left to right 
            {
                //now instantiate the cars with the positions explained above 
                GameObject car = carObjectHandler.RetrieveInstance(
                    new Vector3(0.3f/*way from the edge of the corner*/+ beginPoint.x + 2.5f * i, beginPoint.y, beginPoint.z + ExperementParameters.distanceBetweenCars * i), //putting the position with the distance between each car
                                                                        Quaternion.Euler(new Vector3(0, -90, 0))); //the rotation of course 
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0)); //this is temporary 
                car.transform.parent = roadParent.transform; //and then putting it as a child to the "Side_Go + i" generated road
                car.AddComponent<CarMove>(); //adding the car movement component  
                carsReferences.Add(car);
                Debug.Log("Car Left References " + carsReferences[i].name);

            }

            if (roadType[1].Equals(value: "Right")) //from right to left 
            {
                //now instantiate the cars with the positions explained above 

                GameObject car = carObjectHandler.RetrieveInstance(
                    new Vector3(/*adjust the distance from the edge of the corner*/beginPoint.x - 0.3f - 2.5f * i, beginPoint.y, beginPoint.z - ExperementParameters.distanceBetweenCars * i),//putting the position with the distance between each car
                                                                    Quaternion.Euler(new Vector3(0, 90, 0)));//the rotation of course
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));//this is temporary
                car.transform.parent = roadParent.transform; //and then putting it as a child to the "Side_Go + i" generated road
                car.transform.position += new Vector3(0, 0, -360); //this is for making a translate to -400 which is far far right 
                car.AddComponent<CarMove>(); //adding the car moce component 
                carsReferences.Add(car);
                Debug.Log("Car Left References " + carsReferences[i].name);
            }
        }
    }
}