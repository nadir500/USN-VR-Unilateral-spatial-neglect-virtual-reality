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
		string [] roadType;
    void OnTriggerEnter(Collider playerCollider)
    {
		 roadType = new string [4];
		 parent = this.transform.parent.gameObject;
		 rb = parent.GetComponent<Rigidbody>();

	     playerGB = playerCollider.gameObject;
		 carColliderGB = this.gameObject;
		 roadType = parent.GetComponent<CarMove>().roadType;

     	 rb.drag = 40;

		 isHit=true;
	
        
		 //assign new time scale value

     //after that in like 1 minute after the scale put some text saying you're about to hit a car 
        float fadeTime = GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(1);
	   new WaitForSeconds(2);
    }
	/*void OnTriggerStay(Collider playerCollider)
	{
		Debug.Log("global position of camera parent " + playerCollider.transform.position + " global position car colldier " + this.transform.position );
		
	}*/

	void Update()
	{

          /*  if (rb != null && isHit)
            {
		 playerPos = playerGB.transform.position;
                carColliderPos = carColliderGB.transform.position;
		                Debug.Log("Distance between cars " + Vector3.Distance(playerPos, carColliderPos));
			}*/
 
        if (Time.frameCount % 30 == 0) //excute every couple frames 
        {


            if (rb != null && isHit)
            {
                parent.GetComponent<CarMove>().onBreak();
                playerPos = playerGB.transform.position;
                carColliderPos = carColliderGB.transform.position;
                distance = Vector3.Distance(playerPos, carColliderPos);
                Debug.Log("Distance between cars " + Vector3.Distance(playerPos, carColliderPos));
                if ( roadType[1].Equals(value: "Left") && distance >= 7.0f || roadType[1].Equals(value: "Right") && distance <=8.9f )
                {
                    StopCar();
                    CarHornSound();
                }
            }
        }


	}
    void StopCar()
    {
       // yield return new WaitForSeconds(3f);
		
		parent.GetComponent<CarMove>().StopSound();

		parent.GetComponent<CarMove>().onRemove();
		parent.GetComponent<AudioSource>().Play();
		rb.isKinematic=true;


    }
	void CarHornSound()
	{
		new WaitForSeconds(3);
		parent.GetComponent<CarMove>().CarHorn();
		
	}


}
