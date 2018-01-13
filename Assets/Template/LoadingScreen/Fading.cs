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
	
    void OnGUI()
    {
		
		if(fadeDirection ==1)
		{
      	  //alpha += fadeDirection * fadeSpeed * Time.deltaTime;
	  	 double dd = (Mathf.Sin(Time.time * 5) + 1.0)/2.0;
			if(dd <=0.6)
			{
      		  alpha = Mathf.Clamp01(float.Parse(dd.ToString()));
       		 fadecanvas.alpha = alpha;
			//Debug.Log("dddd" +  dd);
			}
		}
	}
    public float BeginFade(int direction)
	{
		fadeDirection = direction;
		return fadeSpeed;
	}
}
