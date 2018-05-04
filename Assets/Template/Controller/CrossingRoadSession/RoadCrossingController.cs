using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class RoadCrossingController : MonoBehaviour
{
    //make everything ready in this class for the grabbing object's session 
    public CheckPointsController checkPointsController;
    public RoadController roadController;
     void Start()
     {
         checkPointsController.StartAfterMainMenu();
         roadController.generateRoads();
         VRSettings.enabled = true;
     }
}
        