using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMechanism : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
	public GameClient clientReceiverCommand;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }
    void Update()
    {
		if(clientReceiverCommand.result == "go")
		{		
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(1, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
		else
		if (clientReceiverCommand.result == "stop")
		{
			moveDirection*=0;
		}
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
		}
    }




}
