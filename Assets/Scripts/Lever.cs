using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public GameObject connectingDevice;
    public bool hot;
   
    // Start is called before the first frame update
    void Start()
    {
        carryable = false;
        needsToBeHeld = true;
    }

    public override void Interact()
    {
        connectingDevice.SendMessage("Pulled", hot);
    }

    public override void Release()
    {
        base.Release();
        connectingDevice.SendMessage("StoppedPulling");
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
}
