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
    string[] roadType;
    int isHitYellowball = 0;
    LayerMask uiMask = (1 << 5);


    //public void initilization()
    //{
    //    audioSource = this.GetComponent<AudioSource>();
    //    StartCoroutine(playSound("Go"));

    //}
    void Start()
    {
        fadeObject = GameObject.Find("FadeGameObject").GetComponent<Fading>();

    }
    void OnTriggerEnter(Collider hitBox)
    {
        Debug.Log("Player Collided--->" + hitBox.name);
        //ifffff caaaaaaaaar 
        if (hitBox.tag.Equals(value: "Car"))
        {
            roadType = new string[4];
            parent = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 
            rb = parent.GetComponent<Rigidbody>();

            playerGB = this.gameObject; //we'll put it in an apropriate place in the hierarchy 
            carColliderGB = hitBox.gameObject;
            roadType = parent.GetComponent<CarMove>().roadType;
            rb.drag = 40;

            //fade in dark red color as the car hits the player 
            fadeObject.BeginFade(1);
        }
        if (hitBox.tag.Equals(value: "CheckPoint") && isHitYellowball == 0)
        {
            StartCoroutine(fadeObject.playSound("Stop"));

            RoadController.fadeout_after_crossing = false;

            midwalkYellowPoint = GameObject.Find("RoadController").GetComponent<RoadController>().midWalkYellowPoint;
            sidewalkYellowPoint = GameObject.Find("RoadController").GetComponent<RoadController>().sideWalkYellowPoint;

            //making the character position with the yellow point midwalk position 

            StartCoroutine(UpdateCheckPoint());


            GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(2);  //fade entirely and wait for re-positioning 
            Transform KVR = GameObject.Find("OnlineBodyView").transform;
            Camera.main.cullingMask = uiMask;
            KVR.localPosition = new Vector3(KVR.localPosition.x - 6.39f, KVR.localPosition.y, KVR.localPosition.z);
            isHitYellowball = 1;

            //putting audio sources congrats you reached the midwalk 

        }
        else
        if (hitBox.tag.Equals(value: "CheckPoint") && isHitYellowball == 1)
        {
            StartCoroutine(fadeObject.playSound("Stop"));

            RoadController.fadeout_after_crossing = false;

            sidewalkYellowPoint.SetActive(false);
            GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(2);

            //making the position of the player with the sidewalk position 

            Transform KVR = GameObject.Find("OnlineBodyView").transform;
            KVR.localPosition = new Vector3(KVR.localPosition.x - 6.39f, KVR.localPosition.y, KVR.localPosition.z);
            isHitYellowball = 2;
            //putting audio source  you reached the end line 
        }
    }

    IEnumerator UpdateCheckPoint()
    {
        midwalkYellowPoint.SetActive(false);
        yield return new WaitForSeconds(3);
        sidewalkYellowPoint.SetActive(true);
        
    }
    void Update()
    {

        if (Time.frameCount % 7 == 0) //excute every couple frames 
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

                Debug.Log("Distance between cars " + parent.gameObject.name + " " + Vector3.Distance(playerPos, carColliderPos));

                //calculating the distance between the collided car and the player 
                if (roadType[1].Equals(value: "Left") && distance <= 2.2f || roadType[1].Equals(value: "Right") && distance <= 2.2f)
                {
                    StopCar();

                    if (Time.frameCount % 45 == 0)
                    {
                        CarHornSound();
                    }

                }
            }
        }
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
