using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameInfo 
{
    private string savedLevel;


    private static GameInfo _instance;
    //whenever this object exists fill the one slot with this script and refer to this instance
    public static GameInfo Instance { get { if(_instance == null) { _instance = new GameInfo(); }return _instance; } }
    private bool isHost = true;
    private bool isConnected = false;
    //when run from somewhere else returns the value of ishost
    public bool IsHost() { return isHost; }
    public bool IsConnected() { return isConnected; }
    public string SavedLevel() { return savedLevel; }

    public void SetConnected(bool isConnected)
    {
        this.isConnected = isConnected;
    }

   
    //call this to save the level
    public void SaveLevel()
    {
        savedLevel = SceneManager.GetActiveScene().name;
    }

    public void SetHost(bool isHost)
    {
        this.isHost = isHost;  
    }

}
