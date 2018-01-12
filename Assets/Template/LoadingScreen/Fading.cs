using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

	// Use this for initialization
	public CanvasGroup fadecanvas;
	public float fadeSpeed= 0.8f;
	private int drawDepth= -1000;
	private float alpha=0.0f;
	private int fadeDirection= -1;
	
	/*void OnGUI()
	{
		//fade out when colliding 
		alpha += fadeDirection * fadeSpeed* Time.deltaTime;
		//force clamping between zero and one 
		alpha = Mathf.Clamp01(alpha);
		//set fade screen color 
		GUI.color = new Color (GUI.color.r,GUI.color.g,GUI.color.b,alpha);
		GUI.depth =drawDepth;  //rendering on top 
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),fadeOutTexture);
	}*/
    void OnGUI()
    {
        alpha += fadeDirection * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        fadecanvas.alpha = alpha;
    }
    public float BeginFade(int direction)
	{
		fadeDirection = direction;
		return fadeSpeed;
	}
}
