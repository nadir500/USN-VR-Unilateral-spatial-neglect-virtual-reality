using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    [HideInInspector]
	public List<GameObject> carRefernces;

	void Start () {

		IntializeCars();
	}


	void IntializeCars()
	{
		carRefernces= new List<GameObject>();
	}

	 /*we need to instantiate the cars in the scene with the perfect positions on the road when generating it */
    public void InstantiateCarsFastRoad( 
				Transform roadParent  //the road gameObject that is generated
               	     , GameObjectHandler carObjectHandler) //the handler from object pooling class (GameObject Handler)
    {
		 string[] carDirection=roadParent.gameObject.name.Split(' '); //knowing which rotation and direction to instatiate the car
		
        //here everytime i am taking the gameObject.name of the road and spliting it then taking the index [1] to know which direction this road is    
        for (int i = 0; i < 2; i++) //2 cars each road
        {
            //now i am seperating between going cars which is the cars from left to right direction
             //and back cars which is from right to left direction
            if (carDirection[1].Equals(value: "Left"))  //from left to right 
            {
                //now instantiate the cars with the positions explained above 
                GameObject car = carObjectHandler.RetrieveInstance(
                    	new Vector3(0.3f/*way from the edge of the corner*/+ roadParent.position.x + 2.5f * i, roadParent.position.y, roadParent.position.z+ ExperementParameters.distanceBetweenCars * i + 195.0f ), //putting the position with the distance between each car
                                            Quaternion.Euler(new Vector3(0, -90, 0))); //the rotation of course 
                
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0)); //this is temporary 
			    car.AddComponent<CarMove>();  //adding the car movement component  
				car.GetComponent<CarMove>().carDirection = "Left";      //descripe which direction 
                carRefernces.Add(car);        //referncing it to a list 
            }

            if (carDirection[1].Equals(value: "Right")) //from right to left 
            {
                //now instantiate the cars with the positions explained above 
                GameObject car = carObjectHandler.RetrieveInstance(
                    new Vector3( roadParent.position.x - 0.3f - 2.5f * i, roadParent.position.y, roadParent.position.z -195.0f - ExperementParameters.distanceBetweenCars * i ),//putting the position with the distance between each car
                                        Quaternion.Euler(new Vector3(0, 90, 0)));   //the rotation of course
                
                car.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));//this is temporary
                car.AddComponent<CarMove>(); //adding the car moce component 
				car.GetComponent<CarMove>().carDirection = "Right";  //descripe which direction 
                carRefernces.Add(car); //referncing it to a list 
            }
        }
    }
}
