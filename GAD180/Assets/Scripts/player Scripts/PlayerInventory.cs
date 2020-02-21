using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    public GameObject activeObject;
    [SerializeField]
    private Transform playerHands;
    [SerializeField]
    private Transform worldObjects;
    private Rigidbody activeRigidBody;
    [SerializeField]
    private Transform lookRoot;
    private Vector3 throwDirection;
    private float throwAngleMultiplier;
    [SerializeField]
    private float angleMultiplier = 0.15f, forceMultiplier = 12f;
    
    [SerializeField]
    private float moveObjectIntoPositionSpeed = 5f, rotateObjectIntoPositionSpeed = 360f, autoSnapDistance = 0.05f;

    private void Update()
    {
        GetIntoPosition();
        
    }

    public void PickUp(GameObject pickUpObject)
    {
            // Drop the object
            Drop();

            // Set the picked up object to be the active object
            activeObject = pickUpObject;

            //print(activeObject.transform.eulerAngles);


            activeRigidBody = activeObject.transform.GetComponent<Rigidbody>();
            // Deactivate gravity on the game object the player picked up
            activeRigidBody.useGravity = false;
            // Freeze the rotation and position of the game object's rigid body
            activeRigidBody.constraints = RigidbodyConstraints.FreezeAll;


            // Set playerHands as the new parent for the picked up game object
            activeObject.transform.SetParent(playerHands);

            // Change layer of the picked up object so it wont interfere with the raycast
            activeObject.layer = 2;

       
            //instantiate the object "in the players hands"
            //Instantiate(pickUpObject.associatedGameObject, playerHands.position, Quaternion.Euler(transform.eulerAngles), playerHands);

    }
    public void Drop()
    {
        // If an active object already exists, drop the old one
        if (activeObject != null)
        {
            // Set object layer to 8 so we can interact with it after dropping it
            activeObject.layer = 8;

            // Set parent to be worldObjects
            activeObject.transform.SetParent(worldObjects);

            // Set a small rotation on the object so it wont "stand up" when hitting the ground
            Vector3 randomRotation = new Vector3(Random.Range(0f, 5f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            activeObject.transform.rotation = transform.rotation * Quaternion.Euler(randomRotation);

            activeRigidBody = activeObject.transform.GetComponent<Rigidbody>();
            // Activate gravity on the game object that is dropped
            activeRigidBody.useGravity = true;

            // Free all rotations and positions on the game object's rigid body
            activeRigidBody.constraints = RigidbodyConstraints.None;

            // remove the object from the variable activObject
            activeObject = null;
            activeRigidBody = null;
        }
    }
    public void Throw()
    {
        // If an active object already exists, throw the old one
        if (activeObject != null)
        {
            // Set object layer to 8 so we can interact with it after dropping it
            activeObject.layer = 8;

            // Set parent to be worldObjects
            activeObject.transform.SetParent(worldObjects);

            activeRigidBody = activeObject.transform.GetComponent<Rigidbody>();
            // Activate gravity on the game object that is dropped
            activeRigidBody.useGravity = true;


            // Free all rotations and positions on the game object's rigid body
            activeRigidBody.constraints = RigidbodyConstraints.None;

            // Thrust the object forward
            throwDirection = Vector3.forward;
            // Transform the forward direction to make the object travel in the direction the player is facing
            throwDirection = transform.TransformDirection(throwDirection);


            //Set the force in up direction based on the angle of the lookRoot (camera x rotation)

            // Set the throwAngleMultiplier to be the same as the angle of the lookRoot x rotation (player camera)
            throwAngleMultiplier = lookRoot.transform.eulerAngles.x;
            // If throwanlge is greater than 180 degrees, reduce 360 to get minus values. this will for example mean the value is -30 instead of 330. This is needed to know if the player is throwing the object upwards or downwards.
            if (throwAngleMultiplier > 180) throwAngleMultiplier -= 360;
            // Multiplie the value with a negative value to throw the object in the right direction
            throwAngleMultiplier *= -angleMultiplier;

            // Set rotaion to the thrown object
            // Create a random roptation in the x axis 
            Vector3 randomRotation = new Vector3(Random.Range(30f, 60f), 0, 0);
            // Apply the rotation to the rigid body
            activeRigidBody.AddRelativeTorque(randomRotation, ForceMode.Impulse);

            // Add force to make the object shoot away
            activeRigidBody.AddForce( throwDirection * forceMultiplier, ForceMode.Impulse);
            activeRigidBody.AddForce(Vector3.up * throwAngleMultiplier , ForceMode.Impulse);
            // remove the object from the variable activObject
            activeObject = null;
            activeRigidBody = null;
        }
    }
    private void GetIntoPosition()
    {   
        if(activeObject != null)
        {
            // If the active game object is not in the hand of the player
            if (Vector3.Distance(activeObject.transform.position , playerHands.position) > autoSnapDistance)
            {
                Vector3 moveDir = (playerHands.position - activeObject.transform.position).normalized;
                activeObject.transform.position += moveDir * moveObjectIntoPositionSpeed * Time.deltaTime;
            }
            // If the object is very close but still not in the hand of the player, move it to the hand
            else if (Vector3.Distance(activeObject.transform.position, playerHands.position) > 0 && Vector3.Distance(activeObject.transform.position, playerHands.position) <= autoSnapDistance)
            {
                activeObject.transform.position = playerHands.position;
            }
            // If the active object is not rotated in its forward direction, rotate it 
            if(activeObject.transform.eulerAngles != playerHands.eulerAngles)
            {
                // Vector3 rotateDir = ()
                activeObject.transform.rotation = Quaternion.RotateTowards(activeObject.transform.rotation, playerHands.rotation, rotateObjectIntoPositionSpeed * Time.deltaTime);
            }

        }
        
    }
}
