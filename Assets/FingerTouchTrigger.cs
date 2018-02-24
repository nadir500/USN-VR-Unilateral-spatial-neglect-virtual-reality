using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTouchTrigger : MonoBehaviour
{

    //hold for amount of time 
    float timer = 4;
    bool isHoldingOnObject = false;
    GameObject ToyObject;

    //holding the touched object's info 
    void OnTriggerEnter(Collider hitToy)
    {
        if (hitToy.tag.Equals(value: "ObjectGrab"))
        //do something 
        {
            Debug.Log("LeapFinger Crossed Enter");

            isHoldingOnObject = true;
            ToyObject = hitToy.gameObject;
        }

    }

    void OnTriggerExit(Collider hitToy)
    {
        isHoldingOnObject = false;
        Debug.Log("LeapFinger Crossed Exit");

    }

    void Update()
    {


        if (isHoldingOnObject && timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else
        //reset timer 
        {


            //destroy 
            if (ToyObject != null)
            {
               TableObject tempTableObject= ToyObject.GetComponent<TableObject>();
               tempTableObject.finishedRecord = true;
                string parentFinger = this.transform.parent.transform.parent.gameObject.name;
                tempTableObject.SetHandHoldObject(true,parentFinger);
                
                ToyObject.transform.GetChild(0).gameObject.SetActive(false);
                ToyObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            timer = 4;
            isHoldingOnObject = false;
        }



    }
    void PlayBeep()
    {
        
    }
}
