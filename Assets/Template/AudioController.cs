﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    // This class is the Audio Manager
    // to avoid add audio source to each GameObject have sound we create on GameObject with this script attached to it
    // this script has general method to play any audio sound
    // you call playAudioCLip wthi these Parameters:
    //  audioName: the audio clip name at "Audio/DRSounds"
    //  waitTime: the time before playeing the clip (delay time)    #OPTIONAL
    //  clipTime: to stop the clip after time   #OPTIONAL
    public AudioSource audioSource;
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
        switch (ExperementParameters.soundDirections)
        {
            case "Off":
                {
                    audioSource.volume = 0;

                    break;
                }
            case "Left":
                {
                    audioSource.panStereo = -1;

                    break;
                }
            case "Right":
                {
                    audioSource.panStereo = 1;

                    break;
                }
            case "Both":
                {
                    audioSource.volume = 1;

                    audioSource.panStereo = 0;

                    break;
                }

        }
    }

    public void playAudioClip(string audioName, float waitTime = 0.0f, float clipTime = 30.0f)
    {
        StartCoroutine(playTheClipWithTimes(audioName, waitTime, clipTime));
    }

    IEnumerator playTheClipWithTimes(string audioName, float waitTime = 0.0f, float clipTime = 30.0f)
    {
        AudioClip ac = Resources.Load("Audio/DRSounds/" + audioName) as AudioClip;
        audioSource.clip = ac;

        yield return new WaitForSeconds(waitTime);
  
        audioSource.Play();

        yield return new WaitForSeconds(clipTime);
        audioSource.Stop();
    }
	

}
