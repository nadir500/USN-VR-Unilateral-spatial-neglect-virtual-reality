using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsParameter : MonoBehaviour {
    public string[] values;                 // All the avaliable values of each parameter
    public string unitOfParameterValue;         // The Unit of the parameter (ex inch)
    public string parameterKey;                 // THe Parameter Key in playePrefs
    public int _index = 0;                      // it is public because it will be set in the inspector to default value
    public bool linkToResources;                // if this boolean is true it will get tha value from the values array and show the texture with same name at Textures\\UiSprites\\
    public bool isIterativeValues;              // if the values ar Sequence of number with constant step this boolean is active a foor loop to create the values 
    public float startValue;                    // if isIterativeValues = true this will be the first value of values
    public int numberOfValues;                  // if isIterativeValues = true this will be the number of values
    public float IterationStepValue;            // if isIterativeValues = true this will be the step between each two numbers

    
    public int index        // this probrity to manage the _index value settings ( to dong get out of range and go back to the first value after tha last one and vise versa)
    {                       // it is public because it will be change in Settings class in change mode method
        get { return _index; }
        set
        { _index = value;
                indexValidator();
        }

    }       
    public Text parameterText;  // The text lable at the ui
    public string parameterValue {  // this probrity to manage the parameter text value settings    (

        get     // remove the unit before reading the value 
        {
            string str = (string.IsNullOrEmpty(unitOfParameterValue)) ? parameterText.text : parameterText.text.Replace(" " + unitOfParameterValue, "");
            return str;
        }
        set     // add the unit when I put a new value for this parameter and if link to resources = true i read the texture with same value and assign it the child image
        {
            parameterText.text = value + ((string.IsNullOrEmpty(unitOfParameterValue)) ? "" : " " + unitOfParameterValue);
            if (linkToResources)
            {
                parameterText.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures\\UiSprites\\" + parameterText.text);
            }
        }
    }
    public Button plusButton;
    public Button minusButton;


    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChange;


    // Use this for initialization
    // if isIterativeValues = true -> create the values
    //  adjust the index
    //  add the method to the buttons
    //  read the last setted values from player prefs
    //  if linkToResourses = ture -> get the texture
    void Start () {

        if(isIterativeValues)
        {
            values = new string[numberOfValues];
            Debug.Log("count = "+values.Length);
            for (int i = 0; i < numberOfValues; i++)
                values[i] = System.Math.Round((startValue + IterationStepValue * i), 2).ToString(); 
        }


        parameterValue = values[index];
        //plusButton.onClick.AddListener(increase);
        //minusButton.onClick.AddListener(decrease);

        //Read the values from player prefs
        if(!string.IsNullOrEmpty(parameterKey) && PlayerPrefs.HasKey(parameterKey) && (!string.IsNullOrEmpty(PlayerPrefs.GetString(parameterKey)) ))
        {
            parameterValue = PlayerPrefs.GetString(parameterKey);
            index = System.Array.IndexOf(values, parameterValue);
        }

        if (linkToResources)
        {
            parameterText.enabled = false;
            parameterText.transform.GetChild(0).gameObject.SetActive(true);
        }



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
        if (_index == values.Length - 1)
            plusButton.interactable = false;
        else
            plusButton.interactable = true;

        if (_index == 0)
            minusButton.interactable = false;
        else
            minusButton.interactable = true;
    }


}
