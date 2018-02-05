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

    public static int numberOfRenderdCars;

    public CheckPointsController checkPointsController;

    void Start()
    {
        IntializeCars();
    }

    void IntializeCars()
    {
        //intialze the cars references 
        carRefernces = new List<GameObject>();
         checkPointsController.startTheGameCheckPointReachedEvent += CarsOnFastRoad;
    }
    public void CarsOnFastRoad()
    {
          if(MainMenu.playMode ==1)
             StartCoroutine(InstantiateCarsFastRoad());
    }
    /*we need to instantiate the cars in the scene with the perfect positions on the road when generating it */
    public IEnumerator InstantiateCarsFastRoad()
    {
        string[] carDirection = ExperementParameters.streetsDirections.Split(' '); //knowing which rotation and direction to instatiate the car
        int numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        GameObjectHandler carObjectHandler =
                          new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject, //pooling from the prefab with copies that is like the number of paths in each street
                                                      numberOfPathsInSingleRoad * 2,
                                                                              true, "");//making a prefab copy with a number enough to coer a whole one path 
        Debug.Log(ExperementParameters.streetsDirections);
        Vector3 carVector;
        for (int i = 0; i < ExperementParameters.numberOfPathsPerStreet; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
            //and back cars which is from right to left direction
            int yRotate = (ExperementParameters.streetsDirections.Split()[0].Equals("Right")) ? -1 : +1;
            carVector = new Vector3(/*little shift to right0.22f +*/ sidewalkWidth + (streetPathWidth / 4) + (i * (streetPathWidth / 2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars));
            GameObject car = carObjectHandler.RetrieveInstance(new Vector3(/*little shift to right0.22f +*/ sidewalkWidth + (streetPathWidth / 4) + (i * (streetPathWidth / 2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars)), Quaternion.Euler(new Vector3(0, yRotate * -90, 0)));
            // StartCoroutine(CallBackRetrieveInstance(carObjectHandler, carVector, Quaternion.Euler(new Vector3(0, yRotate * -90, 0))));

            car.AddComponent<CarMove>();  //adding the car movement component  
            car.GetComponent<CarMove>().carDirection = ExperementParameters.streetsDirections.Split()[0];      //describe which direction 
            carRefernces.Add(car);        //referncing it to a list 

            yield return new WaitForSeconds(ExperementParameters.distanceBetweenCars);

            if (ExperementParameters.streetsDirections.Length > 1)
            {
                yRotate = (ExperementParameters.streetsDirections.Equals("Left To Right")) ? -1 : 1;
                // carVector = new Vector3(/*-0.2f +*/ sidewalkWidth + (numberOfPathsInSingleRoad * (streetPathWidth / 2)) + midwalkWidth +(streetPathWidth / 4)+ (i * (streetPathWidth / 2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars));
                //  StartCoroutine(CallBackRetrieveInstance(carObjectHandler, carVector, Quaternion.Euler(new Vector3(0, yRotate * -90, 0))));

                car = carObjectHandler.RetrieveInstance(new Vector3(/*-0.2f +*/ sidewalkWidth + (numberOfPathsInSingleRoad * (streetPathWidth / 2)) + midwalkWidth +(streetPathWidth / 4)+ (i * (streetPathWidth / 2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars)), Quaternion.Euler(new Vector3(0, yRotate * -90, 0)));
                //car= CallBackRetrieveInstance(carObjectHandler,carVector, Quaternion.Euler(new Vector3(0, yRotate * -90, 0)));
                car.AddComponent<CarMove>(); //adding the car moce component 
                car.GetComponent<CarMove>().carDirection = ExperementParameters.streetsDirections.Split()[2];  //descripe which direction 
                carRefernces.Add(car); //referncing it to a list 
                yield return new WaitForSeconds(ExperementParameters.distanceBetweenCars);
            }
        }
    }
    private IEnumerator CallBackRetrieveInstance(GameObjectHandler carHandler, Vector3 carPosition, Quaternion carRotation)
    {
        yield return new WaitForSeconds(6);
        GameObject car = carHandler.RetrieveInstance(carPosition, carRotation);
        car.AddComponent<CarMove>();  //adding the car movement component  
        car.GetComponent<CarMove>().carDirection = ExperementParameters.streetsDirections.Split()[0];      //describe which direction 
        carRefernces.Add(car);        //referncing it to a list 

    }




}
