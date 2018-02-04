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
        Vector3 carVector;
        Debug.Log(ExperementParameters.streetsDirections);
        for (int i = 0; i < ExperementParameters.numberOfPathsPerStreet; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
            //and back cars which is from right to left direction

            int yRotate = (ExperementParameters.streetsDirections.Split()[0].Equals("Right")) ? -1 : +1;
            carVector = new Vector3 (/*little shift to right0.22f +*/ sidewalkWidth + (streetPathWidth / 4) + ( i * (streetPathWidth/2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars));

                GameObject car = carObjectHandler.RetrieveInstance(carVector, Quaternion.Euler(new Vector3(0, yRotate * -90, 0)));
                car.AddComponent<CarMove>();  //adding the car movement component  
                car.GetComponent<CarMove>().carDirection = ExperementParameters.streetsDirections.Split()[0];      //describe which direction 
                carRefernces.Add(car);        //referncing it to a list 


            if (ExperementParameters.streetsDirections.Length > 1)
            {

                yRotate = (ExperementParameters.streetsDirections.Equals("Left To Right")) ? -1 : 1;
                car = carObjectHandler.RetrieveInstance(new Vector3(/*-0.2f +*/ sidewalkWidth + (numberOfPathsInSingleRoad * (streetPathWidth/4)) + midwalkWidth + (i * (streetPathWidth/2)), -2.0f, yRotate * (190.0f + ExperementParameters.distanceBetweenCars)), Quaternion.Euler(new Vector3(0, yRotate * -90, 0)));
                car.AddComponent<CarMove>(); //adding the car moce component 
                car.GetComponent<CarMove>().carDirection = ExperementParameters.streetsDirections.Split()[2];  //descripe which direction 
                carRefernces.Add(car); //referncing it to a list 
            }
        }
    }
}
