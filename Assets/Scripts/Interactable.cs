using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject heldBy;
    public Transform holderLocation;
    public Transform cameralocation;
    protected bool isInteractable = true;
    protected bool carryable;
    protected bool needsToBeHeld;
    
    //store the variable of whether your picking up the item


    public virtual void HeldBy(GameObject gameobject,Transform holder,Transform camera)
    {
        heldBy = gameobject;
        holderLocation = holder;
        cameralocation = camera;
       // holding = true;
    }
    public bool IsBeingHeld()
    {
        //return true if heldby has a value
        return heldBy != null;
    }
    public bool IsntCarryable()
    {
        return carryable == false; 
    }
    public bool NeedsToBeHeld()
    {
        //if the object needs to be held return
        return needsToBeHeld != false;
    }

    public virtual void Release()
    {
            heldBy = null;
            holderLocation = null;
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public virtual void Interact()
    {

    }

  


  
}
