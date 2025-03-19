using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScipt : MonoBehaviour
{
    public GameObject joinButton;
    public GameObject showConnection;
    public void PlayGame()
    {
        if (GameInfo.Instance.IsHost())
        {
            if(GameInfo.Instance.SavedLevel() != null)
            {
                SceneManager.LoadScene(GameInfo.Instance.SavedLevel());
            }
            else
            {
                SceneManager.LoadScene("level1");
            }
        }
    }
    public void SendInvite()
    {
        NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents("Invite"));
    }
    public void RecieveInvite()
    {
        joinButton.SetActive(true);  
    }
    public void Join()
    {
        NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents("Join"));
        GameInfo.Instance.SetHost(false);
        GameInfo.Instance.SetConnected(true);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (GameInfo.Instance.IsConnected())
        {
            showConnection.SetActive(true);
        }
    }

}
