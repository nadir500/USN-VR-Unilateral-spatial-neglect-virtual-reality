using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [HideInInspector]
    public List<GameObject> carRefernces;
    public const float streetPathWidth = 5;        //  the width of pair of paths
    public const float sidewalkWidth = 5f;         //  the width of sidewalk
    public const float midwalkWidth = 1.36f;       //  the width of midwalk

    public CheckPointsController checkPointsController;

    void Start()
    {
        IntializeCars();
    }

    void IntializeCars()
    {
        //intialze the cars references 
        carRefernces = new List<GameObject>();
        checkPointsController.startTheGameCheckPointReachedEvent += InstantiateCarsFastRoad;
    }

    /*we need to instantiate the cars in the scene with the perfect positions on the road when generating it */
    public void InstantiateCarsFastRoad()
    {
        string[] carDirection = ExperementParameters.streetsDirections.Split(); //knowing which rotation and direction to instatiate the car
        int numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        float lastPosition = sidewalkWidth + midwalkWidth + (streetPathWidth / 4) + streetPathWidth * (numberOfPathsInSingleRoad / 2);
        GameObjectHandler carObjectHandler =
                          new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject, //pooling from the prefab with copies that is like the number of paths in each street
                                                      numberOfPathsInSingleRoad*2,
                                                                              true, "");//making a prefab copy with a number enough to coer a whole one path 
        Debug.Log(ExperementParameters.streetsDirections);
        for (int i = 0; i < ExperementParameters.numberOfPathsPerStreet; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
            //and back cars which is from right to left direction
            if (carDirection[0].Equals(value: "Left") || carDirection[2].Equals(value: "Left"))  //from left to right 
            {
                GameObject car = carObjectHandler.RetrieveInstance(new Vector3(4.7f + (streetPathWidth * numberOfPathsInSingleRoad / 4), -2.0f, 0.0f), Quaternion.Euler(new Vector3(0, -90, 0)));
                Debug.Log("Instantiaed");
                //now instantiate the cars with the positions explained above 
                //  GameObject car = carObjectHandler.RetrieveInstance(
                //  new Vector3(0.3f/*way from the edge of the corner*/+ roadParent.position.x + 2.5f * i, roadParent.position.y, roadParent.position.z + ExperementParameters.distanceBetweenCars * i + 195.0f), //putting the position with the distance between each car
                //                Quaternion.Euler(new Vector3(0, -90, 0))); //the rotation of course 
                //
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0)); //this is temporary 
                car.AddComponent<CarMove>();  //adding the car movement component  
                car.GetComponent<CarMove>().carDirection = "Left";      //describe which direction 
                carRefernces.Add(car);        //referncing it to a list 
            }

            if (carDirection[0].Equals(value: "Right") || carDirection[2].Equals(value: "Right"))  //from right to left 
            {
                GameObject car = carObjectHandler.RetrieveInstance(new Vector3(4.7f + (streetPathWidth * numberOfPathsInSingleRoad / 4), -2.0f, 0.0f), Quaternion.identity);

                //now instantiate the cars with the positions explained above 
                // GameObject car = carObjectHandler.RetrieveInstance(
                //    new Vector3(roadParent.position.x - 0.3f - 2.5f * i, roadParent.position.y, roadParent.position.z - 195.0f - ExperementParameters.distanceBetweenCars * i),//putting the position with the distance between each car
                //                 Quaternion.Euler(new Vector3(0, 90, 0)));   //the rotation of course

                car.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));//this is temporary
                car.AddComponent<CarMove>(); //adding the car moce component 
                car.GetComponent<CarMove>().carDirection = "Right";  //descripe which direction 
                carRefernces.Add(car); //referncing it to a list 
            }
        }
    }
}
