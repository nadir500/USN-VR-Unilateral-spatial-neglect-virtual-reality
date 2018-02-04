﻿using System.Collections;
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
        checkPointsController.startTheGameCheckPointReachedEvent += InstantiateCarsFastRoad;
    }

    /*we need to instantiate the cars in the scene with the perfect positions on the road when generating it */
    public void InstantiateCarsFastRoad()
    {
        string[] carDirection = ExperementParameters.streetsDirections.Split(' '); //knowing which rotation and direction to instatiate the car
        int numberOfPathsInSingleRoad = ExperementParameters.numberOfPathsPerStreet;
        GameObjectHandler carObjectHandler =
                          new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject, //pooling from the prefab with copies that is like the number of paths in each street
                                                      numberOfPathsInSingleRoad*2,
                                                                              true, "");//making a prefab copy with a number enough to coer a whole one path 
        Debug.Log(ExperementParameters.streetsDirections);
        for (int i = 0; i < ExperementParameters.numberOfPathsPerStreet; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
            //and back cars which is from right to left direction
            if (carDirection[0].Equals(value: "Left") || carDirection[2].Equals(value: "Left"))  //from left to right this is wrong i think ... remember me to till you why..
            {
                GameObject car = carObjectHandler.RetrieveInstance(new Vector3(/*little shift to right*/0.22f + sidewalkWidth + (streetPathWidth / 2) + ( i * (streetPathWidth/2)), -2.0f, 190.0f + ExperementParameters.distanceBetweenCars*i), Quaternion.Euler(new Vector3(0, -90, 0)));
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0)); //this is temporary 
                car.AddComponent<CarMove>();  //adding the car movement component  
                car.GetComponent<CarMove>().carDirection = "Left";      //describe which direction 
                carRefernces.Add(car);        //referncing it to a list 
            }

            if (carDirection[0].Equals(value: "Right") || carDirection[2].Equals(value: "Right"))  //from right to left 
            {
                GameObject car = carObjectHandler.RetrieveInstance(new Vector3(-0.2f + sidewalkWidth + (numberOfPathsInSingleRoad * (streetPathWidth/2)) + midwalkWidth + (i * (streetPathWidth/2)), -2.0f, -190.0f-ExperementParameters.distanceBetweenCars*i), Quaternion.identity);

                car.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));//this is temporary
                car.AddComponent<CarMove>(); //adding the car moce component 
                car.GetComponent<CarMove>().carDirection = "Right";  //descripe which direction 
                carRefernces.Add(car); //referncing it to a list 
            }
        }
    }
}