using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
        
    private Animator settingsAnimator;      // reference to the Animator component of Settings ui game object
    private bool active;                    // boolean to know if the settings ui is shown or not
    private Canvas myCanvas;                // reference to the settings ui canvas

    public GameObject mainMenuWrapper;      // reference to the main menu ui game object (mainMenuWrapper), assigned in the inspector
    public Button saveButton;               // reference to the save button in settings ui to enable is it there is something to to saved
    public Canvas saveWindow;               // reference to the save warning window to show it if the settings is close button pressed and settings not saved

    public SettingsParameter mode;          // reference to the mode parameter in the settings ui
    public string modeValue { get { return mode.parameterValue; } }

    /*
        reference to each settings parameter in the settings ui and a proprety to facilitate dealing with them
        references are assigned in the inspector to the paremeters in : settingsWrapper -> MainUI -> Main -> ScrollView -> Content-> (same parameter name)
        
        the parameters are :
        number of paths per street
        steets directions
        distance beteen car
        cars speed
        langth of patient
        sound directions
        observe frame rate
        cars type

        each parameter have
        - values : array of values (strings - numbers)
        - unit of paramter value : (km - sec ...)
        - parameter key : to use it in playerPrefs
        - link to resouces : if the values is presented as images (like in streets directions)
        - Index: the index of defalut value in the values array
        - parameter text : reference to the text that show the value of the parameter (assigned in the inspector)
        - plus button : referene to the plus button of this parameter (assigned in the inspector)
        - minus button : referene to the minus button of this parameter (assigned in the inspector)
        - OnVariableChange : an event to link the parameter to more method if needed (look at link events method)

        the proprites are used to set and get the value of each parameter
        


     */

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

        mode.OnVariableChange                                   += enableSaveChanges;
        streetsDirectionsparameterWrapper.OnVariableChange      += enableSaveChanges;
        numberOfPathsPerStreetParameterWrapper.OnVariableChange += enableSaveChanges;
        carsSpeedParameterWrapper.OnVariableChange              += enableSaveChanges;
        distanceBetweenCarsParameterWrapper.OnVariableChange    += enableSaveChanges;
        lengthOfPatientParameterWrapper.OnVariableChange        += enableSaveChanges;
        soundDirectionsParameterWrapper.OnVariableChange        += enableSaveChanges;
        observeFrameRateParameterWrapper.OnVariableChange       += enableSaveChanges;
        carsTypeParameterWrapper.OnVariableChange               += enableSaveChanges;
    }
    /*
        Parameters:
        Returns: void
        Callers:  Button componenet in the save button in the SettingsUI (assingd in code in start method)
        Objective:
            call setExperementParameters methd thats set all the parameters value to ExpermentParamters class to use it in the other classes and to playerPrefs to save it to next time
            make the saveButton uninteractable after saving the values
            set the isSettingsChanged value in playerPrefs to true to tell the others that the settings have been changed
     */
    public void saveParameters()
    {
        setExperimentParameters();
        saveButton.interactable = false;
        PlayerPrefs.SetInt("isSettingsChanged", 1);
    }
    /*
        Parameters:
        Returns: void
        Callers: save button in "save worning" 
        Objective:
            call save paramters method from the 
            hide "save worning" window
            hide settings ui

     */
    public void saveButtonInSaveWindow()
    {
        saveParameters();
        saveWindow.GetComponent<Canvas>().enabled = false;
        hide();
    }
    /*
        Parameters:
        Returns: void
        Callers:  Button componenet in the discard button in the SettingsUI (assingd in code in start method)
        Objective:
            call setExperementParameters methd thats set all the parameters value to ExpermentParamters class to use it in the other classes and to playerPrefs to save it to next time
            make the saveButton uninteractable after saving the values
            set the isSettingsChanged value in playerPrefs to true to tell the others that the settings have been changed
     */
    public void discardButtonInSaveWindow()
    {
        saveButton.interactable = false;
        hide();
    }
    public void hideInSaveWindow()
    {
        saveWindow.GetComponent<Canvas>().enabled = false;
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
    public IEnumerator turnOffCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        myCanvas.enabled = false;
    }
    /*
        Parameters:
        Returns: void
        Callers:  in mode parameters in settings ui this method is subscriped OnVariableChange event (mode.OnVariableChange += changeModeHandler)
        Objective:
            change all the settings value according to the mode value
            to add new mode you shoud add it in the mode parameters values array and add new case in the switch-case below
            note: the paramters are in the setModeParameters moethd signature parameters in order
            note: if you wan't to add new paramters don't forget to add it to the setModeParameters signeture in the same order
     */
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
    /*
        Parameters:
        Returns: void
        Callers:  changeModeHandler method
        Objective:
            set the mode values to the parameters and update the index in the SettingsParamter class for each parameter
     */
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
    /*
        Parameters:
        Returns: void
        Callers:  setModeParameters method
        Objective:
            find the index of the new value that assigned to the parameter where we changed the mode and update it in SettingsParameter class for this parameter
     */
    private void setParameterIndex(SettingsParameter parameter)
    {
        parameter.index = System.Array.IndexOf(parameter.values, parameter.parameterValue);
    }

    /*
        Parameters:
        Returns: void
        Callers:  any change in any parameter that this method is linked ot the OnVariableChanges event (parameter.OnVariableChanges += enableSaveChanges;)
        Objective:
            check if there is any change in the parameters value 
            if there as any change then the saveButton will be activated
            else deactivate the save button if it was enabled
     */
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
