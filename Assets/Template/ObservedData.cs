using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservedData : MonoBehaviour
{

    public List<Vector3> playerPositions;      //taken from spineMid GameObject position
    public List<float> playerHeadRotations;    // taken from the camira
    public List<bool> isLookingAtCar;           // taken from CarMove class
    public List<string> traffic_towards_flow;
    public List<int> current_time_span;
    public List<bool> is_hit_by_car;
    public ObservedData(List<Vector3> playerPositions, List<float> playerHeadRotations, List<bool> isLookingAtCar, List<string> traffic_towards_flow, List<int> current_time_span, List<bool> is_hit_by_car)
    {
        this.playerPositions = playerPositions;
        this.playerHeadRotations = playerHeadRotations;
        this.isLookingAtCar = isLookingAtCar;
        this.traffic_towards_flow = traffic_towards_flow;
        this.current_time_span = current_time_span;
        this.is_hit_by_car = is_hit_by_car;
    }



}