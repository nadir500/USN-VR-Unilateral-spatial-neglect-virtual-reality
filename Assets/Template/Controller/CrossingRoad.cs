using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour {

	void OnTriggerEnter(Collider yellowPointBoxCollider)
	{
        GameObject.Find("FadeGameObject").GetComponent<Fading>().BeginFade(2);
        if (yellowPointBoxCollider.gameObject.name.Equals(value: "midwalk"))
        {
        //putting audio sources
			
        }


        else
        if (yellowPointBoxCollider.gameObject.name == "sidewalk")
        {
            //putting audio source 

        }
		  


	 




	}
}
