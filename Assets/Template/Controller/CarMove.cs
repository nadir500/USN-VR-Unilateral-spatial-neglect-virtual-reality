using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour {
 public float speed = 3.5f;
  
	// Use this for initialization
	void Start () {
        speed = Random.RandomRange(2, 4f);
    }
	
	// Update is called once per frame
	void Update () {
	 transform.position -= Vector3.forward * Time.deltaTime * speed;
    }
}
