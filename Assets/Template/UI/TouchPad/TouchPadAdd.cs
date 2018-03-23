using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadAdd : MonoBehaviour
{

    public TouchPadController touchPadController;
    private AudioSource audioSource;
    public AudioClip LEAP_VR_NewUI_Press_On_Ana_01;
    public AudioClip LEAP_VR_NewUI_Press_Off_Ana_011;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            //touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_On_Ana_02");
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_On_Ana_02");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            touchPadController.Add();

            //touchPadController.touchPadButtonClickPlaySound("LEAP_VR_NewUI_Press_Off_Ana_01");
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/LEAP_VR_NewUI_Press_Off_Ana_01");
            this.GetComponent<AudioSource>().PlayOneShot(audioClip);


        }
    }


}
