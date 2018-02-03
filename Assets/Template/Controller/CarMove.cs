using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float speed;
    public AudioClip enginSound;
    public AudioClip brakeSound;
    public AudioClip carHorn;
    public AudioClip crash;
    public string currentClip = "";
    public string carDirection;
    private Vector3 _start_car_position;




    string[] streetsDirections = ExperementParameters.streetsDirections.Split(' ');
    AudioSource carEngineAudio;
    AudioSource carCrashSound;
    AudioSource carBrakeSound;

    void Start()
    {
        enginSound = Resources.Load("Audio/CarEngine") as AudioClip;
        brakeSound = Resources.Load("Audio/tires_squal_loop") as AudioClip;
        carHorn = Resources.Load("Audio/Horn") as AudioClip;
        crash = Resources.Load("Audio/Impact") as AudioClip;
        //this.GetComponent<AudioSource>().PlayOneShot(enginSound);
        speed = ExperementParameters.carsSpeed; //from UI
        _start_car_position = this.transform.position; //taking the prev position 

        carEngineAudio = this.GetComponent<AudioSource>();

        carCrashSound = this.transform.GetChild(2).GetComponent<AudioSource>();

        carBrakeSound = this.transform.GetChild(1).GetComponent<AudioSource>();

        switch (ExperementParameters.soundDirections)
        {
            case "Off":
                {
                    carEngineAudio.volume = 0;
                    carCrashSound.volume = 0;
                    carBrakeSound.volume = 0;

                    break;
                }
            case "Left":
                {
                    carEngineAudio.panStereo = -1;
                    carCrashSound.panStereo = -1;
                    carBrakeSound.panStereo = -1;
                    break;
                }
            case "Right":
                {
                    carEngineAudio.panStereo = 1;
                    carCrashSound.panStereo = 1;
                    carBrakeSound.panStereo = 1;
                    break;
                }
            case "Both":
                {
                    carEngineAudio.volume = 1;
                    carCrashSound.volume = 1;
                    carBrakeSound.volume = 1;
                    carEngineAudio.panStereo = 0;
                    carCrashSound.panStereo = 0;
                    carBrakeSound.panStereo = 0;
                    break;
                }
           
        }
    }

    void Update()
    {
        var rb = this.GetComponent<Rigidbody>();
        //i need to know which road is this is it from left to right road? and vice versa 
        if (carDirection.Equals(value: "Left"))// && RoadController.fadeout_after_crossing == true)
        {

            rb.velocity = -Vector3.forward * speed;

        }
        else
        {
            if (carDirection.Equals(value: "Right"))// && RoadController.fadeout_after_crossing == true)
            {
                rb.velocity = Vector3.forward * speed;
                //rb.isKinematic=false;
            }
        }
        if (!RoadController.fadeout_after_crossing)
        {
            FadeSound();
            if (carEngineAudio.volume == 0)
            {
                this.transform.position = _start_car_position;
                rb.velocity = Vector3.zero;
            }
        }
        //if exceeds the streÄet's limit configure the position again
        if (Mathf.Round(this.transform.position.z) <= -266.0f || Mathf.Round(this.transform.position.z) >= 200)  // going and back street
        {
            this.transform.position = _start_car_position;

        }
        if (this.transform.position == _start_car_position) //intializing after the car triggered the player 
        {
            rb.isKinematic = false;
            rb.drag = 1;
        }


    }
    public void CrashSound()
    {
        carCrashSound.PlayOneShot(crash);
    }
    public void onBrake()
    {
        carBrakeSound.Play();
    }
    public void RemoveBrakeSound()
    {
        carBrakeSound.Stop();
    }
    public void FadeSound()
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
        carEngineAudio.PlayOneShot(carHorn); //playing beep beep with it besides engine 

    }
}
