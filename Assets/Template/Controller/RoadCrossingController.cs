using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class RoadCrossingController : MonoBehaviour {
public CheckPointsController checkPointsController;
public RoadController roadController;
	void Start()
	{
		checkPointsController.StartAfterMainMenu();
		roadController.generateRoads();
		VRSettings.enabled = true;
	}
}
