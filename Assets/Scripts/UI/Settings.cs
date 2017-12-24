using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    private Animator settingsAnimator;
    private bool active;
    private Canvas myCanvas;

    public GameObject mainMenuWrapper;
    // Use this for initialization
    void Start()
    {
        active = false;
        myCanvas = this.gameObject.GetComponent<Canvas>();
        settingsAnimator = this.gameObject.GetComponent<Animator>();
        myCanvas.enabled = false;

    }
    public void hide()
    {
        active = false;
        settingsAnimator.SetBool("Active", active);
        mainMenuWrapper.GetComponent<MainMenu>().show();
    }
    
    public IEnumerator turnOffCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        myCanvas.enabled = false;
    }

	
    
}
