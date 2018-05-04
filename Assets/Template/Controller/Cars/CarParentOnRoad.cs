using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParentOnRoad : MonoBehaviour
{

    // This class aim to manage the generated road through adding it as a component 
    //in a "parent" after generating every lane 

    public GameObject[] carReferences;  //array references to the children (cars) in the parent attached with this class 
    public int index = 0; //index of the car we're trying to generate in every lane from the array above
    private CheckPointsController checkPointsController;  //helping us in detecting if the car hit the player or not 
    private bool hitByCar = false;  //determine weather you hit a car or not 
    private GameObject[] carHandlerPool;  //cars object pool as reference to _pool from the object pooling class 
    void Start()
    {
        //initialize the array 
        carReferences = new GameObject[this.transform.childCount];
        //getting the reference ready 
        carHandlerPool = new GameObject[ExperimentParameters.lanes_per_direction * 10 * 2];
        //initialize checkpoint controller 
        checkPointsController = GameObject.Find("CheckPointController").GetComponent<CheckPointsController>();
        for (int i = 0; i < carReferences.Length; i++)  //reference every child to the array 
        {
            carReferences[i] = this.transform.GetChild(i).gameObject;
        }

        if (this.gameObject.name.Split(' ')[1].Equals("1"))   //making some distance between each car in different lanes
            InvokeRepeating("SpawnCar", 5, ExperimentParameters.distanceBetweenCars);
        else
            InvokeRepeating("SpawnCar", 1, ExperimentParameters.distanceBetweenCars);
    }
    //entering this method in a loop for spawning cars using setActive for effectiveness  
    void SpawnCar()  //spawning cars for a while the distance between cars is the main parameter in the generating process after making an object pool for the cars
    {
        
        if (index < carReferences.Length && !hitByCar)  //if the patient didn't hit a car
        {
        
            carReferences[index].SetActive(true);
            index++;
        }
        else
        {
            index = 0;
            Debug.Log("Now respawn the first one (the car index is zero now)");
        }

        if (RoadController.fadeout_after_crossing)  //after fade make all cars disappear and do not enter the 1st condition and intialize hitByCar variable for a new record on the next road 
        {
            hitByCar = false;
        }
    }
    //making all cars disappear except the one that hit the player SetActive(true)
    public void StopAllCarsAfterAccident(int carIndex)  
    {
        carHandlerPool = GameObject.Find("CarController").GetComponent<CarController>().carObjectHandler.Pool;
        for (int i = 0; i < carHandlerPool.Length; i++)
        {
            if (i != carIndex)
            {
                carHandlerPool[i].SetActive(false);
                Debug.Log("Done car referencing set active false ");
            }
        }
        hitByCar = true;
    }
}
