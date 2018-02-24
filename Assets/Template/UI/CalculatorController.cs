using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CalculatorController : MonoBehaviour {

	public Text numbersPad;
	public Button leftButton;
	public Button rightButton;
    public Button addButton;
    public Button clearButton;
    private bool readyToAdd = false;

	private string direction;
    private bool Active = false;
    private Animator calculatorAnimator;

    private GameObject leapMotionCamera;
    public TableController tableController;

    // Use this for initialization
    void Start () {
		numbersPad.text = "0";
		leftButton.interactable = true;
		rightButton.interactable = false;
        calculatorAnimator = this.GetComponent<Animator>();
		direction = "none";
        InvokeRepeating("SearchForLeapMotionCamera", 1, 1);
        addButton.interactable = false;
    }
    void SearchForLeapMotionCamera()
    {
        Debug.Log("LeapCameraIntialize");
        leapMotionCamera = GameObject.Find("CenterEyeAnchor") as GameObject;
        if (leapMotionCamera != null)
        {
            leapMotionCameraTracking = true;
            CancelInvoke("SearchForLeapMotionCamera");
            Debug.Log("FOund");
        }
        else
        {
            Debug.Log("not found");
        }
    }
    private bool leapMotionCameraTracking = false;
    private float cameraRotationX = 0.0f;
    void Update()
    {
        if (leapMotionCameraTracking)
        {
            cameraRotationX = leapMotionCamera.transform.eulerAngles.x;
            Debug.Log("camera x = " + cameraRotationX);
            if (((cameraRotationX <=350.0f)&& (cameraRotationX >= 330.0f)) && (!Active))
            {
                Show();
            }
            else if (((cameraRotationX < 0.0f) && (cameraRotationX > 330.0f) || (cameraRotationX >350f) && (cameraRotationX < 359.0f)) &&(Active))
            {
                Hide();
            }
        }
    }
    void Show()
    {
        Active = true;
        calculatorAnimator.SetBool("Active", Active);
    }

    void Hide()
    {
        Active = false;
        calculatorAnimator.SetBool("Active", Active);
        Debug.Log("BoxColliders Enabled");
        for (int i = 0; i < tableController.instantiatedTableActiveGameObjects.Length; i++)
        {
            tableController.instantiatedTableActiveGameObjects[i].GetComponent<BoxCollider>().enabled=true;
        }
    }
    
	public void putNumber(string number)
	{
		numbersPad.text = number;
        if(!direction.Equals("none"))
        {
            Debug.Log("green");
            addButton.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 162, 0, 147);
            addButton.interactable = true;
        }
	}

	public void Done()
	{
		Clear();
		DeSelectDirection();
        leapMotionCameraTracking = false;
        Hide();
        
	}
	public void Clear()
	{
		numbersPad.text = "0";
	}
	public void SelectDirection(string direction)
	{
		this.direction = direction;
        Debug.Log("Direction "+ direction);
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

        if(!numbersPad.Equals("0"))
        {
            Debug.Log("green");
            addButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;
            addButton.interactable = true;
        }

	}
	public void DeSelectDirection()
	{
		this.direction = "none";
		leftButton.interactable = true;
		rightButton.interactable = true;
	}

    public void Ok()
    {
        if(direction.Equals("none"))
        {
            Debug.Log("Direction is null");
            return;
        }
        if(numbersPad.Equals("0"))
        {
            Debug.Log("Number is not selected");
            return;
        }
        addButton.interactable = false;
        tableController.tableObjectSelectedByCalculator(numbersPad.text, direction);

        Clear();
        DeSelectDirection();
    }

}
