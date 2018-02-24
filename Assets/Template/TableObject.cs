using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableObject : MonoBehaviour {

    public TableController tableController;
    public Transform canvas;
    public string id;
    public string level;
    public string side;
    public bool obj_recorded_on_pad;
    public bool obj_collected;
    public string obj_caollected_by_hand;
    public Collected_Objects collected_Objects =null;
    public bool finishedRecord=false;
    public int attempts = 1 ;

    void Start()
    {
        
    }
    public void setValues(string id, string level, string side)
    {
        this.id = id;
        this.level = level;
        this.side = side;
        canvas = transform.Find("Canvas");

        if (side.Equals("right"))
        {
            canvas.localEulerAngles = new Vector3(0, 270, 0);
            canvas.GetChild(0).localScale = new Vector3(1, 1, 1);
            canvas.GetChild(0).GetChild(0).localScale = new Vector3(-1, 1, 1);
        }

        transform.Find("Canvas").GetChild(0).GetChild(0).GetComponent<Text>().text = id.ToString();
    }
    public void SetCollectedObject(Collected_Objects collected_Objects)
    {
        this.collected_Objects = collected_Objects;
    }
    public void SetHandHoldObject(bool obj_collected, string  obj_collected_by_hand)
    {
        this.collected_Objects.obj_collected = obj_collected;
        this.collected_Objects.obj_collected_by_hand = obj_collected_by_hand;
    }
}
