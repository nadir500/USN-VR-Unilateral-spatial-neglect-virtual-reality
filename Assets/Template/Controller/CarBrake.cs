using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrake : MonoBehaviour {

		Rigidbody rb;
    void OnTriggerEnter(Collider playerCollider)
    {
		
		GameObject parent = this.transform.parent.gameObject;
		 rb = parent.GetComponent<Rigidbody>();

		int fadeValue= Random.Range(0,1);

		rb.drag = 40;

		if(rb.isKinematic != true)
		 parent.GetComponent<CarMove>().onBreak();
		 StartCoroutine(delayedStopCar());
        
		 //assign new time scale value

     //after that in like 1 minute after the scale put some text saying you're about to hit a car 
        float fadeTime = GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
	   new WaitForSeconds(2);
    }

    IEnumerator delayedStopCar()
    {
        yield return new WaitForSeconds(3f);
		rb.isKinematic=true;


    }


}
