using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fading : MonoBehaviour {

	public CanvasGroup fadecanvas;
	public float fadeSpeed= 0.8f;
	private int drawDepth= -1000;
	private float alpha=0.0f;
	private int fadeDirection= -1;
	private Color darkRedColor;
	GameObject fadeImage;
	GameObject loadingImage;
    LayerMask everythingMask = -1;
    AudioSource audioSource;
    void Start()
    {
        darkRedColor = new Color32(38, 20, 20, 255); //by default 

        fadeImage = GameObject.Find("FadeImage") as GameObject;
        loadingImage = GameObject.Find("LoadingImage") as GameObject;
        loadingImage.SetActive(false);

    }
    void OnGUI()
    {
		if(fadeDirection ==1) //fading continously 
		{
 		 fadeImage.GetComponent<Image>().color = darkRedColor;
	  	 double dd = (Mathf.Sin(Time.time * 5) + 1.0)/2.0; 
			if(dd <=0.6)
			{
      		  alpha = Mathf.Clamp01(float.Parse(dd.ToString()));
       		 fadecanvas.alpha = alpha;
 			}
		}
		else 
		  //phase 2 fading if he successfully crossed a section  
		{
			alpha += fadeDirection * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
       		fadecanvas.alpha = alpha;  //fading entirly 
			if(!RoadController.fadeout_after_crossing) //if the server sent the false value to the client 
			{
				darkRedColor = new Color32(38, 20, 20, 255); //return to red by default 
				loadingImage.SetActive(false); 
				//reverse fading
				fadeDirection = -1;
				//see everything with UIs and all the World's Objects
                Camera.main.cullingMask = everythingMask;

            }
		}

	}

   public IEnumerator playSound(string s) //playing sound from resources 
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        AudioClip ac = Resources.Load("Audio/DRSounds/" + s) as AudioClip;
        audioSource.clip = ac;
        yield return new WaitForSeconds(1);

        audioSource.Play();
    }

    public void BeginFade(int direction)
	{
		if(direction ==2 )
		{
			//changing the UI Color for phase 2 Fade
			fadeImage.GetComponent<Image>().color=Color.black;
			//making the loading Icon Appear
			loadingImage.SetActive(true); 
		}
		//the effect of this assignment will result in OnGUI method above 
		fadeDirection = direction;
	}
    
}
