using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    private Animator settingsAnimator;
    private bool active;
    private Canvas myCanvas;

    public GameObject mainMenuWrapper;

    public SettingsParameter mode;
    public string modeValue { get { return mode.parameterValue; } }
    public SettingsParameter numberOfPathsPerStreetParameterWrapper;
    public int numberOfPathsPerStreetValue { get { return int.Parse(numberOfPathsPerStreetParameterWrapper.parameterValue); } set { numberOfPathsPerStreetParameterWrapper.parameterValue = value.ToString(); } }
    public SettingsParameter streetsDirectionsparameterWrapper;
    public string streetsDirectionsValue { get { return streetsDirectionsparameterWrapper.parameterValue; } set { streetsDirectionsparameterWrapper.parameterValue = value; } }

    // Use this for initialization
    void Start()
    {
        active = false;
        myCanvas = this.gameObject.GetComponent<Canvas>();
        settingsAnimator = this.gameObject.GetComponent<Animator>();
        myCanvas.enabled = false;

        mode.OnVariableChange += changeModeHandler;

    }
    public void hide()
    {
        active = false;
        settingsAnimator.SetBool("Active", active);
        mainMenuWrapper.GetComponent<MainMenu>().show();

        setExperementParameters();
    }
    
    public IEnumerator turnOffCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        myCanvas.enabled = false;
    }

	private void changeModeHandler()
    {
        switch (modeValue)
        {
            case "Mode1":
                Debug.Log("Mode1");
                setModeParameters(2, "Left");
                break;
            case "Mode2":
                Debug.Log("Mode2");
                setModeParameters(4, "Left To Right");
                break;
            case "Mode3":
                Debug.Log("Mode3");
                setModeParameters(6, "Right To Left");
                break;
            default:
                break;
        }
    }

    private void setParameterIndex(SettingsParameter parameter)
    {
        parameter.index = parameter.values.IndexOf(parameter.parameterValue);

        if (parameter.index == parameter.values.Count - 1)
            parameter.plusButton.interactable = false;   
        else
            parameter.plusButton.interactable = true;

        if (parameter.index == 0)
            parameter.minusButton.interactable = false;
        else
            parameter.minusButton.interactable = true;
    }
    private void setModeParameters(int numberOfPathsPerStreet, string streetsDirections)
    {
        numberOfPathsPerStreetValue = numberOfPathsPerStreet;
        setParameterIndex(numberOfPathsPerStreetParameterWrapper);

        streetsDirectionsValue = streetsDirections;
        setParameterIndex(streetsDirectionsparameterWrapper);
    }

    private void setExperementParameters()
    {
        ExperementParameters.numberOfPathsPerStreet = numberOfPathsPerStreetValue;
        ExperementParameters.streetsDirections = streetsDirectionsValue;
    }
}
