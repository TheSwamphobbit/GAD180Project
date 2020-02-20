using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupObjects
{
    PROJECTAL_WEAPON,
    MELEE_WEAPON,
    STAMINA_BOOST,
    HEALTH_BOOST
}
public enum StaticInteractables
{
    DOOR_HANDLE
}
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Transform playerView;
    [SerializeField]
    private float interactionDistance = 3f;
    private PlayerInventory playerInventory;
    private LayerMask layerMaskAll, layerMaskInteractable;



    // Start is called before the first frame update
    void Start()
    {
        // Set the layer for pick up objects to 8
        layerMaskInteractable = 1 << 8;
        layerMaskAll = 1 << 2;
        layerMaskAll = ~layerMaskAll;

        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player presses right mouse button
        if(Input.GetButtonDown("Fire2"))
        {
            // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
            // Test raycast hit against all gameobjects with the layer layerMaskInteractable
            RaycastHit hit;
            if(Physics.Raycast(playerView.position, playerView.TransformDirection(Vector3.forward), out hit, interactionDistance, layerMaskInteractable))
            {
                // If the raycast hit has the tag for pickup objects
                if (hit.transform.tag == "PickUp")
                {
                    print(hit.transform.name);
                    playerInventory.PickUp(hit.transform.gameObject);
                }
                
            }
            else
            {
                playerInventory.Drop();
            }
            
        }
    }
}
