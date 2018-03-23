using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameButton : MonoBehaviour
{
    public TableController tableController;
    public AudioController audioController;
    private AudioSource audioSource;
	 AudioClip BeepClip;
    GameObject fingerHit;
    int beepCounter = 0;
    void Start()
    {
        BeepClip = Resources.Load<AudioClip>("Audio/TableSounds/Beep");
		
        audioSource = this.GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LeapFingerSphere"))
        {
            fingerHit = other.gameObject;
            InvokeRepeating("WaitCoupleSeconds", 0, 1);

        }
    }
    void OnTriggerExit(Collider other)
    {
        beepCounter = 0;
    }
    void WaitCoupleSeconds()
    {
        audioSource.PlayOneShot(BeepClip);
        beepCounter++;
        if (beepCounter == 3)
        {
            if (fingerHit != null)
            {
				tableController.RecordCollectedObjectsToDB();
                audioController.playAudioClip("DRSounds/TestCompleted", 0.0f, -1);
                this.gameObject.GetComponent<BoxCollider>().enabled=false;
            }
            CancelInvoke("WaitCoupleSeconds");
        }

    }


}
