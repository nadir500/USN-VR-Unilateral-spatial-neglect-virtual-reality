using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public AudioClip enginSound;
    public AudioClip brakeSound;
    // Use this for initialization
    void Start () {
        enginSound = Resources.Load("Audio/CarEngine") as AudioClip;
        brakeSound = Resources.Load("Audio/tires_squal_loop") as AudioClip;
    }
	
	// Update is called once per frame
	void Update () {
		myVoid ();
	}

	void myVoid (){
		if (Input.GetButton ("Jump")){
            Debug.Log("space");
            //Vector3 tempLocation = transform.position;
            //tempLocation.y += 1f*Time.deltaTime;
            //transform.position = tempLocation;
            this.GetComponent<AudioSource>().clip = brakeSound;
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            Debug.Log("no space");
            //this.GetComponent<AudioSource>().PlayOneShot(enginSound, 1);
            this.GetComponent<AudioSource>().clip = enginSound;
            this.GetComponent<AudioSource>().Play();
        }
	}
}
