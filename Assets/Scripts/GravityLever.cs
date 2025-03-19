using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLever : Interactable
{

    // Start is called before the first frame update
    void Start()
    {
        carryable = false;
        needsToBeHeld = true;
    }

    public override void Interact()
    {
        //use for now
        GameObject.Find("Player")?.SendMessage("LowerGravity");
        /*
        //set player 2's gravity
        Dictionary<string, string> leverPush = new Dictionary<string, string>();

        //linking the key of name to the value of door name in the dictionary called buttonpush
        //player 2 will just be called as such until otherwise named
        leverPush["Name"] = "player2";
        leverPush["Method"] = "LowerGravity";
        

        NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(leverPush));
        */
    }
    public override void Release()
    {
        base.Release();
        GameObject.Find("Player")?.SendMessage("NormaliseGravity");
        
        /*
        Dictionary<string, string> leverPush = new Dictionary<string, string>();

       
        leverPush["Name"] = "player2";
        leverPush["Method"] = "NormaliseGravity";
        
        NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(leverPush));
        */
    }




    // Update is called once per frame
    void Update()
    {

        

    }
}
