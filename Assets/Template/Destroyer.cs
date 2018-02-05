using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter(Collider hitbox)
    {
        if(hitbox.gameObject.tag=="Car")
        {
            Destroy(hitbox.transform.parent.gameObject);
        }
    }
}
