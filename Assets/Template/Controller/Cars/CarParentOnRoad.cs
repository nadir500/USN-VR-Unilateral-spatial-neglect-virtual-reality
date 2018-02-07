using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParentOnRoad : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] carRefernces;
    public int index = 0;
    private CheckPointsController checkPointsController;
    private bool hitByCar = false;
    void Start()
    {
        carRefernces = new GameObject[this.transform.childCount];
        checkPointsController= GameObject.Find("CheckPointController").GetComponent<CheckPointsController>();
        //Debug.Log("LEngth "+ carRefernces.Length);
        for (int i = 0; i < carRefernces.Length; i++)
        {
            carRefernces[i] = this.transform.GetChild(i).gameObject;
        }
        if(this.gameObject.name.Split(' ')[1].Equals("1"))
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
