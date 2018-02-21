using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CalculatorController : MonoBehaviour {

	public Text numbersPad;
	public Button leftButton;
	public Button rightButton;

	private string direction;


	// Use this for initialization
	void Start () {
		numbersPad.text = "0";
		leftButton.interactable = true;
		rightButton.interactable = false;
		direction = "none";
	}
	
    void Show()
    {

    }

    void Hide()
    {

    }

	public void putNumber(string number)
	{
		numbersPad.text = number;
	}

	public void Done()
	{
		Clear();
		DeSelectDirection();
	}
	public void Clear()
	{
		numbersPad.text = "0";
	}
	public void SelectDirection(string direction)
	{
		this.direction = direction;
		if(direction.Equals("left"))
		{
			leftButton.interactable = false;
			rightButton.interactable = true;
		}
		else
		{
			leftButton.interactable = true;
			rightButton.interactable = false;
		}

	}
	public void DeSelectDirection()
	{
		this.direction = "none";
		leftButton.interactable = true;
		rightButton.interactable = true;
	}

}
