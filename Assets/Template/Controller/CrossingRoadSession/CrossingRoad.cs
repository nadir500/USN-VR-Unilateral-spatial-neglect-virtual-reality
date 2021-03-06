﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour
{
    //this class aims to set the behavior when a car hit the player 

    public delegate void HitByCar();  //a delegate linked to CheckPointController to see if i hit a car or not 
    public HitByCar WhenHitByCar;
    public CharacterMechanism characterMechanism;
    public GameClient gameClientController;
    public Fading fadeController;
    public ExperimentObserves experimentObserves;
    private float timeleft;  //time left to stop the car
    private CarParentOnRoad carParentOnRoadController;  //the parent of the car that hit the player and we will use it in the trigger function above
    private Rigidbody rb;  //getting the car rigid body when trigger enter
    private GameObject parentCar; //getting the parent of the car when trigger enter 
                                  // private GameObject playerGB, carColliderGB; //getting the player collider and the car collider when trigger enter 
    //private CarMove carMoveController;
//    private string carDirection;
   // private bool stopCar;
    private bool isHit_ContinueChoice = false;
    //TODO: REFACTOR THIS CODE 
    void OnTriggerEnter(Collider hitBox)
    {
        if (hitBox.gameObject.name == "1" || hitBox.gameObject.name == "2")
        {
            //summon function from experiments observe 
            
        }
         
          experimentObserves.PassingAndProcessingDistanceValues(hitBox.gameObject);
         

        if (hitBox.gameObject.tag == "Midwalk")  //entering midwalk then stop automatically to be ready for the next round 
        {

            Debug.Log("Hit Midwalk");
            characterMechanism.SetCurrentPositionForX_Axis(hitBox.transform.position.x);

            //make him stop if he's moving 
            if (isHit_ContinueChoice)
            {
                gameClientController.result = "stop";

                hitBox.enabled = false;
                fadeController.fadeDirection = -1;

                isHit_ContinueChoice = false;
            }

        }
        if (hitBox.tag.Equals(value: "Car") && !RoadController.fadeout_after_crossing)
        {
            Debug.Log("FADE STATE = " + RoadController.fadeout_after_crossing);
            //time before car stop
            timeleft = 0.5f;
            parentCar = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 

            rb = parentCar.GetComponent<Rigidbody>(); //car rigidbody 
           // carMoveController = parentCar.gameObject.GetComponent<CarMove>();  //getting the script 
//            carDirection = carMoveController.carDirection; //the direction of the car 
           // stopCar = carMoveController.hasToStop;  //the car needs to stop bool
            rb.drag = 47; //slowing down the car 
            WhenHitByCar();
            parentCar.GetComponent<CarMove>().onBrake(); //make brake sound 
            //getting the parent and make all cars dissappear except the one is collided with the player
            carParentOnRoadController = parentCar.transform.parent.GetComponent<CarParentOnRoad>();
            //getting the index of the car object collided by the player and not making it disappear with the others

            Debug.Log("Sibling " + parentCar.transform.GetSiblingIndex());
            carParentOnRoadController.StopAllCarsAfterAccident(parentCar.transform.GetSiblingIndex(), parentCar);

            if (ExperimentParameters.afterAccidentEvent == "Reset")//has to stop and repeat the experiment 
            {
                gameClientController.result = "stop";

                //Doctor's Sound telling the player that he has failed
                StartCoroutine(ResetTheGame());
                StartCoroutine(StopCarForWhile(rb));
                StartCoroutine(carParentOnRoadController.MakeCarsAppearAgain(ExperimentParameters.distanceBetweenCars, parentCar.transform.GetSiblingIndex(),
                                     int.Parse(parentCar.transform.parent.GetChild(0).ToString().Split(' ')[1]),
                                         int.Parse(parentCar.transform.parent.GetChild(parentCar.transform.parent.childCount - 1).ToString().Split(' ')[1])));
            }
            else
            {
                gameClientController.result = "stop";

                StartCoroutine(carParentOnRoadController.MakeCarsAppearAgain(ExperimentParameters.distanceBetweenCars, parentCar.transform.GetSiblingIndex(),
                                    int.Parse(parentCar.transform.parent.GetChild(0).ToString().Split(' ')[1]),
                                        int.Parse(parentCar.transform.parent.GetChild(parentCar.transform.parent.childCount - 1).ToString().Split(' ')[1])));
                StartCoroutine(StopCarForWhile(rb));

                isHit_ContinueChoice = true;

                StartCoroutine(GoCommandOnAccident());
                Debug.Log("GO in Else of hitting a car");
            }

        }

    }

    private IEnumerator StopCarForWhile(Rigidbody carRB)
    {
        yield return new WaitForSeconds(8f);
        carRB.isKinematic = false;
        carRB.drag = 0;

    }

    void Update()
    {
        if (rb != null && RoadController.fadeout_after_crossing == false)
        {
            timeleft -= Time.deltaTime;
            if (timeleft < 0.0f)
            {

                StopCar();
                parentCar.GetComponent<CarMove>().RemoveBrakeSound();
                // CrashSound();  //here to add additional sound effects when hit the car 
                CarHornSound();
                rb = null;
            }
        }
    }
    IEnumerator BrakeCarSound()
    {
        yield return new WaitForSeconds(1.5f);
        parentCar.GetComponent<CarMove>().onBrake();
    }
    //stop the car from moving 
    void StopCar()
    {
        rb.isKinematic = true;
    }
    //horn sound played 
    void CarHornSound()
    {
        parentCar.GetComponent<CarMove>().CarHorn();
    }

    IEnumerator ResetTheGame()
    {
        yield return new WaitForSeconds(5f);
        characterMechanism.ResetPositionToStartPoint();
        fadeController.fadeDirection = -1;


    }

    IEnumerator GoCommandOnAccident()
    {
        yield return new WaitForSeconds(5);
        gameClientController.result = "go";

    }

}
