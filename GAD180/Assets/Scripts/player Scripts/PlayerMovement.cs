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
    [SerializeField]
    private float rotateAirbornLimits = 30f, groundedRaycastDistance = 0.3f;
    private Vector2 lookAngles, rotationLimit;
    private float movementSpeed;
    [SerializeField]
    private Transform lookRoot, playerRoot, playerFeet;
    private Vector3 currentRotation;
    private bool grounded, rotationLimitSet;
    // Set layermask for the grounded check
    int layerMask = 1 << 9;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        movementSpeed = walkSpeed;
        // Luck cursor and make it invicible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementAllowed)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerFeet.position, transform.TransformDirection(-Vector3.up), out hit, groundedRaycastDistance, layerMask))
            {
                print(hit.distance);

                // Set grounded to true and reset the rotation limits
                grounded = true;
                rotationLimitSet = false;

                // Get inputs from WASD keys
                keyboardInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                // Check if player is running
                if (Input.GetButton("Sprint")) movementSpeed = runSpeed;
                else movementSpeed = walkSpeed;

                // Mulitply the inputs with movementSpeed
                keyboardInput *= movementSpeed;
                // Transform direction so that the inputs are relative to the direction the player i facing
                keyboardInput = transform.TransformDirection(keyboardInput);

                //Adjust speed if player is moving vrtically and horizontally at the same time
                if (keyboardInput.x != 0 && keyboardInput.z != 0)
                {
                    keyboardInput /= Mathf.Sqrt(2);
                }
                if (Input.GetButton("Jump"))
                {
                    keyboardInput.y = jumpSpeed;
                }
            }
            // If the Raycast don't hit any ground, set grounded to false
            else 
            { 
                grounded = false;
                
            }

            
        }

        // Move player
        characterController.Move(keyboardInput * Time.deltaTime);

        //Apply gravity
        keyboardInput.y -= gravitySpeed * Time.deltaTime;

        if (mouseRotationAllowed)
        {
            RotatePlayer();
        }
        // Change between locked and unlocked cursor
        CheckCursorState();
    }
    void CheckCursorState()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
        
    void RotatePlayer()
    {
        transform.Rotate(mouseInput * Time.deltaTime);
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));

        lookAngles.x += mouseInput.x * mouseRotationSpeed * (invertedMouse ? 1f : -1f);
        lookAngles.y += mouseInput.y * mouseRotationSpeed;
        lookAngles.x = Mathf.Clamp(lookAngles.x, lookLimits.x, lookLimits.y);

        // If themplayer is not grounded and a rotationlimit is not set, set limits
        if(!grounded && !rotationLimitSet)
        {
            // Get the current rotation
            currentRotation = transform.rotation.eulerAngles;
            // Determine rotationlimits
            rotationLimit = new Vector2(currentRotation.y - rotateAirbornLimits, currentRotation.y + rotateAirbornLimits);
            // If the rotationlimits are above 180 degrees. Reduce 360 to get the right value (-30 insted of 330, -70 instead of 290 and so on)
            if (rotationLimit.x > 180) rotationLimit.x -= 360;
            if (rotationLimit.y > 180) rotationLimit.y -= 360;
            // Set rotationLimitSet to true so the limits are not set again the next frame
            rotationLimitSet = true;
        }
        // If the player is not grounded
        if(!grounded)
        {
            // Set rotationlimits
            lookAngles.y = Mathf.Clamp(lookAngles.y, rotationLimit.x, rotationLimit.y);
            //print(rotationLimit.x + " " + rotationLimit.y);
        }
        // Set rotation for vertical and horizontal rotation
        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);

    }
}
