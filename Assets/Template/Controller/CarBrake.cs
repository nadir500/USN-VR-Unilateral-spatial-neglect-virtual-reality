using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrake : MonoBehaviour {

		Rigidbody rb;
		GameObject parent;
		GameObject playerGB,carColliderGB;
		Vector3 playerPos,carColliderPos;
		bool carHorn=false;
		bool isHit=false;
		float distance ; 
    void OnTriggerEnter(Collider playerCollider)
    {
		 parent = this.transform.parent.gameObject;
		 rb = parent.GetComponent<Rigidbody>();

	     playerGB = playerCollider.gameObject;
		 carColliderGB = this.gameObject;
     	 rb.drag = 40;

		 isHit=true;
		 //RoadController.fadeout_after_crossing=false;

     //after that in like 1 minute after the scale put some text saying you're about to hit a car 
         GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
    }

	/*void Update()
	{
        if (Time.frameCount % 30 == 0) //excute every couple frames 
        {
            if (rb != null && isHit)
            {
               // parent.GetComponent<CarMove>().onBreak();
                playerPos = playerGB.transform.position;
                carColliderPos = carColliderGB.transform.position;
                distance = Vector3.Distance(playerPos, carColliderPos);
                Debug.Log("Distance between cars " + parent.gameObject.name+" "+ Vector3.Distance(playerPos, carColliderPos));
                if ( roadType[1].Equals(value: "Left") && distance >= 7.0f || roadType[1].Equals(value: "Right") && distance <=8.9f )
                {
                    StopCar();
                    CarHornSound();
                }
            }
        }
	}*/
   
}
