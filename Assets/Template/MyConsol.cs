using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyConsol : MonoBehaviour {
    static Text consolText;
    static Scrollbar scroll;
	// Use this for initialization
	void Start () {
        consolText = this.gameObject.GetComponent<Text>();
        scroll = this.transform.parent.GetChild(1).GetComponent<Scrollbar>();
        InvokeRepeating("clearConsol", 1f, 1f);
	}
	
    public static void log(string s)
    {
        int index = consolText.text.IndexOf(s);
        //if (index == -1)
            consolText.text += s + "\t"+"\n";
        scroll.value = 0;

    }
    void clearConsol()
    {
        consolText.text = "";
    }
}
