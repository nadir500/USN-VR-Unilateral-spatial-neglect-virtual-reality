using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservedData : MonoBehaviour
{

    public Vector3[] playerPositions;      //taken from spineMid GameObject position
    public float[] playerHeadRotations;    // taken from the camira
    public bool[] isLookingAtCar;           // taken from CarMove class
    public string []  traffic_towards_flow;
    public float [] current_time_span;
    public bool[] is_hit_by_car;


    public ObservedData(Vector3[] playerPositions, float[] playerHeadRotations, bool[] isLookingAtCar, string[] traffic_towards_flow, float[] current_time_span, bool[] is_hit_by_car)
    {
        this.playerPositions = playerPositions;
        this.playerHeadRotations = playerHeadRotations;
        this.isLookingAtCar = isLookingAtCar;
        this.traffic_towards_flow = traffic_towards_flow;
        this.current_time_span = current_time_span;
        this.is_hit_by_car = is_hit_by_car;
    }



}