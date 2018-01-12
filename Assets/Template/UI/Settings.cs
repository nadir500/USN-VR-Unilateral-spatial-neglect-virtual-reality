using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    private Animator settingsAnimator;
    private bool active;
    private Canvas myCanvas;

    public GameObject mainMenuWrapper;
    public Button saveButton;
    public Canvas saveWindow;

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
        settingsAnimator = this.gameObject.GetComponent<Animator>();
        myCanvas = this.gameObject.GetComponent<Canvas>();
        saveWindow.GetComponent<Canvas>().enabled = false;
        saveButton.onClick.AddListener(saveParameters);
        saveButton.interactable = false;
        myCanvas.enabled = false;
        
        linkEvents();
        

    }
    public void linkEvents()
    {
        mode.OnVariableChange += changeModeHandler;
        mode.OnVariableChange += enableSaveChanges;
        streetsDirectionsparameterWrapper.OnVariableChange += enableSaveChanges;
        numberOfPathsPerStreetParameterWrapper.OnVariableChange += enableSaveChanges;
        carsSpeedParameterWrapper.OnVariableChange += enableSaveChanges;
        distanceBetweenCarsParameterWrapper.OnVariableChange += enableSaveChanges;
    }
    public void saveParameters()
    {
        setExperementParameters();
        saveButton.interactable = false;
    }
    public void hide()
    {
        if(saveButton.interactable)
        {
            saveWindow.GetComponent<Canvas>().enabled = true;
            return;
        }
        active = false;
        settingsAnimator.SetBool("Active", active);
        mainMenuWrapper.GetComponent<MainMenu>().show();  
    }
    public void saveButtonInSaveWindow()
    {
        saveParameters();
        saveWindow.GetComponent<Canvas>().enabled = false;
        hide();

    }
    public void discardButtonInSaveWindow()
    {
        saveButton.interactable = false;
        hide();
    }
    public void hideInSaveWindow()
    {
        saveWindow.GetComponent<Canvas>().enabled = false;
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

                setModeParameters(2, "Left", 20, 30);
                break;
            case "Mode2":

                setModeParameters(4, "Left To Right", 30, 25);
                break;
            case "Mode3":

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
    private void enableSaveChanges()
    {
        saveButton.interactable = true;
    }
    private void setExperementParameters()
    {
        Debug.Log("numberOfPathsPerStreet"+ numberOfPathsPerStreetValue);
        PlayerPrefs.SetString("numberOfPathsPerStreet", numberOfPathsPerStreetValue.ToString());
        PlayerPrefs.SetString("streetsDirections", streetsDirectionsValue.ToString());
        PlayerPrefs.SetString("carsSpeed", carsSpeedValue.ToString());
        PlayerPrefs.SetString("distanceBetweenCars", distanceBetweenCarsValue.ToString());
        PlayerPrefs.Save();
        ExperementParameters.numberOfPathsPerStreet = numberOfPathsPerStreetValue;
        ExperementParameters.streetsDirections = streetsDirectionsValue;
        ExperementParameters.carsSpeed = carsSpeedValue;
        ExperementParameters.distanceBetweenCars = distanceBetweenCarsValue;
    }
}
