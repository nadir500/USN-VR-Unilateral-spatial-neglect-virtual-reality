using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TouchPadController : MonoBehaviour
{

    public Text numbersPad;

    public Button leftButton;
    public Button rightButton;
    public Button addButton;
    public Button clearButton;

    private string objectPosition;
    private bool Active = false;
    private Animator calculatorAnimator;

    private GameObject leapMotionCamera;
    public TableController tableController;
    public AudioController audioController;
    public int numberOnPad;


    // Use this for initialization
    void Start()
    {
        numbersPad.text = "0";
        turnLeapMotionUiButton(leftButton, true);
        turnLeapMotionUiButton(rightButton, true);
        turnLeapMotionUiButton(addButton, false);
        objectPosition = "none";
        calculatorAnimator = this.GetComponent<Animator>();
        InvokeRepeating("SearchForLeapMotionCamera", 1, 1);
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
            if (((cameraRotationX <= 357.0f) && (cameraRotationX >= 330.0f)) && (!Active))
            {
                Show();
            }
            else if (((cameraRotationX > 0.0f) && (cameraRotationX < 330.0f) || (cameraRotationX > 357.0f) && (cameraRotationX < 359.0f)) && (Active))
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
    }

    public void putNumber(string number)
    {
        numbersPad.text = number;
        if (!objectPosition.Equals("none"))
        {
            Debug.Log("num = " + number + " dir = " + objectPosition);
            Debug.Log("add = " + addButton.interactable + "  right = " + rightButton.interactable + "  left =" + leftButton.interactable);
            addButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 162, 0, 147);
            addButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.black;
            turnLeapMotionUiButton(addButton, true);
        }
    }

    public void Done()
    {
        audioController.playAudioClip("DRSounds/TouchpadDone", 0, 19);
        Clear();
        DeSelectObjectPosition();
        tableController.EnableAllGameObjectsBoxColliders();
        leapMotionCameraTracking = false;
        Hide();

        tableController.DoneWithTouchPad();

    }
    public void Clear()
    {
        numbersPad.text = "0";
        DeSelectObjectPosition();
        turnLeapMotionUiButton(addButton, false);

        addButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(13, 13, 13, 255);
        addButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = new Color32(99, 99, 99, 255);

    }
    public void SelectObjectPosition(string objectPosition)
    {
        this.objectPosition = objectPosition;
        if (objectPosition.Equals("left"))
        {
            Debug.Log("num = " + numbersPad.text + " dir = " + objectPosition);
            Debug.Log("add = " + addButton.interactable + "  right = " + rightButton.interactable + "  left =" + leftButton.interactable);

            turnLeapMotionUiButton(leftButton, false);
            turnLeapMotionUiButton(rightButton, true);
        }
        else
        {
            Debug.Log("num = " + numbersPad.text + " dir = " + objectPosition);
            Debug.Log("add = " + addButton.interactable + "  right = " + rightButton.interactable + "  left =" + leftButton.interactable);

            turnLeapMotionUiButton(leftButton, true);
            turnLeapMotionUiButton(rightButton, false);
        }

        if (!numbersPad.text.Equals("0"))
        {
            Debug.Log("green");
            addButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 162, 0, 147);
            addButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = new Color32(204, 197, 197, 255);
            turnLeapMotionUiButton(addButton, true);
        }

    }
    public void DeSelectObjectPosition()
    {
        this.objectPosition = "none";
        turnLeapMotionUiButton(leftButton, true);
        turnLeapMotionUiButton(rightButton, true);
    }

    private void turnLeapMotionUiButton(Button button, bool interactable)
    {
        button.interactable = interactable;
        button.gameObject.GetComponent<Leap.Unity.InputModule.CompressibleUI>().enabled = interactable;
        StartCoroutine(turnLeapMotionUIButtonSound(button.gameObject, interactable));

    }
    private IEnumerator turnLeapMotionUIButtonSound(GameObject buttonGameObject, bool status)
    {
        yield return new WaitForSeconds(0.3f);
        buttonGameObject.GetComponent<AudioSource>().enabled = status;
    }

    public void Add()
    {

        if (objectPosition.Equals("none"))
        {
            Debug.Log("ObjectPosition is none");
            audioController.playAudioClip("TableSounds/errorTyping", 0, 0.41f);
            return;
        }
        if (numbersPad.text.Equals("0"))
        {
            Debug.Log("Number is not selected");
            audioController.playAudioClip("TableSounds/errorTyping", 0, 0.41f);

            return;
        }

        turnLeapMotionUiButton(addButton, false);
        audioController.playAudioClip("TableSounds/Add", 0, 0.8f);

        addButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(13, 13, 13, 255);
        addButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = new Color32(218, 218, 218, 255);
        tableController.tableObjectSelectedByCalculator(numbersPad.text, objectPosition);

        Clear();
        DeSelectObjectPosition();

        Debug.Log("num = " + numbersPad + " dir = " + objectPosition);
        Debug.Log("add = " + addButton.interactable + "right = " + rightButton.interactable + "left =" + leftButton.interactable);
        Debug.Log("green");
    }


    public void touchPadButtonClickPlaySound(string audioName)
    {
        //audioController.playAudioClip("TouchPadSound/"+audioName, 0, -1);
        AudioClip audioClip = Resources.Load<AudioClip>("Audio/TouchPadSound/" + audioName);
        this.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

}
