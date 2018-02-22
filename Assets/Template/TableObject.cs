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
}
