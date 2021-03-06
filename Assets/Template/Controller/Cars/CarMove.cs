﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    //the main script which is attached to every car Instantiated 
    //we can move the car in the direction specified by this script  
    public static int numberOfRenderedCars = 0;      //??? need abdalla

    public float speed = 0;  //speed of the car 
    public string carDirection;  //knowing which direction is the car 
    public bool hasToStop = false;
    private Vector3 _start_car_position;  //we will use it when we fade the screen and colliding with the player 
    private CarParentOnRoad carParentOnRoad;
    AudioSource carEngineAudio;  //audio source reference to Engine Audio Source from Car Prefab
    AudioSource carCrashSound;  //audio source reference to Crash Audio Source from Car Prefab (deleted in the latest feedback)
    AudioSource carBrakeSound;  //audio source reference to Brake Audio Source from Car Prefab
    AudioSource carHornSound;  //audio source reference to Car Horn Audio Source from Car Prefab
    bool isRendered = false; // ?? Abdalla
    bool lastIsRenderedState = false; // ?? Abdalla

    private Renderer renderer;  //??? Abdalla


    void Start()
    {
        renderer = GetComponent<Renderer>();
        carParentOnRoad = this.transform.parent.GetComponent<CarParentOnRoad>();
        InvokeRepeating("GetSpeedCarFromParent", 0, 0.001f);

        _start_car_position = this.transform.position; //taking the prev position 
        //getting audio source references from the each Car Prefab after instantiating it from Car Controller
        carEngineAudio = this.GetComponent<AudioSource>();
        carBrakeSound = this.transform.GetChild(1).GetComponent<AudioSource>();
        carCrashSound = this.transform.GetChild(2).GetComponent<AudioSource>();
        carHornSound = this.transform.GetChild(3).GetComponent<AudioSource>();
        InitializeCarSounds(); //initialize the state of the sounds from  Experiment Parameters by soundDirections variable

    }
    void GetSpeedCarFromParent()
    {
        if (carParentOnRoad != null)
            speed = carParentOnRoad.carSpeed; //Getting the speed value from static variables in ExperimentParameters Class which is assigned from the UI and Player Prefs 
        if(speed !=0)
            CancelInvoke("GetSpeedCarFromParent");
    }
     
    private void InitializeCarSounds()
    {
        switch (ExperimentParameters.soundDirections)
        {
            case "Off":
                {
                    carEngineAudio.volume = 0;
                    carCrashSound.volume = 0;
                    carBrakeSound.volume = 0;
                    carHornSound.volume = 0;
                    break;
                }
            case "Left":
                {
                    carBrakeSound.volume = 0.7f;
                    //for left side hearing 
                    carEngineAudio.panStereo = -1;
                    carCrashSound.panStereo = -1;
                    carBrakeSound.panStereo = -1;
                    carHornSound.panStereo = -1;
                    break;
                }
            case "Right":
                {

                    carBrakeSound.volume = 0.7f;
                    //for right side hearing 
                    carEngineAudio.panStereo = 1;
                    carCrashSound.panStereo = 1;
                    carBrakeSound.panStereo = 1;
                    carHornSound.panStereo = 1;
                    break;
                }
            case "Both":
                {
                    carEngineAudio.volume = 1;
                    carCrashSound.volume = 1;
                    carBrakeSound.volume = 0.7f;
                    carHornSound.volume = 1;
                    //both hearing
                    carEngineAudio.panStereo = 0;
                    carCrashSound.panStereo = 0;
                    carBrakeSound.panStereo = 0;
                    carHornSound.panStereo = 0;
                    break;
                }

        }
    }

    void Update()
    {
        var rb = this.GetComponent<Rigidbody>();
        //i need to know which road is this is it from left to right road? and vice versa 
        if (carDirection.Equals(value: "Left"))
        {
            rb.velocity = -Vector3.forward * speed;
        }
        else
        {
            if (carDirection.Equals(value: "Right"))
            {
                rb.velocity = Vector3.forward * speed;
            }
        }
        if (RoadController.fadeout_after_crossing)  //if the screen faded black and the loading icon appeared 
        {
            FadeSound(); //fade sound smoothly 
            Debug.Log("Road Static Variable " + RoadController.fadeout_after_crossing);
            if (carEngineAudio.volume == 0)
            {
                this.transform.position = _start_car_position;  //repositioning after fading to prepare for generating again on the road 
                                                                //make all the cars dissappear after repositioning 
                this.gameObject.SetActive(false);
            }
        }

        //if exceeds the street's limit configure the position again
        if (carDirection.Equals(value: "Left") && Mathf.Round(this.transform.position.z) <= -266.0f || carDirection.Equals(value: "Right") && Mathf.Round(this.transform.position.z) >= 200)  // going and back street
        {
            this.transform.position = _start_car_position;
            this.gameObject.SetActive(false);
        }

        if (this.transform.position == _start_car_position) //initializing after the car triggered the player or after the screen faded out 
        {
            //when we hit a car the is kinematic will be true thus we need to initialize the car values after the screen faded out or when colliding with the player 
            rb.isKinematic = false;
            rb.drag = 1;
        }

        isRendered = renderer.isVisible;
        if (isRendered != lastIsRenderedState)
        {
            numberOfRenderedCars += (isRendered) ? +1 : -1;
            lastIsRenderedState = isRendered;
        }

    }
    /****************************************************Audio Events For the Car*******************************************************/
    public void onBrake() //when colliding with a car
    {
        carBrakeSound.Play();
    }
    public void RemoveBrakeSound()  //to stop the brake sound 
    {
        carBrakeSound.Stop();
    }
    public void FadeSound()  //fading the cars sound after fade (i think it's now deprecated after our last edit in requirements on the project)
    {
        if (carEngineAudio.volume != 0)   //audio threshold 
        {
            carEngineAudio.volume -= 0.6f * Time.deltaTime;
            carBrakeSound.Stop();
            carCrashSound.Stop();
        }

    }
    public void CarHorn()
    {
        carHornSound.Play();
    }

    public void resetCarsPositions()
    {
        Debug.Log("Reset Cars Positions");
        this.transform.position = _start_car_position;
    }
    /****************************************************END*******************************************************/

}
