using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour
{

    Rigidbody rb;
    GameObject parent;
    GameObject playerGB, carColliderGB;
    Fading fadeObject;
    GameObject midwalkYellowPoint, sidewalkYellowPoint;
    Vector3 playerPos, carColliderPos;
    bool carHorn = false;
    bool isHitByCar = true;
    float distance;
    string carDirection;
    int isHitYellowball = 0;
    LayerMask uiMask = (1 << 5);
    void Start()
    {
        fadeObject = GameObject.Find("FadeController").GetComponent<Fading>();

    }
    void OnTriggerEnter(Collider hitBox)
    {
        //Debug.Log("Player Collided--->" + hitBox.name);
        //ifffff caaaaaaaaar 
        if (hitBox.tag.Equals(value: "Car"))
        {
            parent = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 
            rb = parent.GetComponent<Rigidbody>();
            carDirection  = hitBox.gameObject.GetComponent<CarMove>().carDirection;
            playerGB = this.gameObject; //we'll put it in an apropriate place in the hierarchy 
            carColliderGB = hitBox.gameObject;
            rb.drag = 40;

            //fade in dark red color as the car hits the player 
            fadeObject.BeginFade(1);
        }
    }

    IEnumerator UpdateCheckPoint()
    {
        midwalkYellowPoint.SetActive(false);
        yield return new WaitForSeconds(6);
        sidewalkYellowPoint.SetActive(true);
        
    }
    void Update()
    {

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

                distance = Vector3.Distance(playerPos, carColliderPos);

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
    void StopCar()
    {
        rb.isKinematic = true;
    }
    void CarHornSound()
    {
        parent.GetComponent<CarMove>().CarHorn();
    }
}
