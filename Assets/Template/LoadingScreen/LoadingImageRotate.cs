using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImageRotate : MonoBehaviour {

  
	// it's a loading bar rotate nothing more 
	void Update () {
		this.transform.Rotate(new Vector3(0,0,1),20); 
	}
}
