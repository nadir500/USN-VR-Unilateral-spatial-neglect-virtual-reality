using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RoadController : MonoBehaviour
{


    public GameObject sidewalk;
    public GameObject streatPath;
    public GameObject midWalk;
    const int streetPathWidth = 5; //feet
    const float sidewalkWidth = 2.5f;
    public GameObject BuildingsWrapper;
    public static bool fadeout_after_crossing = true;
    private int numberOfPathsInSingleRoad = 2;
    private Vector3 RoadMeasure;
    private Vector3 startPosition = new Vector3(10f, -2.0f, 0.0f);
    private List<Vector3> beginPoints = new List<Vector3>();
    private List<float> endPoints = new List<float>();
    private GameObjectHandler car_handler1;
    string[] streetsDirections;

    public GameObject midWalkYellowPoint = null;
    public GameObject sideWalkYellowPoint = null;

    private GameObject yellowArrowsFirstPath = null;
    private GameObject yellowArrowsSecondPath = null;

    public GameObject yellowPoint;
    public GameObject yellowArrows;
    private BoxCollider checkPointBoxCollider;

    AudioSource audioSource;

    public ExperimentObserves experimentObserves;
    public List<GameObject> carsReferences;
    private int pathGenerateIndex = 0;

    public void generateRoads()
    {
        StringBuilder stringBuilder;
        //Assigning number of paths from the UI

        numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        carsReferences = new List<GameObject>();
       // Debug.Log("Car Refernces "+carsReferences.Length); 

        //making a prefab copy with a number enough to coer a whole one path 
        car_handler1 = new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject, numberOfPathsInSingleRoad, true, "");
        //i am using string builder to rename the roads into a correct format just to make it easy reaching them

        streetsDirections = ExperementParameters.streetsDirections.Split(' ');

        //Debug.Log("streetsDirections");
        //for(int i = 0; i < streetsDirections.Length; i++)
        //    Debug.Log("streetsDirections["+i+"] = "+ streetsDirections[i]);

        float lastPosition = 5f + streetPathWidth * (numberOfPathsInSingleRoad / 2);
        yellowArrowsFirstPath = Instantiate(yellowArrows, new Vector3(4.7f + (streetPathWidth * numberOfPathsInSingleRoad / 4), -1.99f, -8.98f), Quaternion.identity);
        if (ExperementParameters.streetsDirections.Split()[0].Equals("Right"))
            yellowArrowsFirstPath.transform.localScale = new Vector3(1, 1, -1);
        //Road #1
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            stringBuilder = new StringBuilder();
            RoadMeasure = new Vector3(7.51f + (streetPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad = Instantiate(streatPath, RoadMeasure, Quaternion.identity) as GameObject;
            foreach(Transform child in generatedRoad.transform)
            {
                child.gameObject.name = pathGenerateIndex.ToString();
                pathGenerateIndex++;
            }
            //i'll take each road generated (the cars are from left to right movement) and rename it into a specific name
            //i used string builder for the performance issues
            stringBuilder.Append("Road ");
            stringBuilder.Append(streetsDirections[0] + " ");
            stringBuilder.Append(i + 1);
            generatedRoad.name = stringBuilder.ToString();

            //now i am instantiating the cars after preparing them in the line '31'
            Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x, RoadMeasure.y, RoadMeasure.z + 150), //here i'll take the road position from line 37 as the position of the generated  cars and the parent  is of course the road 
            RoadMeasure.z = 500 //do not be alerted by this parameter i'll remove it later 
            , generatedRoad //the road game object
            , car_handler1); //passing the handler to summon a function that make a new gameobject to the scene (cars)
        }
        if (streetsDirections.Length > 1)
        {
            lastPosition = 6.25f + (streetPathWidth * numberOfPathsInSingleRoad);
            // add the mid walk
            Instantiate(midWalk, new Vector3(5.68f + streetPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);


            midWalkYellowPoint = Instantiate(yellowPoint, new Vector3(5.6f + streetPathWidth * (numberOfPathsInSingleRoad / 2), -0.5f, -8.98f), Quaternion.identity);
            midWalkYellowPoint.name = "midwalkYellowPoint";

            yellowArrowsSecondPath = Instantiate(yellowArrows, new Vector3(-3.8f + lastPosition + 0.5f, -1.99f, -8.98f), Quaternion.identity);
            if (ExperementParameters.streetsDirections.Equals("Left To Right"))
                yellowArrowsSecondPath.transform.localScale = new Vector3(1, 1, -1);
            //yellowArrowsSecondPath.transform.localScale = new Vector3(1, 1, -1);
            //Road #2
            for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
            {
                stringBuilder = new StringBuilder();
                //i'll take each road generated (the cars are from right to left movement) and rename it into a specific name via string builder

                RoadMeasure = new Vector3(8.85f + (streetPathWidth * i) + (streetPathWidth * (numberOfPathsInSingleRoad / 2)), -2.0f, 0.0f);
                GameObject generatedRoad = Instantiate(streatPath, RoadMeasure, Quaternion.identity);
                foreach (Transform child in generatedRoad.transform)
                {
                    child.gameObject.name = pathGenerateIndex.ToString();
                    pathGenerateIndex++;


                }
                stringBuilder.Append("Road ");
                stringBuilder.Append(streetsDirections[2] + " ");
                stringBuilder.Append(i + 1);
                generatedRoad.name = stringBuilder.ToString();

                //now i am instantiating the cars after preparing them in the line '59'
                Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x, RoadMeasure.y, RoadMeasure.z + 150),//here i'll take the road position from line 37 as the position of the generated  cars and the parent  is of course the road 
                RoadMeasure.z = 200.0f //i'll delete it later :/ 
                , generatedRoad //the road game object 
                , car_handler1);  //passing the handler to summon a function that make a new gameobject to the scene (cars)
            }


        }
        checkPointBoxCollider = midWalk.AddComponent<BoxCollider>();
        checkPointBoxCollider.size = new Vector3(14.5f, 0.46f, 10);
        checkPointBoxCollider.isTrigger = true;
        Instantiate(sidewalk, new Vector3(lastPosition, -0.0012f, 0.0f), Quaternion.identity);
        sideWalkYellowPoint = Instantiate(yellowPoint, new Vector3(lastPosition + 0.25f, -0.5f, -8.98f), Quaternion.identity);
        sideWalkYellowPoint.name = "sidewalkYellowPoint";

        sideWalkYellowPoint.SetActive(false);
        BuildingsWrapper.transform.position = new Vector3(lastPosition + 8f, 0, 0);

        StartCoroutine(TurnOnAndOfYellowArrows());


        experimentObserves.roadController = this;
        experimentObserves.carsReferences = this.carsReferences;
        experimentObserves.Initilize();

    }
    IEnumerator playSound(string s)
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        AudioClip ac = Resources.Load("Audio/DRSounds/" + s) as AudioClip;
        Debug.Log("Clip" + ac.name);
        audioSource.clip = ac;
        yield return new WaitForSeconds(1);

        audioSource.Play();
    }
    IEnumerator TurnOnAndOfYellowArrows()
    {
        for (int i = 0; i < 4; i++)
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
        StartCoroutine(playSound("Go"));
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
