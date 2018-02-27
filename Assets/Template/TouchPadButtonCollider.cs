using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadButtonCollider : MonoBehaviour {

	public TouchPadController touchPadController;
	
	void Intialize()
	{
		//touchPadController = GameObject.Find("TouchPadCanvas").GetComponent<TouchPadController>();
		
	}
	void OnTriggerExit(Collider hitHand)
	{
		
		touchPadController.numberOnPad = int.Parse(this.gameObject.name[0].ToString());
	}
}
