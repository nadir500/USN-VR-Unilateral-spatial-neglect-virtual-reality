using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    private Animator mainMenuAnimator;
    private bool active;
    private Canvas myCanvas;

    public GameObject settingsWrapper;
    public GameObject creditsWrapper;
    public RoadController roadController;
    public Canvas uiMainCanvas;

    // Use this for initialization
    void Start () {
        active = true;
        myCanvas = this.gameObject.GetComponent<Canvas>();
        mainMenuAnimator = this.gameObject.GetComponent<Animator>();
        myCanvas.enabled = true;
        uiMainCanvas.enabled = true;
        setExperementParametersToLastSavedOnes();

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
    }
    public void newGame()
    {
        Debug.Log("newGame()");
        uiMainCanvas.enabled = false;
        roadController.generateRoads();
    }
    public void loadGame()
    {
        Debug.Log("loadGame()");
        uiMainCanvas.enabled = false;
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
