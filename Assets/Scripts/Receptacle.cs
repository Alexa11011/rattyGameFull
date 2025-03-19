using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : MonoBehaviour
{
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //tell puzzle receptacle script what you hit and what you are
    private void OnCollisionEnter(Collision collision)
    {
        GameObject[] tempStorage = new GameObject[2];
        tempStorage[0] = gameObject;
        tempStorage[1] = collision.gameObject;
        transform.parent.gameObject.BroadcastMessage("TryPlace", tempStorage);
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
