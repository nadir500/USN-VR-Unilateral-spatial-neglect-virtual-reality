using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDoubleClickController : MonoBehaviour
{


    private int clickCount = 0;
    private DataService dbUpdateClickState;
    void Start()
    {
        dbUpdateClickState = new DataService("USN_Simulation.db");

    }
    void OnTriggerExit(Collider hitFinger)
    {
		Debug.Log("Trigger Exit Finger " + hitFinger.gameObject.name);
        if (hitFinger.gameObject.name.Equals("bone3") )
        {
			Debug.Log("Click Count object " + clickCount );
            clickCount ++;
        }
        if (clickCount > 2)
        {
            string parentFinger = hitFinger.transform.parent.transform.parent.gameObject.name;
            dbUpdateClickState.UpdateCollectedObjectByClicking(int.Parse(this.GetComponent<TableObject>().id), true, parentFinger[parentFinger.Length - 1]);
            Destroy(this.gameObject);
        }
    }

}
