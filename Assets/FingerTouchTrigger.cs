using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTouchTrigger : MonoBehaviour
{

    //hold for amount of time 
    float timer = 4;
    int beepCounter = 0;
    bool isHoldingOnObject = false;
    GameObject ToyObject;
    AudioController audioController;
     string parentFinger ;
    void Start()
    {
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
    }
    //holding the touched object's info 
    void OnTriggerEnter(Collider hitToy)
    {

         parentFinger = this.transform.parent.transform.parent.gameObject.name;
        parentFinger = parentFinger[parentFinger.Length-1].ToString();

        if (hitToy.tag.Equals(value: "ObjectGrab")&& hitToy.GetComponent<TableObject>() != null &&  CheckGrabbedHandWithObject(parentFinger,hitToy.GetComponent<TableObject>().objectPosition))
        //do something 
        {
            Debug.Log("LeapFinger Crossed Enter");

            isHoldingOnObject = true;
            ToyObject = hitToy.gameObject;

            InvokeRepeating("PlayBeep", 0, 1);
        }
        else
        {
        audioController.playAudioClip("TableSounds/errorTyping",0,0.41f);
        }

    }

    void OnTriggerExit(Collider hitToy)
    {
        isHoldingOnObject = false;
        Debug.Log("LeapFinger Crossed Exit");
        CancelInvoke("PlayBeep");
        beepCounter = 0;
    }
    void PlayBeep()
    {
        audioController.playAudioClip("TableSounds/Beep", 0.0f,-1);
        beepCounter++;
        if (beepCounter == 3)
        {
            if (ToyObject != null)
            {
                StartCoroutine(removeFromTable());
            }
            // timer = 4;
            //   isHoldingOnObject = false;
            CancelInvoke("PlayBeep");
        }

    }

    private IEnumerator removeFromTable()
    {
        ToyObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.27f);
        audioController.playAudioClip("TableSounds/Add", 0, -1);
        yield return new WaitForSeconds(0.8f);
        TableObject tempTableObject = ToyObject.GetComponent<TableObject>();
        tempTableObject.finishedRecord = true;
        
        tempTableObject.SetHandHoldObject(true, parentFinger);

        ToyObject.transform.GetChild(0).gameObject.SetActive(false);
        ToyObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    private bool CheckGrabbedHandWithObject(string fingerSide,string objectSide)
    {
        if (fingerSide.Equals(objectSide.ToUpper()[0].ToString()))
        {
            Debug.Log("Hand And Object Suceeded ");
            return false;
        }
        else
        {
            Debug.Log("Hand And Object NOT Suceeded!! ");
            return true;
        }
    }
}
