using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class MainMenu : MonoBehaviour {
    private Animator mainMenuAnimator;
    private bool active;
    private Canvas myCanvas;

    public GameObject settingsWrapper;
    public GameObject creditsWrapper;
    public RoadController roadController;
    public Canvas uiMainCanvas;

    public Button startGameButton;
    public Button testGameButton;
    public static int playMode;         // 0 => test ; 1 => full
    // Use this for initialization
    void Start () {
        VRSettings.enabled = false;
        active = true;
        myCanvas = this.gameObject.GetComponent<Canvas>();
        mainMenuAnimator = this.gameObject.GetComponent<Animator>();
        myCanvas.enabled = true;
        uiMainCanvas.enabled = true;
        setExperementParametersToLastSavedOnes();

        
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

    public void setExperementParametersToLastSavedOnes()
    {
        if (PlayerPrefs.HasKey("numberOfPathsPerStreet") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("numberOfPathsPerStreet"))))
            ExperementParameters.numberOfPathsPerStreet = int.Parse(PlayerPrefs.GetString("numberOfPathsPerStreet"));
        if (PlayerPrefs.HasKey("streetsDirections") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("streetsDirections"))))
            ExperementParameters.streetsDirections = PlayerPrefs.GetString("streetsDirections");
        if (PlayerPrefs.HasKey("carsSpeed") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("carsSpeed"))))
            ExperementParameters.carsSpeed = int.Parse(PlayerPrefs.GetString("carsSpeed"));
        if (PlayerPrefs.HasKey("distanceBetweenCars") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("distanceBetweenCars"))))
            ExperementParameters.distanceBetweenCars = int.Parse(PlayerPrefs.GetString("distanceBetweenCars"));

        if (PlayerPrefs.HasKey("PatientHeight") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("PatientHeight"))))
            ExperementParameters.lengthOfPatient = float.Parse(PlayerPrefs.GetString("PatientHeight"));

        if (PlayerPrefs.HasKey("soundDirections") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("soundDirections"))))
            ExperementParameters.soundDirections =PlayerPrefs.GetString("soundDirections");
    
        if (PlayerPrefs.HasKey("gameplay_id") && (!string.IsNullOrEmpty(PlayerPrefs.GetString("gameplay_id"))))
        {
            ExperementParameters.gameplay_id = int.Parse(PlayerPrefs.GetString("gameplay_id"));
            Debug.Log("Latest Gameplay_id " + ExperementParameters.gameplay_id);
        }
        else
        {
         //   ExperementParameters.gameplay_id = 1;
            Debug.Log("NOT FOUND gameplay_id ");
        }



        
    }
    
    public bool CheckGamplayID() //check if PlayerPrefs gameplay_id is the same as the latest row in the table 
    {
        DataService _connect_db = new DataService("USN_Simulation");

        return _connect_db.GetGameplayIDFromDatabase()==ExperementParameters.gameplay_id;
    }
    public void hide()
    {
        active = false;
        mainMenuAnimator.SetBool("Active", active);
    }
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
    public void newGame()
    {
        Debug.Log("newGame()");
        playMode = 1;
        DataService _sqlite_connection_gamoplay = new DataService("USN_Simulation.db");
        ExperementParameters.gameplay_id = _sqlite_connection_gamoplay.GetGameplayIDFromDatabase();
        uiMainCanvas.enabled = false;
        roadController.generateRoads();
        VRSettings.enabled = true;


    }
    public void testGame()
    {
        Debug.Log("PLAY GAME ()");
        playMode = 0;
        DataService _sqlite_connection_gamoplay = new DataService("USN_Simulation.db");
        _sqlite_connection_gamoplay.CreateGameplay();
        ExperementParameters.gameplay_id = _sqlite_connection_gamoplay.GetGameplayIDFromDatabase();
        PlayerPrefs.SetString("gameplay_id", ExperementParameters.gameplay_id.ToString());
        Debug.Log("Gameplay ID Changed To = " + ExperementParameters.gameplay_id);

        uiMainCanvas.enabled = false;
        roadController.generateRoads();
        VRSettings.enabled = true;
        PlayerPrefs.SetInt("isSettingsChanged", 0);
        PlayerPrefs.Save();
        
    }
    public void settings()
    {
        Debug.Log("settings()");
        this.hide();
        settingsWrapper.GetComponent<Canvas>().enabled = true;
        settingsWrapper.GetComponent<Animator>().SetBool("Active", true);
    }
    public void credits()
    {
        Debug.Log("credits()");
        this.hide();
        creditsWrapper.GetComponent<Canvas>().enabled = true;
        creditsWrapper.GetComponent<Animator>().SetBool("Active", true);
    }
    public void exit()
    {
        Debug.Log("exit()");
        Application.Quit();
    }
}
