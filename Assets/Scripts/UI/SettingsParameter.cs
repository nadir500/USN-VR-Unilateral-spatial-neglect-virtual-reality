using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsParameter : MonoBehaviour {
    public List<string> values;
    private int index = 0;
    public GameObject parameterValue;
    public Button plusButton;
    public Button minusButton;
    // Use this for initialization
    void Start () {
        parameterValue.GetComponent<Text>().text = values[index];
        plusButton.onClick.AddListener(increase);
        minusButton.onClick.AddListener(decrease);
    }
    public void increase()
    {
        Debug.Log("increase()");
        if (index == values.Count - 1)
            return;
        index++;
        parameterValue.GetComponent<Text>().text = values[index];
    }
    public void decrease()
    {
        Debug.Log("decrease()");
        if (index == 0)
            return;
        index--;
        parameterValue.GetComponent<Text>().text = values[index];
    }
	
}
