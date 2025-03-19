using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class NetworkScript : MonoBehaviour
{
    private Thread listener;
    private NetworkHandler.ThreadParams threadParams;

    public void ProcessMessage(NetworkHandler.MessageContents messageContents)
    {
        throw new System.NotImplementedException();
    }

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
                else if (message.GetMessageType() == NetworkHandler.MessageContents.MessageType.MULTI)
                {
                    string objectName = message.GetValue("Name");
                    string methodName = message.GetValue("Method");
                    string value = message.GetValue("Value");
                    if (objectName != string.Empty && methodName != string.Empty && value != string.Empty)
                    {
                        // might want to put error checking rather than just ignoring calls to non-existent objects
                        GameObject.Find(objectName)?.SendMessage(methodName, value);
                    }
                    else if(objectName != string.Empty && methodName != string.Empty)
                    {
                        GameObject.Find(objectName)?.SendMessage(methodName);
                    }
                    
                }
                
                else
                {
                    Debug.Log(message.Message());
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        threadParams.Terminate();
    }

}
