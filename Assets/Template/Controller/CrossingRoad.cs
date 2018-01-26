using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour
{

    Rigidbody rb;
    GameObject parent;
    GameObject playerGB, carColliderGB;
    Vector3 playerPos, carColliderPos;
    bool carHorn = false;
    bool isHitByCar = false;
    float distance;
    string[] roadType;
    void OnTriggerEnter(Collider hitBox)
    {
        Debug.Log("Player Collided--->" + hitBox.name );
        //ifffff caaaaaaaaar 
        RoadController.fadeout_after_crossing = false;
        if (hitBox.tag.Equals(value: "Car"))
        {
            roadType = new string[4];
            parent = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 
            rb = parent.GetComponent<Rigidbody>();

            playerGB = this.gameObject; //we'll put it in an apropriate place in the hierarchy 
            carColliderGB = hitBox.gameObject;
            roadType = parent.GetComponent<CarMove>().roadType;
            rb.drag = 40;
            isHitByCar = true;
            //fade in dark red color as the car hits the player 
            GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
        }
        if (hitBox.gameObject.name.Equals(value: "midwalkYellowPoint"))
        {
            GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(2);  //fade entirely and wait for re-positioning 
            //putting audio sources congrats you reached the midwalk 
            //sending packets 

        }
        else
        if (hitBox.gameObject.name == "sidewalkYellowPoint")
        {
            GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(2);

            //putting audio source  you reached the end line 
        }
    }
    void Update()
    {
        if (Time.frameCount % 10 == 0) //excute every couple frames 
        {
            if (rb != null && isHitByCar)
            {
                parent.GetComponent<CarMove>().onBreak();
                playerPos = playerGB.transform.position;
                carColliderPos = carColliderGB.transform.position;
                distance = Vector3.Distance(playerPos, carColliderPos);

                Debug.Log("Distance between cars " + parent.gameObject.name + " " + Vector3.Distance(playerPos, carColliderPos));
                //calculating the distance between the collided car and the player 
                if (roadType[1].Equals(value: "Left") && distance >= 7.0f || roadType[1].Equals(value: "Right") && distance <= 8.9f)
                {
                    StopCar();
                    CarHornSound();
                }
            }
        }
    }
    void StopCar()
    {
        parent.GetComponent<CarMove>().StopSound();
        parent.GetComponent<CarMove>().onRemove();
        parent.GetComponent<AudioSource>().Play();
        rb.isKinematic = true;
    }
    void CarHornSound()
    {
        parent.GetComponent<CarMove>().CarHorn();
    }
}
