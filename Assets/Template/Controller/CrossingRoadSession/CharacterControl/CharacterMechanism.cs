using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMechanism : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public GameClient clientReceiverCommand;
    public Vector3 currentCharacterPosition;
    public bool playerCurrentState = false;  //false means standing and true means walking for the player
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody controllerRB;

    void Start()
    {
        currentCharacterPosition = this.transform.position;

        controllerRB = GetComponent<Rigidbody>();

    }
    void Update()
    {
         if (clientReceiverCommand.result == "go")
        {

            controllerRB.velocity = Vector3.right * speed;
            playerCurrentState = true;

        }
        else
        if (clientReceiverCommand.result == "stop")
        {
          controllerRB.velocity = Vector3.zero;
          playerCurrentState = false;

        }
    }

    public void SetCurrentPositionForX_Axis(float newPositionForX)
    {
        currentCharacterPosition.x = newPositionForX;
    }
    public void ResetPositionToStartPoint()
    {
        this.transform.position = currentCharacterPosition;
    }
}
