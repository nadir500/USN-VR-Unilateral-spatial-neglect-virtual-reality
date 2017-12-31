using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsParameter : MonoBehaviour {
    public List<string> values;
    public string unitOfParameterValue;
    public int _index = 0;   // it is public because it will be setten in the inspector to default value
    public int index {
        get { return _index; }
        set
        {
            _index = value;
            indexValidator();
        }

    }       // it is public because it will be change in Settings class in change mode method
    public Text parameterText;
    public string parameterValue {
        get
        {
            string str = (string.IsNullOrEmpty(unitOfParameterValue)) ? parameterText.text : parameterText.text.Replace(" " + unitOfParameterValue, "");
            Debug.Log("str = "+str);
            return str;
        }
        set
        {
            parameterText.text = value + ((string.IsNullOrEmpty(unitOfParameterValue))? "" :" " + unitOfParameterValue);

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

        indexValidator();
    }
    public void increase()
    {

        index++;
        parameterValue = values[index];

        if (OnVariableChange != null)
            OnVariableChange();
    }
    public void decrease()
    {

        index--;
        parameterValue = values[index];

        if (OnVariableChange != null)
            OnVariableChange();
    }

    private void indexValidator()
    {
        if (_index == values.Count - 1)
            plusButton.interactable = false;
        else
            plusButton.interactable = true;

        if (_index == 0)
            minusButton.interactable = false;
        else
            minusButton.interactable = true;
    }
}
