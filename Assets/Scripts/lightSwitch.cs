using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSwitch : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        carryable = false;
        needsToBeHeld = false;
    }
    public override void Interact()
    {
        //to be made when lights exist
    }

    public override void Release()
    {
        base.Release();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
