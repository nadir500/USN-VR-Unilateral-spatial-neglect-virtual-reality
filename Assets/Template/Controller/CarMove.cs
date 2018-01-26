using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float speed;
    public AudioClip enginSound;
    public AudioClip brakeSound;
    public AudioClip carHorn;
    public string[] roadType = new string[4];
    private Vector3 _start_car_position;

    string[] streetsDirections = ExperementParameters.streetsDirections.Split(' ');
    AudioSource carAudio;

    void Start()
    {
        enginSound = Resources.Load("Audio/CarEngine") as AudioClip;
        brakeSound = Resources.Load("Audio/tires_squal_loop") as AudioClip;
        carHorn = Resources.Load("Audio/Horn") as AudioClip;
        this.GetComponent<AudioSource>().PlayOneShot(enginSound);
        speed = ExperementParameters.carsSpeed; //from UI
        _start_car_position = this.transform.position; //taking the prev position 

        // slowMoTimeScale = Time.timeScale/factor;    //calculate the new timescale

        roadType = this.transform.parent.gameObject.name.Split(' ');
        carAudio = this.GetComponent<AudioSource>();

        // here

    }

    void Update()
    {
        var rb = this.GetComponent<Rigidbody>();
        //i need to know which road is this is it from left to right road? and vice versa 
        if (roadType[1].Equals(value: "Left"))// && RoadController.fadeout_after_crossing == true)
        {

            rb.velocity = -Vector3.forward * speed;

        }
        else
        {
            if (roadType[1].Equals(value: "Right"))// && RoadController.fadeout_after_crossing == true)
            {
                rb.velocity = Vector3.forward * speed;
                //rb.isKinematic=false;
            }
        }
        if (!RoadController.fadeout_after_crossing)
        {
            FadeSound();
            if (carAudio.volume == 0)
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

    public void onBrake()
    {
        this.GetComponent<AudioSource>().PlayOneShot(brakeSound);
    }
    public void onRemove()
    {
        this.GetComponent<AudioSource>().PlayOneShot(enginSound);
    }

    public void FadeSound()
    {
        if (carAudio.volume != 0)   //audio threshold 
            carAudio.volume -= 0.4f * Time.deltaTime;
        //this.GetComponent<AudioSource>().Stop();

    }

    public void CarHorn()
    {
        this.GetComponent<AudioSource>().PlayOneShot(carHorn);

    }
}
