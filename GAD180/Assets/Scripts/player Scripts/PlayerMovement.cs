using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController characterController;
    // SerializeFiled allows a private variable to be displayed in the inspector window. They are still not accessable from other scripts.
    [SerializeField]
    private bool movementAllowed = true, mouseRotationAllowed = true, invertedMouse;

    [SerializeField]
    private float walkSpeed = 3f, runSpeed = 5f, mouseRotationSpeed = 3f, gravitySpeed = 30f, jumpSpeed = 8f;

    private Vector3 keyboardInput;
    private Vector2 mouseInput;
    [SerializeField]
    private Vector2 lookLimits = new Vector2(-70f, 80f);
    private Vector2 lookAngles;
    private float movementSpeed;
    [SerializeField]
    private Transform lookRoot, playerRoot;
    private int notGroundedFrameCount;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        movementSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // If the characterController is grounded, reset the notGroundedFrameCount
        if (characterController.isGrounded) notGroundedFrameCount = 0;
        // If characterController is not grounded, increase notGroundedFrameCount by 1
        else notGroundedFrameCount += 1;

        //print(notGroundedFrameCount);

        // If the boolen movementAllowed is set to true, get move the player
        if (movementAllowed)
        {
            // If the player is on the ground, listen for keyboard movement
            if (notGroundedFrameCount <= 6)
            {
                // Get inputs from WASD keys
                keyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                // Mulitply the inputs with movementSpeed
                keyboardInput *= movementSpeed;
                // Transform direction so that the inputs are relative to the direction the player i facing
                keyboardInput = transform.TransformDirection(keyboardInput);

                //Adjust speed if player is moving vrtically and horizontally at the same time
                if(keyboardInput.x != 0 && keyboardInput.z != 0 )
                {
                    keyboardInput /= Mathf.Sqrt(2);
                }
                if(Input.GetButton("Jump"))
                {
                    keyboardInput.y = jumpSpeed;
                }
            }
            
            // If the character controller is not on the ground
            else
            {
                //
            }

            // Move player
            characterController.Move(keyboardInput * Time.deltaTime);

            //Apply gravity
            keyboardInput.y -= gravitySpeed * Time.deltaTime;

        }
        if(mouseRotationAllowed)
        {
            RotatePlayer();
        }

    }
    void RotatePlayer()
    {
        transform.Rotate(mouseInput * Time.deltaTime);
        mouseInput = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        lookAngles.x += mouseInput.x * mouseRotationSpeed * (invertedMouse ? 1f : -1f);
        lookAngles.y += mouseInput.y * mouseRotationSpeed;
        lookAngles.x = Mathf.Clamp(lookAngles.x, lookLimits.x, lookLimits.y);

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
