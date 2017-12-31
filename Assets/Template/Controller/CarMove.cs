﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour {
 public float speed ;
private string[] roadType = new string[4];
private Vector3 _start_car_position;

 	void Start () {
         speed = ExperementParameters.carsSpeed; //from UI
		_start_car_position = this.transform.position; //taking the prev position 

		roadType= this.transform.parent.gameObject.name.Split(' ');
    }
	
 	void Update()
    {
        //i need to know which road is this is it from left to right road? and vice versa 
        if (roadType[1] == "Side_Go")
        {
            this.transform.position -= Vector3.forward * Time.deltaTime * speed; 
        }
        else
        {
            if (roadType[1] == "Side_Back")
                this.transform.position += Vector3.forward * Time.deltaTime * speed;  
        }
        //if exceeds the street's limit configure the position again  
        if (Mathf.Round(this.transform.position.z) <= -266.0f || Mathf.Round(this.transform.position.z) >= 200)  // going and back street
        {
            this.transform.position = _start_car_position;
        }

    }
}
