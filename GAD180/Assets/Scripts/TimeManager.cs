using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float slowDownFactor = 0.4f, speedUpFactor = 1.5f, magnitudeFreezeLimit = 4f;
    private CharacterController characterController;
    public static bool freezeTime, boostTime;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        print("Boost Time: " + TimeManager.boostTime + ". Freeze Time: " + TimeManager.freezeTime);
    }

    public void SlowMotionEffect()
    {
        float magnitude = characterController.velocity.magnitude;
        // If player is standing still
        if (magnitude == 0)
        {
            //Normal time
            Time.timeScale = 1.5f;
            boostTime = true;
            freezeTime = false;

        }
        // If player is moving but benethe the time freeze limit
        else if(magnitude > 0 && magnitude <  magnitudeFreezeLimit)
        {
            // Time speed depends on the magnitude percentage of the magnitudeFreezeLimit 
            //Time.timeScale = magnitude / magnitudeFreezeLimit;

            Time.timeScale = slowDownFactor;
            boostTime = false;
            freezeTime = false;

        }
        // If player is moving equal or faster than the time freeze limit
        else if(magnitude >= magnitudeFreezeLimit)
        {
            Time.timeScale = 1f;
            boostTime = false;
            freezeTime = true;
        }

        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
