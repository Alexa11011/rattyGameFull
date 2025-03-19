using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwitch : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        carryable = false;
        needsToBeHeld = true;
    }
    public override void Interact()
    {

        GameObject.Find("ElectricSource")?.SendMessage("SwitchElectric");

        /*
       //Turn electricity off
       Dictionary<string, string> leverPush = new Dictionary<string, string>();

       //linking the key of name to the value of door name in the dictionary called buttonpush
       //electricsource is the name of the gameobject
       leverPush["Name"] = "ElectricSource";
       leverPush["Method"] = "SwitchElectric";


       NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(leverPush));
       */
    }

    public override void Release()
    {
        base.Release();
        GameObject.Find("ElectricSource")?.SendMessage("SwitchElectric");
        /*
     
      Dictionary<string, string> leverPush = new Dictionary<string, string>();
      leverPush["Name"] = "ElectricSource";
      leverPush["Method"] = "SwitchElectric";


      NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(leverPush));
      */
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
