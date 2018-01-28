using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObserves : MonoBehaviour {

    public List<Vector3> playerPositions;
    public List<Vector3> playerHeadRotations;

	// Use this for initialization
	void Initilize () {

        InvokeRepeating("onFrame", 1f, 0.003f);
	}

    void onFrame()
    {
        Debug.Log("frame");

    }
}
