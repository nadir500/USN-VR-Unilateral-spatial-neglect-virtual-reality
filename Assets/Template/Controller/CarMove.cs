using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour {
    public float speed ;
    public AudioClip enginSound;
    public AudioClip brakeSound;
    public AudioClip carHorn;
    private string[] roadType = new string[4];
    private Vector3 _start_car_position;
   
    string[] streetsDirections = ExperementParameters.streetsDirections.Split(' ');
    

    void Start () {
         enginSound = Resources.Load("Audio/CarEngine") as AudioClip;
        brakeSound = Resources.Load("Audio/tires_squal_loop") as AudioClip;
        carHorn = Resources.Load("Audio/Horn") as AudioClip;        
        this.GetComponent<AudioSource>().PlayOneShot(enginSound);
         speed = ExperementParameters.carsSpeed; //from UI
		_start_car_position = this.transform.position; //taking the prev position 
        
       // slowMoTimeScale = Time.timeScale/factor;    //calculate the new timescale
		
        roadType= this.transform.parent.gameObject.name.Split(' ');
        // here

    }
	
 	void Update()
    {
    var rb = this.GetComponent<Rigidbody>();
        //i need to know which road is this is it from left to right road? and vice versa 
        if (roadType[1].Equals(value: "Left"))
        {
            rb.velocity = -Vector3.forward * speed;
           // this.transform.position -= Vector3.forward * Time.deltaTime * speed;
        }
        else
        {
            if (roadType[1].Equals(value: "Right"))
            rb.velocity = Vector3.forward * speed;

               // this.transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        //if exceeds the street's limit configure the position again



        if (Mathf.Round(this.transform.position.z) <= -266.0f || Mathf.Round(this.transform.position.z) >= 200)  // going and back street
        {
            this.transform.position = _start_car_position;

        }

    }

   public void onBreak()
    {
        this.GetComponent<AudioSource>().PlayOneShot(brakeSound);
        //this.GetComponent<AudioSource>().clip = brakeSound;
        //this.GetComponent<AudioSource>().Play();
      //  this.GetComponents<AudioSource>()[0].enabled = false;
       // this.GetComponents<AudioSource>()[1].enabled = true;
    }
   public void onRemove()
    {
        this.GetComponent<AudioSource>().PlayOneShot(enginSound);
        ////this.GetComponent<AudioSource>().clip = enginSound;
        //this.GetComponent<AudioSource>().Play();
    }

    public void StopSound()
    {
        this.GetComponent<AudioSource>().Stop();
    }

    public void CarHorn()
    {
        this.GetComponent<AudioSource>().PlayOneShot(carHorn);

    }
 }
