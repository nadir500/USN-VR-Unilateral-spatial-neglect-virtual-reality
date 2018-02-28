using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TouchPadNumberCollider : MonoBehaviour
{

    public TouchPadController touchPadController;

    void Intialize()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            // touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_On_Ana_02");
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_On_Ana_02");

            this.GetComponent<AudioSource>().PlayOneShot(audioClip);
            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(83, 83, 83, 255);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            touchPadController.putNumber(this.gameObject.name[0].ToString());
            this.transform.GetChild(0).GetComponent<Image>().color = new Color32(154, 154, 154, 255);
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_Off_Ana_01");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

            //touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_Off_Ana_01");
        }
    }
}
