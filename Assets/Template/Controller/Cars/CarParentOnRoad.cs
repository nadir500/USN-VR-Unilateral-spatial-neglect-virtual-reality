using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CarParentOnRoad : MonoBehaviour
{

    // This class aim to manage the generated road through adding it as a component 
    //in a "parent" after generating every lane 

    public GameObject[] carReferences;  //array references to the children (cars) in the parent attached with this class 
    public int index = 0; //index of the car we're trying to generate in every lane from the array above
    public float carSpeed;
    CarController carController;
    private CheckPointsController checkPointsController;  //helping us in detecting if the car hit the player or not 
    private bool hitByCar = false;  //determine weather you hit a car or not 
    private GameObject[] carHandlerPool;  //cars object pool as reference to _pool from the object pooling class 
    private StringBuilder stringBuilder;
    private StringBuilder namingCarStringBuilder;

    void Start()
    {
        //random spawn number
        float spawnInRadomNumber = 0.0f;
        string[] stringSubset;
        //initialize the array 
        carReferences = new GameObject[this.transform.childCount];
        //getting the reference ready 
        carHandlerPool = new GameObject[ExperimentParameters.numberOfRoads * 10 * 2];
        //initialize checkpoint controller 
        checkPointsController = GameObject.Find("CheckPointController").GetComponent<CheckPointsController>();
        carSpeed = ChooseSpeedRandom(ExperimentParameters.carsSpeed);
        namingCarStringBuilder = new StringBuilder();
        for (int i = 0; i < carReferences.Length; i++)  //reference every child to the array 
        {
            // namingCarStringBuilder.Length = 0;
            carReferences[i] = this.transform.GetChild(i).gameObject;
            //namingCarStringBuilder.Append(carReferences[i].name);
            //  stringSubset = carReferences[i].name.Split(' ');
            //  namingCarStringBuilder.Append(stringSubset[0]);
            //  namingCarStringBuilder.Append(" " + i);
            //  carReferences[i].name = namingCarStringBuilder.ToString();
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        spawnInRadomNumber = Random.Range(1.0f, 6.0f);

        InvokeRepeating("SpawnCar", spawnInRadomNumber, ExperimentParameters.distanceBetweenCars);

        //if (int.Parse(this.gameObject.name.Split(' ')[1]) > 0 )   //making some distance between each car in different lanes
        //     InvokeRepeating("SpawnCar", 5, ExperimentParameters.distanceBetweenCars);
        //   else
        //    InvokeRepeating("SpawnCar", 1, ExperimentParameters.distanceBetweenCars);
    }

    float ChooseSpeedRandom(string rangeSpeedstr)
    {
        Debug.Log("range string = " + rangeSpeedstr);
        //extract the main two numbers of the car speed
        string[] mainNumbers = rangeSpeedstr.Split('-');

        //pass it to a random range function 

        float firstInRange = float.Parse(mainNumbers[0]);
        float secondInRange = float.Parse(mainNumbers[1]);
        float velocity_result = Random.Range(firstInRange, secondInRange);
        velocity_result -= 5;
        return velocity_result;
        //return the value to make it for each car 
    }
    //entering this method in a loop for spawning cars using setActive for effectiveness  
    void SpawnCar()  //spawning cars for a while the distance between cars is the main parameter in the generating process after making an object pool for the cars
    {
        Debug.Log("hitbycar in parent car class = " + hitByCar);
        if ((index < carReferences.Length) && !hitByCar)  //if the patient didn't hit a car
        {

            carReferences[index].SetActive(true);
            index++;
        }
        else
        {
            Debug.Log("car INDEX in parent class = " + index + "PARENT = " + this.gameObject.name );
            if (index > carReferences.Length - 1 )
            {
                Debug.Log("index now zero");
                index = 0; //exceed limits
            }
        }

        if (RoadController.fadeout_after_crossing)  //after fade make all cars disappear and do not enter the 1st condition and intialize hitByCar variable for a new record on the next road 
        {
            hitByCar = false;
        }
    }
    //making all cars disappear except the one that hit the player SetActive(true)
    public void StopAllCarsAfterAccident(int carIndex, GameObject carObject)
    {
        carController = GameObject.Find("CarController").GetComponent<CarController>();
        stringBuilder = new StringBuilder();
        stringBuilder.Append(carObject.name);
        string[] tokens = stringBuilder.ToString().Split(' ');

        if (ExperimentParameters.carType != "All")
        {
            switch (ExperimentParameters.carType)
            {
                case "Car":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[0].Pool;
                        Debug.Log("Got Car Pool");
                        break;
                    }
                case "Truck":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[1].Pool;

                        break;
                    }
                case "Bus":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[2].Pool;

                        break;
                    }
            }
            Debug.Log("Disable Cars in Road and Length Pool = " + carHandlerPool.Length);
        }
        else
        {
            //extract name for car Object Type Info

            switch (tokens[0])
            {
                case "Car(Clone)":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[0].Pool;

                        break;
                    }
                case "Truck(Clone)":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[1].Pool;

                        break;
                    }
                case "Bus(Clone)":
                    {
                        carHandlerPool = carController.carObjectHandlerArray[2].Pool;

                        break;
                    }

            }

        }

        int CarIndexInPool = int.Parse(tokens[1]); //the car that hit the player 
        int firstIndexInPoolSection = int.Parse(carObject.transform.parent.GetChild(0).ToString().Split(' ')[1]);
        Debug.Log("firstIndexInPoolSection " + firstIndexInPoolSection);
        int lastIndexInPoolSection = int.Parse(carObject.transform.parent.GetChild(carObject.transform.parent.childCount - 1).ToString().Split(' ')[1]);
        Debug.Log("lastIndexInPoolSection " + lastIndexInPoolSection);
        Debug.Log("carIndex " + carIndex);

        DisappearSpecificCars(CarIndexInPool, firstIndexInPoolSection, lastIndexInPoolSection, carHandlerPool);

        hitByCar = true;
    }

    private void DisappearSpecificCars(int carIndex, int firstIndexInPoolSection, int lastIndexInPoolSection, GameObject[] carHandlerPool)
    {
        for (int i = firstIndexInPoolSection; i < lastIndexInPoolSection; i++)  //TODO: specify even or odd car number to determine which car on which side
        {
            if (i != carIndex)
            {
                Debug.Log("car index in DisappearSpecificCars method " + carIndex);

                carHandlerPool[i].SetActive(false);

                // Debug.Log("Done car referencing set active false ");
            }
        }
    }
    public IEnumerator MakeCarsAppearAgain(int carTimeSpan, int carSiblingIndex, int firstIndexInPoolSection, int lastIndexInPoolSection)  //execute always after all cars instantiated on the Road
    {
        yield return new WaitForSeconds(10);
        for (int i = firstIndexInPoolSection; i < lastIndexInPoolSection; i++)
        {
            //a workaround for our object pooling system that if you didn't make setActive(true) to the pool then you can't control its appearance in the game just like the 2 lines suggest 
            carHandlerPool[i].SetActive(true);

            for (int j = 0; j < carReferences.Length; j++)  //reference every child to the array 
            {
                if (j != carSiblingIndex)
                {
                    carReferences[j].GetComponent<CarMove>().resetCarsPositions();
                    carReferences[j].SetActive(false);
                }

            }

            hitByCar = false;

        }

    }
}
