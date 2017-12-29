using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {


    public int numberOfPathsInSingleRoad = 2;
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
    private float speed = 3.5f;

    void Start () {
       
        speed = Random.RandomRange(2, 4f);
        //by example on handler 
        car_handler1= new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject,numberOfPathsInSingleRoad,true,"");

      //instantiate all cars 

        for(int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            RoadMeasure = new Vector3(10f + (streatPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad=  Instantiate(streatPath,RoadMeasure,Quaternion.identity) as GameObject;
             generatedRoad.name= "Road " +i;
          
            //two instantiates :D plzzzz
                Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x,RoadMeasure.y ,RoadMeasure.z + 150),
                RoadMeasure.z =500
                ,generatedRoad,car_handler1);
        }
       
        Instantiate(midWalk, new Vector3(6.25f + streatPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
        
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
          RoadMeasure = new Vector3(12.5f + (streatPathWidth * i) + (streatPathWidth * (numberOfPathsInSingleRoad / 2)), -2.0f, 0.0f);
         GameObject generatedRoad = Instantiate(streatPath,RoadMeasure , Quaternion.identity);
     
     
       //two instantiates plzzzz
            Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x,RoadMeasure.y ,RoadMeasure.z + 150),
            RoadMeasure.z = 200.0f/*END POINT TO REPEAT*/,generatedRoad,car_handler1);
        }
    
        Instantiate(midWalk, new Vector3(8.75f + streatPathWidth * (numberOfPathsInSingleRoad), -2.0f, 0.0f), Quaternion.identity);
        
        BuildingsWrapper.transform.position = new Vector3(9.3f + streatPathWidth * (numberOfPathsInSingleRoad), 0, 0);

    }
	void Update()
    {
        //making the cars move 
      //  MovingTheCars(car_handler1);

    }
    public void Instantiate_Cars_FastRoad( Vector3 beginPoint ,float endPoint,GameObject roadParent
    ,GameObjectHandler carObjectHandler)
	{   
    for (int i = 0; i<2; i++)
     {    
        GameObject car= carObjectHandler.RetrieveInstance(new Vector3(beginPoint.x+ 4.0f*i ,beginPoint.y,beginPoint.z + 1.0f*i), Quaternion.Euler(new Vector3(0, -90,0)));
        car.transform.localRotation =  Quaternion.Euler(new Vector3(0, -90, 0));
        car.AddComponent<CarMove>();
        car.transform.parent = roadParent.transform;
     }
     //   Debug.Log("CARS =" + car.transform.localRotation.y);

	  //  car.transform.parent = GameObject.Find(roadParent) as GameObject;
        //beginPoints.Add(beginPoint);
        //endPoints.Add(endPoint);
       
	}

    //Moving Cars in Update 
    /*public void MovingTheCars(GameObjectHandler carObjectHandler)
    {
      GameObject car;
      GameObject[] carsPool = carObjectHandler.Pool;

     //  GameObject  carObject = carObjectHandler.Pool[];
     for (int i = 0; i < carsPool.Length; i++)
     {
         car = carsPool[i];
      //   Debug.Log(car.gameObject.name);
           
          if (car.transform.position.z == 0.0f)
            //frmmmmmmmmmmmmm
            {         
            car.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
            Debug.Log("{0} "+i +car.transform.localPosition.z );

            }
            else
            {
                //eeeeek back to the start point 
               // car.transform.position= new Vector3(0,0,0);
            }
        }
    }*/


 }
