using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class MenuNetworking : MonoBehaviour
{

    public GameObject theMenu;
    public GameObject showConnection;
    private Thread listener;
    private NetworkHandler.ThreadParams threadParams;



    void Start()
    {
        threadParams = new NetworkHandler.ThreadParams();
        listener = NetworkHandler.StartTcpListening(5555, threadParams);
    }
    void Update()
    {
        while (threadParams.HasMessage())
        {
            NetworkHandler.MessageContents message = threadParams.GetNextMessage();
            if (message.GetMessageType() != NetworkHandler.MessageContents.MessageType.EMPTY)   // not implementing empty messages to avoid garbage collection
            {
                if (message.GetMessageType() == NetworkHandler.MessageContents.MessageType.ERROR)
                {
                    Debug.LogError(message.Message());
                }
                else if (message.GetMessageType() == NetworkHandler.MessageContents.MessageType.SINGLE)
                {

                    //message.message is the message sent
                    string messageRecieved = message.Message();
                    if(messageRecieved == "Join")
                    {
                        GameInfo.Instance.SetHost(true);
                        GameInfo.Instance.SetConnected(true);
                    }
                    else if(messageRecieved == "Invite")
                    {
                        theMenu.SendMessage("RecieveInvite");
                        
                       
                    }
                }

            }
        }
    }
}


