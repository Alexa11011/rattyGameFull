using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersoncam : MonoBehaviour
{
    

    //camera sensativity
    public float sensitivity;

    //How far away the player can interact with buttons
    public float ButtonPressDistance;

    //public string code;
    public float rotator;

    //if we have a reference to an item we are holding it will be in here
    //holding now knows all the options on the interactable class
    private Interactable holding;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void LookUpAndDown()
    {
        //apply the value of mouse speed
        float RotateVertical = Input.GetAxis("Mouse Y") * sensitivity;
        //float RotateVertical = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        //stop the mouse from going over a limit
        rotator -= RotateVertical;
        rotator = Mathf.Clamp(rotator, -90f, 90f);  
    }

    private void FixedUpdate()
    {
        //rotate the screen
        transform.localRotation = Quaternion.Euler(rotator, 0f, 0f);
        //looking was moved to update to fix the stuttering
    }
    private void Update()
    {
        
        if(holding != null)
        {
            if (holding.NeedsToBeHeld())
            {
                //moving interact to a once only thing
              //  holding.Interact();
                //using bop as hit has been used
                RaycastHit bop;
                if (Physics.Raycast(this.transform.position, this.transform.forward, out bop, ButtonPressDistance))
                {

                    if (bop.collider != holding.GetComponent<Collider>())
                    {
                        Drop();
                        
                    }
                }
                else
                {
                    Drop();
                  
                   // holding.Release();
                }
            }
        }



        LookUpAndDown();
        //am i still looking at the object
        
        // Get the button to run its buttonpressed event after looking at it and pressing e
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (holding == null)
            {
                
                if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, ButtonPressDistance))
                {
                    //if interact with pickup
                    if (hit.collider.gameObject.GetComponent<Interactable>() != null)
                    {
                        print("isinteractable");
                        //run the hold command give it the interactable class
                        Hold(hit.collider.gameObject.GetComponent<Interactable>());
                        Interact(hit.collider.gameObject.GetComponent<Interactable>());
                    }
                }
            }
            else
            {
                Drop();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            //if there is an item we are using
            if(holding != null)
            {
                //if the item cannot be carried
                if (holding.IsntCarryable())
                {
                    Drop();
                }
                
            }
        }
    }
    //for activating an object
    private void Interact(Interactable interactable)
    {
        if (interactable.IsInteractable())
        {
            interactable.Interact();
        }
    }
  
    private void Hold(Interactable interactable)
    {
        if (interactable.IsInteractable())
        {
            holding = interactable;
            holding.HeldBy(gameObject, transform.GetChild(0),transform);
        }
    }

    private void Drop()
    {
        if (holding != null)
        {
            holding.Release();
            holding = null;
        }
    }


    
    // Update is called once per frame
    void LateUpdate()
    {
   
    }
}
