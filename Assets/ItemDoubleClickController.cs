using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDoubleClickController : MonoBehaviour {


	private int clickCount=0;
	void OnTriggerExit(Collider hitFinger)
	{
		if (hitFinger.gameObject.name.Equals("bone3") && hitFinger.transform.parent.Equals("index"))
		{
			clickCount+=1;
		}
		if(clickCount==2)
		{
			Destroy(this.gameObject);
		}
	}

}
