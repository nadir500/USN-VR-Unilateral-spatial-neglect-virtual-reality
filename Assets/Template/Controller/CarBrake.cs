using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrake : MonoBehaviour {

		Rigidbody rb;
		GameObject parent;
		bool carHorn=false;
    void OnTriggerEnter(Collider playerCollider)
    {
		
		 parent = this.transform.parent.gameObject;
		 rb = parent.GetComponent<Rigidbody>();

		int fadeValue= Random.Range(0,1);

		rb.drag = 40;

		if(rb.isKinematic != true)
		{
		 parent.GetComponent<CarMove>().onBreak();
		 StartCoroutine(delayedStopCar());
		 StartCoroutine(CarHornSound());
		}

        
		 //assign new time scale value

     //after that in like 1 minute after the scale put some text saying you're about to hit a car 
        float fadeTime = GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
	   new WaitForSeconds(2);
    }
	
    IEnumerator delayedStopCar()
    {
        yield return new WaitForSeconds(3f);
		
		parent.GetComponent<CarMove>().StopSound();

		parent.GetComponent<CarMove>().onRemove();
		parent.GetComponent<AudioSource>().Play();
		rb.isKinematic=true;


    }
	IEnumerator CarHornSound()
	{
		yield return new WaitForSeconds(5f);
		parent.GetComponent<CarMove>().CarHorn();
		
	}


}
