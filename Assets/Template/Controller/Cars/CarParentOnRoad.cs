using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParentOnRoad : MonoBehaviour
{

    // This class aim to manage the generated road through adding it as a component in a "parent" after generating evey lane 
    public GameObject[] carRefernces;  //array refernces to the childs (cars) in the parent attached with this class 
    public int index = 0; //index of the car we're trying to generate in every lane from the array above
    private CheckPointsController checkPointsController;  //helping us in detecting if the car hit the player or not 
    private bool hitByCar = false;  //determine weather you hit a car or not 
    void Start()
    {
        //intialize the array 
        carRefernces = new GameObject[this.transform.childCount];
        //intialize chackpoint controller 
        checkPointsController= GameObject.Find("CheckPointController").GetComponent<CheckPointsController>();
        for (int i = 0; i < carRefernces.Length; i++)  //reference every child to the array 
        {
            carRefernces[i] = this.transform.GetChild(i).gameObject;
        }
        if(this.gameObject.name.Split(' ')[1].Equals("1"))   //making some distance between each car in different lanes
        InvokeRepeating("SpawnCar", 3, ExperementParameters.distanceBetweenCars);
        else
         InvokeRepeating("SpawnCar", 1, ExperementParameters.distanceBetweenCars);
    }
    void SpawnCar()
    {
        if (index < carRefernces.Length &&!hitByCar)
        {
            carRefernces[index].SetActive(true);
            index++;
        }
        else
        {
            index = 0;
            Debug.Log("Now respawn the first one (the car index is zero now)");
        }

        if(RoadController.fadeout_after_crossing)
        {
            hitByCar=false;
        }
     
    }   
    public void StopAllCarsAfterAccident(int carIndex)
    {
        for (int i = 0; i < carRefernces.Length; i++)
        {
            if (i != carIndex)
                carRefernces[i].SetActive(false);
        }
        hitByCar = true;
    }
}
