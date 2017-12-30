using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsParameter : MonoBehaviour {
    public List<string> values;
    public int index = 0;       // it is public because it will be change in Settings class in change mode method
    public Text parameterText;
    public string parameterValue {
        get
        {
            return parameterText.text;
        }
        set
        {
            parameterText.text = value;
        }
    }
    public Button plusButton;
    public Button minusButton;

    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChange;


    // Use this for initialization
    void Start () {
        parameterValue = values[index];
        plusButton.onClick.AddListener(increase);
        minusButton.onClick.AddListener(decrease);

        if(index == 0)
        {
            minusButton.interactable = false;
        }
        if(index == values.Count - 1)
        {
            plusButton.interactable = false;;
        }
    }
    public void increase()
    {
        Debug.Log("increase()");
        if (index == 0)
        {
            minusButton.interactable = true;
        }

        index++;
        if (index == values.Count - 1)
            plusButton.interactable = false;;
        parameterValue = values[index];
        if (OnVariableChange != null)
            OnVariableChange();
    }
    public void decrease()
    {
        Debug.Log("decrease()");

        if (index == values.Count - 1)
        {
            plusButton.interactable = true;
        }


        index--;

        if (index == 0)
            minusButton.interactable = false;

        parameterValue = values[index];
        if (OnVariableChange != null)
            OnVariableChange();
    }

    
	
}
