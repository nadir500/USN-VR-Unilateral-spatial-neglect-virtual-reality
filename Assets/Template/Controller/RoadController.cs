using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    //private GameObject car;
    void Start () {
       
       // speed = Random.RandomRange(2, 4f);
        //by example on handler 
        car_handler1= new GameObjectHandler(Resources.Load("Prefabs/Car") as GameObject,numberOfPathsInSingleRoad,true,"");
        StringBuilder stringBuilder =new StringBuilder();

      //instantiate all cars 

        for(int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            RoadMeasure = new Vector3(10f + (streatPathWidth * i), -2.0f, 0.0f);
            GameObject generatedRoad=  Instantiate(streatPath,RoadMeasure,Quaternion.identity) as GameObject;
             
             stringBuilder.Append("Road ");
             stringBuilder.Append("Side_Go ");
             stringBuilder.Append(i+1);

             generatedRoad.name = stringBuilder.ToString();
          //  generatedRoad.tag = "GoingCars";
            //two instantiates :D plzzzz
                Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x,RoadMeasure.y ,RoadMeasure.z + 150),
                RoadMeasure.z =500
                ,generatedRoad,car_handler1);
        }
       
        Instantiate(midWalk, new Vector3(6.25f + streatPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
        
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
          stringBuilder = new StringBuilder();

          RoadMeasure = new Vector3(12.5f + (streatPathWidth * i) + (streatPathWidth * (numberOfPathsInSingleRoad / 2)), -2.0f, 0.0f);
          GameObject generatedRoad = Instantiate(streatPath,RoadMeasure , Quaternion.identity);
               
             stringBuilder.Append("Road ");
             stringBuilder.Append("Side_Back ");
             stringBuilder.Append(i+1);
             generatedRoad.name = stringBuilder.ToString();
             //generatedRoad.tag = "BackCars";
     
       //two instantiates plzzzz
            Instantiate_Cars_FastRoad(new Vector3(RoadMeasure.x,RoadMeasure.y ,RoadMeasure.z + 150),
            RoadMeasure.z = 200.0f/*END POINT TO REPEAT*/,generatedRoad,car_handler1);
        }
    
        Instantiate(midWalk, new Vector3(8.75f + streatPathWidth * (numberOfPathsInSingleRoad), -2.0f, 0.0f), Quaternion.identity);
        
        BuildingsWrapper.transform.position = new Vector3(9.3f + streatPathWidth * (numberOfPathsInSingleRoad), 0, 0);

    }
    public void Instantiate_Cars_FastRoad( 
                                Vector3 beginPoint ,
                                               float endPoint,GameObject roadParent
                                                    ,GameObjectHandler carObjectHandler)
	{
     string[]  roadType = roadParent.name.Split(' ');
     
        for (int i = 0; i < 2; i++)
        {
            if (roadType[1] == "Side_Go")
            {
                GameObject car = carObjectHandler.RetrieveInstance(
                    new Vector3(beginPoint.x + 4.0f * i, beginPoint.y, beginPoint.z + 1.0f * i),
                                                                        Quaternion.Euler(new Vector3(0, -90, 0)));

                car.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
                car.transform.parent = roadParent.transform;
                car.AddComponent<CarMove>();

            }
            else
            if (roadType[1] =="Side_Back")
            {
                GameObject car = carObjectHandler.RetrieveInstance(
                    new Vector3(beginPoint.x - 4.0f * i, beginPoint.y, beginPoint.z - 1.0f * i),
                                                                    Quaternion.Euler(new Vector3(0, 90, 0)));

                car.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                car.transform.parent = roadParent.transform;
                car.transform.position +=new Vector3(0,0,-400);
                car.AddComponent<CarMove>();

            }

        }

    }
 }
