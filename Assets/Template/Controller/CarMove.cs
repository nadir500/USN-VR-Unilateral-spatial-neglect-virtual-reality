using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour {
    public float speed ;
   
    private string[] roadType = new string[4];
    private Vector3 _start_car_position;
    private float slowMoTimeScale;  //slow motion time scale
    private float factor = 13.0f;      //factor to increase or decrease the timescale by
    string[] streetsDirections = ExperementParameters.streetsDirections.Split(' ');

    void Start () {
         speed = ExperementParameters.carsSpeed; //from UI
		_start_car_position = this.transform.position; //taking the prev position 
        
        slowMoTimeScale = Time.timeScale/factor;    //calculate the new timescale
		
        roadType= this.transform.parent.gameObject.name.Split(' ');
    }
	
 	void Update()
    {
        //i need to know which road is this is it from left to right road? and vice versa 
        if (roadType[1].Equals(value:"Left"))
        {
            this.transform.position -= Vector3.forward * Time.deltaTime * speed; 
        }
        else
        {
            if (roadType[1].Equals(value:"Right"))
                this.transform.position += Vector3.forward * Time.deltaTime * speed;  
        }
        //if exceeds the street's limit configure the position again  
        if (Mathf.Round(this.transform.position.z) <= -266.0f || Mathf.Round(this.transform.position.z) >= 200)  // going and back street
        {
            this.transform.position = _start_car_position;
        
        }

    }
    void OnTriggerEnter(Collider playerCollider)
    {
        //assign new time scale value
        Debug.Log("Car Triggered");

        Time.timeScale = slowMoTimeScale;
        //reduce this to the same proportion as timescale to ensure smooth simulation
        Time.fixedDeltaTime = Time.fixedDeltaTime*Time.timeScale; 

        //after that in like 1 minute after the scale put some text saying you're about to hit a car 
        new WaitForSeconds(20);
        float fadeTime = GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
    }
 }
