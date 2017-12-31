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
    public SettingsParameter distanceBetweenCarsParameterWrapper;
    public int distanceBetweenCarsValue { get { return int.Parse(distanceBetweenCarsParameterWrapper.parameterValue.Split(' ')[0].ToString()); } set { distanceBetweenCarsParameterWrapper.parameterValue = value.ToString(); } }
    public SettingsParameter carsSpeedParameterWrapper;
    public int carsSpeedValue { get { return  int.Parse(carsSpeedParameterWrapper.parameterValue.Split(' ')[0].ToString()); } set { carsSpeedParameterWrapper.parameterValue = value.ToString(); } }

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
        Debug.Log("changeModeHandler");
        switch (modeValue)
        {
            case "Mode1":
                Debug.Log("Mode1");
                setModeParameters(2, "Left", 20, 30);
                break;
            case "Mode2":
                Debug.Log("Mode2");
                setModeParameters(4, "Left To Right", 30, 25);
                break;
            case "Mode3":
                Debug.Log("Mode3");
                setModeParameters(6, "Right To Left", 40, 20);
                break;
            default:
                break;
        }
    }

    private void setParameterIndex(SettingsParameter parameter)
    {
        parameter.index = parameter.values.IndexOf(parameter.parameterValue);
    }
    private void setModeParameters(int numberOfPathsPerStreet, string streetsDirections, int carsSpeed, int distanceBetweenCars)
    {
        this.numberOfPathsPerStreetValue = numberOfPathsPerStreet;
        this.setParameterIndex(numberOfPathsPerStreetParameterWrapper);

        this.streetsDirectionsValue = streetsDirections;
        this.setParameterIndex(streetsDirectionsparameterWrapper);

        this.carsSpeedValue = carsSpeed;
        this.setParameterIndex(carsSpeedParameterWrapper);

        this.distanceBetweenCarsValue = distanceBetweenCars;
        this.setParameterIndex(distanceBetweenCarsParameterWrapper);
    }

    private void setExperementParameters()
    {
        ExperementParameters.numberOfPathsPerStreet = numberOfPathsPerStreetValue;
        ExperementParameters.streetsDirections = streetsDirectionsValue;
        ExperementParameters.carsSpeed = carsSpeedValue;
        ExperementParameters.distanceBetweenCars = distanceBetweenCarsValue;
    }
}
