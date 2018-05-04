using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
        
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
    public int carsSpeedValue { get { return int.Parse(carsSpeedParameterWrapper.parameterValue.Split(' ')[0].ToString()); } set { carsSpeedParameterWrapper.parameterValue = value.ToString(); } }

    public SettingsParameter lengthOfPatientParameterWrapper;
    public float lengthOfPatientValue { get { return float.Parse(lengthOfPatientParameterWrapper.parameterValue); } set { lengthOfPatientParameterWrapper.parameterValue = value.ToString(); } }

    public SettingsParameter soundDirectionsParameterWrapper;
    public string soundDirectionsValue { get { return soundDirectionsParameterWrapper.parameterValue; } set { soundDirectionsParameterWrapper.parameterValue = value; } }

    public SettingsParameter observeFrameRateParameterWrapper;
    public float observeFrameRateValue { get { return float.Parse(observeFrameRateParameterWrapper.parameterValue); } set { observeFrameRateParameterWrapper.parameterValue = value.ToString(); } }

    public SettingsParameter carsTypeParameterWrapper;
    public string carsTypeParameterWrappeValue { get { return carsTypeParameterWrapper.parameterValue; } set { carsTypeParameterWrapper.parameterValue = value; } }

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
        lengthOfPatientParameterWrapper.OnVariableChange += enableSaveChanges;
        soundDirectionsParameterWrapper.OnVariableChange += enableSaveChanges;
        observeFrameRateParameterWrapper.OnVariableChange += enableSaveChanges;
        carsTypeParameterWrapper.OnVariableChange += enableSaveChanges;
    }

    public void saveParameters()
    {
        setExperimentParameters();
        saveButton.interactable = false;
        PlayerPrefs.SetInt("isSettingsChanged", 1);
    }

    public void hide()
    {
        if (saveButton.interactable)
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
        parameter.index = System.Array.IndexOf(parameter.values, parameter.parameterValue);
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
        if (
            (ExperimentParameters.lanes_per_direction != numberOfPathsPerStreetValue)    ||
            (ExperimentParameters.streetsDirections != streetsDirectionsValue)              ||
            (ExperimentParameters.carsSpeed != carsSpeedValue)                              ||
            (ExperimentParameters.distanceBetweenCars != distanceBetweenCarsValue)          ||
            (ExperimentParameters.lengthOfPatient != lengthOfPatientValue)                  ||
            (ExperimentParameters.soundDirections != soundDirectionsValue)                  ||
            (ExperimentParameters.observeFrameRate != observeFrameRateValue.ToString())     ||
            (ExperimentParameters.carType != carsTypeParameterWrappeValue)
          )
        {
            saveButton.interactable = true;
        }
        else
            saveButton.interactable = false;

          
        
    }
    private void setExperimentParameters()
    {
        //DataService _sqlite_dataServices = new DataService("USN_Simulation.db");

        PlayerPrefs.SetString("numberOfPathsPerStreet", this.numberOfPathsPerStreetValue.ToString());
        PlayerPrefs.SetString("streetsDirections", this.streetsDirectionsValue.ToString());
        PlayerPrefs.SetString("carsSpeed", this.carsSpeedValue.ToString());
        PlayerPrefs.SetString("distanceBetweenCars", this.distanceBetweenCarsValue.ToString());
        PlayerPrefs.SetString("PatientHeight", this.lengthOfPatientValue.ToString());
        PlayerPrefs.SetString("soundsDirection", this.soundDirectionsValue);
        PlayerPrefs.SetString("observeFrameRate", this.observeFrameRateValue.ToString());
        PlayerPrefs.SetString("VehicleType", this.carsTypeParameterWrappeValue);
        ExperimentParameters.lanes_per_direction = this.numberOfPathsPerStreetValue;
        ExperimentParameters.streetsDirections = this.streetsDirectionsValue;
        ExperimentParameters.carsSpeed = this.carsSpeedValue;
        ExperimentParameters.distanceBetweenCars = this.distanceBetweenCarsValue;
        ExperimentParameters.lengthOfPatient = this.lengthOfPatientValue;
        ExperimentParameters.soundDirections = this.soundDirectionsValue;
        ExperimentParameters.carType = this.carsTypeParameterWrappeValue;
       // _sqlite_dataServices.CreateGameplay();
       // ExperimentParameters.gameplay_id = _sqlite_dataServices.GetGameplayIDFromDatabase();
        ExperimentParameters.observeFrameRate = this.observeFrameRateValue.ToString();
        //Debug.Log("Saved id gamplay in player prefs" + ExperimentParameters.gameplay_id);
        //PlayerPrefs.SetString("gameplay_id", ExperimentParameters.gameplay_id.ToString());  //storing gameplay_id for SQL SERVER Later :D
        PlayerPrefs.Save();
    }
}
