using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPadClear : MonoBehaviour
{

    public TouchPadController touchPadController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            //	touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_On_Ana_02");
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_On_Ana_02");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(163, 111, 0, 63);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            touchPadController.Clear();
            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 174, 0, 63);
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_Off_Ana_01");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

            //touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_Off_Ana_01");
        }
    }
}
