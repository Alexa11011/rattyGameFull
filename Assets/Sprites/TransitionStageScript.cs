using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionStageScript : MonoBehaviour
{
    public GameObject otherExit;
    public GameObject player;
    private bool isInExit = false;

    // Start is called before the first frame update
    void Start()
    {
        //turn the mesh renderer off
        transform.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if both players are in the exit
        
        
    }
  
    //bellow sends a message to let the other door know when you walk in and makes a note of whether this player is in as well
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject == player)
        {
           
            isInExit = true;

            Dictionary<string, string> tryExit = new Dictionary<string, string>();
            tryExit["Name"] = otherExit.name;
            tryExit["Method"] = "TryExit";
            NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(tryExit));
        }
    }

    public void TryExit()
    {
        if(isInExit == true)
        {
            //if the other player tells you to exit then tell them as well

            Dictionary<string, string> tryExit = new Dictionary<string, string>();
            tryExit["Name"] = otherExit.name;
            tryExit["Method"] = "TryExit";
            NetworkHandler.SendTcpMessage("127.0.0.1", 5556, new NetworkHandler.ThreadParams(), new NetworkHandler.MessageContents(tryExit));


            //code for load next level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //exit to next stage
            print("exited");
        }
    }





    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
           
            isInExit = false;
        }
    }
}
