using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {


    public int numberOfPathsInSingleRoad = 4;
    public GameObject sidewalk;
    public GameObject streatPath;
    public GameObject midWalk;
    const int streatPathWidth = 10;
    const float sidewalkWidth = 2.5f;
    public GameObject BuildingsWrapper;
    private Vector3 RoadMeasure;
    private Vector3 startPosition = new Vector3(10f, -2.0f, 0.0f);
    private List<Vector3> beginPoints = new List<Vector3>();
    private List<float> endPoints = new List<float>();
     private GameObjectHandler car_handler1;

    void Start () {
       
        //by example on handler 
        car_handler1= new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject,numberOfPathsInSingleRoad,true,"");

      //instantiate all cars 

     //   Debug.Log("WAT " + _handler.RetrieveInstance(Vector3.zero,Quaternion.identity).gameObject.name);
      
        for(int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            RoadMeasure = new Vector3(10f + (streatPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad=  Instantiate(streatPath,RoadMeasure,Quaternion.identity) as GameObject;
             generatedRoad.name= "Road " +i;
            //two instantiates :D plzzzz
        
            Instantiate_Cars_FastRoad(RoadMeasure,RoadMeasure.z =200,generatedRoad.name,car_handler1);
        }
       
        Instantiate(midWalk, new Vector3(6.25f + streatPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
        
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
          RoadMeasure = new Vector3(12.5f + (streatPathWidth * i) + (streatPathWidth * (numberOfPathsInSingleRoad / 2)), -2.0f, 0.0f);
         GameObject generatedRoad = Instantiate(streatPath,RoadMeasure , Quaternion.identity);
     
     
       //two instantiates plzzzz
            Instantiate_Cars_FastRoad(RoadMeasure,RoadMeasure.z = 200.0f,generatedRoad.name,car_handler1);
        }
    
        Instantiate(midWalk, new Vector3(8.75f + streatPathWidth * (numberOfPathsInSingleRoad), -2.0f, 0.0f), Quaternion.identity);
        BuildingsWrapper.transform.position = new Vector3(9.3f + streatPathWidth * (numberOfPathsInSingleRoad), 0, 0);

    }
	void Update()
    {
        //making the cars move 
        MovingTheCars(car_handler1);

    }
    public void Instantiate_Cars_FastRoad( Vector3 beginPoint ,float endPoint,string roadParent
    ,GameObjectHandler carObjectHandler)
	{   
	   carObjectHandler.RetrieveInstance(beginPoint,Quaternion.identity,roadParent);
       beginPoints.Add(beginPoint);
       endPoints.Add(endPoint);
       
	}

    public void MovingTheCars(GameObjectHandler carObjectHandler)
    {
      GameObject car;
      float movementSpeed = 2.0f;
      float currentLerpTime=0;
      float LerpTime =5;
      GameObject[] carsPool = carObjectHandler.Pool;
     //  GameObject  carObject = carObjectHandler.Pool[];
     for (int i = 0; i < carsPool.Length; i++)
     {
         car = carsPool[i];
      //   Debug.Log(car.gameObject.name);
           
          if (car.transform.position.z == 0.0f)
            //frmmmmmmmmmmmmm
            {         

            }
            else
            {
                //eeeeek back to the start point 
               // car.transform.position= new Vector3(0,0,0);
            }
        }
    }


 }
