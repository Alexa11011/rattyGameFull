using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Events : MonoBehaviour
{
    //count down until player fail

    //at about halfway have enemy walk through

    public float timeToCompleteLevel;
    private float worldTimer = 0;
    public GameObject enemy;
    bool calledAlert = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //end the game if time has run out
        worldTimer = worldTimer + Time.deltaTime;
        if(worldTimer >= timeToCompleteLevel)
        {
           // print("gameOver");
        }

        //summon the enemy after about halfway through the time
        if(worldTimer >= timeToCompleteLevel / 2 && calledAlert == false)
        {
            enemy.SendMessage("RecieveAlert", transform.position);
            calledAlert = true;
        }

    }
}
