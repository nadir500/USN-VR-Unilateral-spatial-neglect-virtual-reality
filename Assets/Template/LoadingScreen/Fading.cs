using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fading : MonoBehaviour
{
    public CheckPointsController checkPointsController;
    //making the fade screen effect 
    public CanvasGroup fadecanvas;  //fade canvas 
    public GameObject fadeGB;
    public float fadeSpeed = 0.8f;
    private int drawDepth = -1000;
    private float alpha = 0.0f;
    private int fadeDirection = -1;
    private Color darkRedColor;
    GameObject fadeImage;
    GameObject loadingImage;
    LayerMask everythingMask = -1;
    AudioSource audioSource;
    GameObject fakefadechild;
    GameObject kinectCamera;
    float lastX;
    float lastY;
    float lastZ;
    void Start()
    {

        //checkPointsController.backToOtherSideCheckPointReachedEvent += backToOtherSideRemoveFade;
        kinectCamera =  Camera.main.gameObject;
        fakefadechild = GameObject.Find("FadeFakeChildKinect") as GameObject;
        darkRedColor = new Color32(38, 20, 20, 255); //by default 
        fadeImage = GameObject.Find("FadeImage") as GameObject;
        loadingImage = GameObject.Find("LoadingImage") as GameObject;
        loadingImage.SetActive(false);
    }
    public void SetCanvasFadeReference(CanvasGroup fadecanvas)
    {
        this.fadecanvas = fadecanvas;
    }
    void LateUpdate()
    {
//        Debug.Log("Camera Current Parent " + fakefadechild.transform.parent.transform.parent.gameObject.name);
       fadeGB.transform.position = kinectCamera.transform.TransformPoint(new Vector3(fakefadechild.transform.localPosition.x,fakefadechild.transform.localPosition.y,fakefadechild.transform.localPosition.z+0.02f));

        Quaternion From = fadeGB.transform.rotation;
        Quaternion To = kinectCamera.transform.rotation;
        fadeGB.transform.rotation = Quaternion.Lerp(From, To, 1);
    }
    void OnGUI()
    {
        if (fadecanvas != null)
        {
            if (fadeDirection == 1) //fading continuously 
            {
                fadeImage.GetComponent<Image>().color = darkRedColor;
                double dd = (Mathf.Sin(Time.time * 5) + 1.0) / 2.0;
                if (dd <= 0.6)
                {
                    alpha = Mathf.Clamp01(float.Parse(dd.ToString())); //controlling the alpha of a canvas (here is fade continuosly in red cuz the player hit a car)
                    fadecanvas.alpha = alpha; //assigning it to the alpha parameter in the canvas 
                }
            }
            else
            //phase 2 fading if he successfully crossed a section  
            {
                alpha += fadeDirection * fadeSpeed * Time.deltaTime;
                alpha = Mathf.Clamp01(alpha);
                fadecanvas.alpha = alpha;  //fading entirly 
                if (!RoadController.fadeout_after_crossing) //if the server sent the false value to the client 
                {
//                    Debug.Log("RoadController condition");
                    darkRedColor = new Color32(38, 20, 20, 255); //return to red by default 
                    loadingImage.SetActive(false);
                    //reverse fading
                    fadeDirection = -1;
                    //see everything with UIs and all the World's Objects
                    Camera.main.cullingMask = everythingMask;
                }
            }
        }

    }
    //the trigger to begin fade 
    public void BeginFade(int direction)
    {
        if (direction == 2) //fade entirely 
        {
            //changing the UI Color for phase 2 Fade
            fadeImage.GetComponent<Image>().color = Color.black;
            //making the loading Icon Appear
            loadingImage.SetActive(true);
        }
        //the effect of this assignment will result in OnGUI method above 
        fadeDirection = direction;
    }

}
