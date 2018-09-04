using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class MainMenu : MonoBehaviour
{
    private Animator mainMenuAnimator;      // reference the the Animator on main menu ui
    private bool active;                    // boolean to tell if the main menu ui is shown or not
    private Canvas myCanvas;                // reference to the main menu ui canvas

    public GameObject settingsWrapper;      // reference to the settings ui game object (settingsWrapper), assigned in the inspector
    public GameObject creditsWrapper;       // reference to the settings ui game object (settingsWrapper), assigned in the inspector


    public Canvas uiMainCanvas;             // reference to the main canvas that contains main menu - settings - credit, assigned in the inspector to "uiMainCanvas" game object

    public Button startGameButton;          // reference to the start game button on the main menu (to disable )
    public Button testGameButton;           // reference to the test game button on the main menu
    public InputField player_name_InputField;               //Nadir
    public static int playMode;         // 0 => test ; 1 => full boolean to check if the game mode is testing or full experement
    private DataService _sqlite_connection_gamoplay;        //Nadir

    // Use this for initialization
    /*
        1- we should ti disable the vr mode to let the researcher could use the 2d UIs to set the settings
        2- get the animator by assign the mainMenuAnimator reference to the Animator component on (mainMenuWrapper/this.gameObject) game object
        3- Enable UI canvas (uiMainCanvas)
        4- assing the myCanvas reference to the canvas reference on (mainMenuWrapper/this.gameObject) game object and Enable it
        5- set the exoerement parameters to the last saved one in player prefs
        6- if check if the The last settings have set was tested or not
        6-1- if it have tested then the startGameButton will be enabled
        6-2- else it will be disabled until the test game is done
        7- if this is the first use then isSettingsChanged key is not exist and 
     */
    void Start()
    {
        player_name_InputField.onValueChange.AddListener(delegate { InputFieldChangedValue(); });
        _sqlite_connection_gamoplay = new DataService("USN_Simulation.db");
        UnityEngine.XR.XRSettings.enabled = false;
        uiMainCanvas.enabled = true;
        myCanvas = this.gameObject.GetComponent<Canvas>();
        myCanvas.enabled = true;
        mainMenuAnimator = this.gameObject.GetComponent<Animator>();
        active = true;
        setExperimentParametersToLastSavedOnes();


        if (PlayerPrefs.HasKey("isSettingsChanged")) // 1 => the settings have been changed
        {
            if (PlayerPrefs.GetInt("isSettingsChanged") == 1)
            {
                Debug.Log("main menu start settings changed, 1");
                startGameButton.interactable = false;
            }
            else
                Debug.Log("main menu start settings changed, 0");
        }
        else
        {
            Debug.Log("main menu start create new settings changed 0");
            PlayerPrefs.SetInt("isSettingsChanged", 0);
            PlayerPrefs.Save();
        }

    }
    /*
        Parameters:
        Returns: void
        Objective: get the saved data from the playerPrefs and assign it to the ExperementParam
        check if it exist (because the first time we open the game the key will be not Exist)
        //Nadir talk aboud the gameplay id
     */
    public void setExperimentParametersToLastSavedOnes()
    {
        if (PlayerPrefs.HasKey("numberOfPathsPerStreet") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("numberOfPathsPerStreet"))))
            ExperimentParameters.lanes_per_direction = int.Parse(PlayerPrefs.GetString("numberOfPathsPerStreet"));

        if (PlayerPrefs.HasKey("streetsDirections") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("streetsDirections"))))
            ExperimentParameters.streetsDirections = PlayerPrefs.GetString("streetsDirections");

        if (PlayerPrefs.HasKey("carsSpeed") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("carsSpeed"))))
            ExperimentParameters.carsSpeed = PlayerPrefs.GetString("carsSpeed");

        if (PlayerPrefs.HasKey("distanceBetweenCars") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("distanceBetweenCars"))))
            ExperimentParameters.distanceBetweenCars = int.Parse(PlayerPrefs.GetString("distanceBetweenCars"));

        if (PlayerPrefs.HasKey("PatientHeight") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("PatientHeight"))))
            ExperimentParameters.lengthOfPatient = float.Parse(PlayerPrefs.GetString("PatientHeight"));

        if (PlayerPrefs.HasKey("soundsDirection") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("soundsDirection"))))
            ExperimentParameters.soundDirections = PlayerPrefs.GetString("soundsDirection");

        if (PlayerPrefs.HasKey("observeFrameRate") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("observeFrameRate"))))
            ExperimentParameters.observeFrameRate = PlayerPrefs.GetString("observeFrameRate");

        if (PlayerPrefs.HasKey("VehicleType") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("VehicleType"))))
            ExperimentParameters.carType = PlayerPrefs.GetString("VehicleType");

        if (PlayerPrefs.HasKey("NumberOfRoads") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("NumberOfRoads"))))
        {
            ExperimentParameters.numberOfRoads = int.Parse(PlayerPrefs.GetString("NumberOfRoads"));
        }
        if (PlayerPrefs.HasKey("ColorsChoice") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("ColorsChoice"))))
        {
            ExperimentParameters.colorChoice = PlayerPrefs.GetString("ColorsChoice");
        }
        if (PlayerPrefs.HasKey("AfterAccidentEvent") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("AfterAccidentEvent"))))
        {
            ExperimentParameters.afterAccidentEvent = PlayerPrefs.GetString("AfterAccidentEvent");
        }


        if (PlayerPrefs.HasKey("gameplay_id") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("gameplay_id"))))
        {
            if (CheckGamplayID())
            {
                ExperimentParameters.gameplay_id = int.Parse(PlayerPrefs.GetString("gameplay_id"));

                Debug.Log("Latest Gameplay_id " + ExperimentParameters.gameplay_id);
            }
            else
            {
                ExperimentParameters.gameplay_id = _sqlite_connection_gamoplay.GetGameplayIDFromDatabase();
                Debug.Log("GamePlay ID Not Matched and the new one is " + ExperimentParameters.gameplay_id);
            }

        }
        else

        {
            Debug.Log("NOT FOUND gameplay_id ");
        }




    }
    //Nadir
    public bool CheckGamplayID() //check if PlayerPrefs gameplay_id is the same as the latest row in the table 
    {

        return _sqlite_connection_gamoplay.GetGameplayIDFromDatabase() == ExperimentParameters.gameplay_id;
    }
    public void hide()
    {
        active = false;
        mainMenuAnimator.SetBool("Active", active);
    }
    /*
        Parameters:
        Returns: void
        Objective: activate the main menu ui
        if check if the The last settings have set was tested or not
        if it have tested then the startGameButton will be enabled
        else it will be disabled until the test game is done
     */
    public void show()
    {
        active = true;
        mainMenuAnimator.SetBool("Active", active);

        if (PlayerPrefs.HasKey("isSettingsChanged")) // 1 => the settings have been changed
        {
            if (PlayerPrefs.GetInt("isSettingsChanged") == 1)
            {
                Debug.Log("show settings changed, 1");
                startGameButton.interactable = false;
            }
            else
            {
                Debug.Log("show settings changed, 0");
            }
        }
        else
            Debug.Log("has not key or null or empty");
    }

    /*
        Parameters:
        Returns: void
        Objective:
            start the experement of crossing streets
            set the playMode to 1 to tell the other scripts that this is a full experement game (not just testing) (some times we used the isSettingsChanged key)
            remove the UIs
            go the street crossing scene
     */
    public void newGame()
    {
        Debug.Log("newGame()");
        playMode = 1;               //// 0 => test ; 1 => full  
        PlayerPrefs.SetString("CrossingChoice", playMode.ToString());

        ExperimentParameters.gameplay_id = _sqlite_connection_gamoplay.GetGameplayIDFromDatabase();
        uiMainCanvas.enabled = false;
        //checkPointsController.StartAfterMainMenu();
        // roadController.generateRoads();
        // VRSettings.enabled = true;

        Application.LoadLevel(1);
    }

    /*
        Parameters:
        Returns: void
        Objective:
            start the testing mode of crossing streets experement, players are forced to do it after changeing the settings to be sure that the new settings are fine 
            remove the ui canvas
            go the street crossing scene


     */
    public void testGame()
    {
        playMode = 0;
        PlayerPrefs.SetString("CrossingChoice", playMode.ToString());

        _sqlite_connection_gamoplay.CreateGameplay();
        ExperimentParameters.gameplay_id = _sqlite_connection_gamoplay.GetGameplayIDFromDatabase();
        PlayerPrefs.SetString("gameplay_id", ExperimentParameters.gameplay_id.ToString());
        uiMainCanvas.enabled = false;
        //checkPointsController.StartAfterMainMenu();
        // roadController.generateRoads();
        // VRSettings.enabled = true;
        PlayerPrefs.Save();
        Application.LoadLevel(1);
    }
    public void settings()
    {
        Debug.Log("settings()");
        this.hide();
        settingsWrapper.GetComponent<Canvas>().enabled = true;
        settingsWrapper.GetComponent<Animator>().SetBool("Active", true);
    }
    public void GrabObjectsScene()
    {
        Debug.Log("credits()");
        uiMainCanvas.enabled = false;

        Application.LoadLevel(2);
        //this.hide();
        //  creditsWrapper.GetComponent<Canvas>().enabled = true;
        //  creditsWrapper.GetComponent<Animator>().SetBool("Active", true);
    }
    public void exit()
    {
        Debug.Log("exit()");
        Application.Quit();
    }

    public void InputFieldChangedValue()
    {
        ExperimentParameters.player_name = player_name_InputField.text;
        Debug.Log("Value Name " + ExperimentParameters.player_name);
    }
}
