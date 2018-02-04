﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour
{

    Rigidbody rb;
    GameObject parentCar;
    GameObject playerGB, carColliderGB;
    Vector3 playerPos, carColliderPos;
    string carDirection;
    bool stopCar;
    LayerMask uiMask = (1 << 5);
    public delegate void HitByCar();
    public HitByCar WhenHitByCar;
    private float Timeleft;


    void Start()
    {
        Intialize();
    }
    void Intialize()
    {
    }
    void OnTriggerEnter(Collider hitBox)
    {
        if (hitBox.tag.Equals(value: "Car"))
        {
            Timeleft = 0.5f;
            parentCar = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 
            rb = parentCar.GetComponent<Rigidbody>();
            carDirection = parentCar.gameObject.GetComponent<CarMove>().carDirection;
            stopCar = parentCar.gameObject.GetComponent<CarMove>().hasToStop;
            playerGB = this.gameObject; //we'll put it in an apropriate place in the hierarchy 
            carColliderGB = hitBox.gameObject;
            rb.drag = 40;
            WhenHitByCar();
            parentCar.GetComponent<CarMove>().onBrake();
        }
    }

    void Update()
    {
        if (rb != null && RoadController.fadeout_after_crossing == false)
        {
            Timeleft -= Time.deltaTime;
            if (Timeleft < 0.0f)
            {

                StopCar();
                parentCar.GetComponent<CarMove>().RemoveBrakeSound();
                CrashSound();
                Camera.main.GetComponent<CameraShake>().shakeDuration = 1 ;
                rb = null;
            }
        }
        /* if (Time.frameCount % 7 == 0) //excute every couple frames 
         {
             if (rb != null && RoadController.fadeout_after_crossing == true)
             {
                 if (!rb.isKinematic)
                 {
                     parent.GetComponent<CarMove>().onBrake();
                 }
                 else
                 {

                     parent.GetComponent<CarMove>().RemoveBrakeSound();
                     if(isHitByCar)
                      {
                     parent.GetComponent<CarMove>().CrashSound();
                     //shake camera 
                     Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 1;
                     isHitByCar=false;

                      }

                 }

                 playerPos = playerGB.transform.position;
                 carColliderPos = carColliderGB.transform.position;

                float distance = Vector3.Distance(playerPos, carColliderPos);

                 //Debug.Log("Distance between cars " + parent.gameObject.name + " " + Vector3.Distance(playerPos, carColliderPos));

                 //calculating the distance between the collided car and 2the player 
                 if (carDirection.Equals(value: "Left") && distance <= 2.3f || carDirection.Equals(value: "Right") && distance <= 2.3f)
                 {
                     StopCar();

                     if (Time.frameCount % 45 == 0)
                     {
                         CarHornSound();
                     }

                 }
             }
         }*/
    }
    IEnumerator BrakeCarSound()
    {
        yield return new WaitForSeconds(1.5f);
        parentCar.GetComponent<CarMove>().onBrake();
    }

    void StopCar()
    {
        rb.isKinematic = true;
    }
    void CrashSound()
    {
        parentCar.GetComponent<CarMove>().CrashSound();

    }
    void CarHornSound()
    {
        parentCar.GetComponent<CarMove>().CarHorn();
    }
}