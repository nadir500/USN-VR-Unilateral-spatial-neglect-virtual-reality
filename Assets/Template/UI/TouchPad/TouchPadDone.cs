using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPadDone : MonoBehaviour
{

    public TouchPadController touchPadController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            //  touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_On_Ana_02");
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_On_Ana_02");

            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(154, 113, 8, 255);
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            touchPadController.Done();
            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(206, 151, 8, 255);
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_Off_Ana_01");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

            //touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_Off_Ana_01");

        }
    }
}