using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffectNonPlayer : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator animator;
    private void Start()
    {
        // If the components are attached to the game object, get (set) them
        if(GetComponent<Rigidbody>() != null) rigidBody = GetComponent<Rigidbody>();
        if(GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // If bool frezeTime in players TimeManager script is true 
        if (TimeManager.freezeTime)
        {
            // Set the rigid body to isKinematic: "Forces, collisions or joints will not affect the rigidbody anymore" to stop time on the object. From https://docs.unity3d.com/ScriptReference/Rigidbody-isKinematic.html
            rigidBody.isKinematic = true;
            // If an animator componentr is attached to the game object, disable it to freeze the animation
            if(animator != null) animator.enabled = false;
        }
        // If bool freezeTime is false
        else
        {
            // set kinematic to false to make it affected by forces, collissions
            rigidBody.isKinematic = false;
            // If an animator componentr is attached to the game object, enable it to start the animation again
            if (animator != null) animator.enabled = true;
        }
        // If object has animator, set animation speed 
        if (animator != null)
        {
            // If boostTime is true
            if (TimeManager.boostTime)
            {
                // Set speed to double
                // animator.speed = 1;
            }
            // If boostTime is false
            else
            {
                // Set speed to normal p[layback speed (1)
                // animator.speed = 1;
            }
        }
    }
}
