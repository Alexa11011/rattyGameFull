using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    //This holds all the modes an enemy can have and makes a reference to the current mode
    enum Mode { Searching, Alerted, Hunting};
    Mode EnemyMode;

    AudioSource soundThatPlaysWhenEnemyHasLostYou;

    //Start of ray
    public Vector3 enemyHead;
    //end of ray
    public Vector3 playerHead;
    //refrence to play location
    public GameObject player;
    public NavMeshAgent navigater;
    //a number to count time with with
    public float timeTillAlerted;
    public float timeTillGiveUp;
    public float followPlayerTime;
    //Speeds while in each mode
    public float searchSpeed;
    public float alertSpeed;
    public float huntSpeed;
    //location of where an alert is
    public Vector3 alertLocation;
    //get the player object and number his layer
    private int playerLayer;
    public GameObject player;
    //how far the enemy can kill your from
    public float catchRange;   
    //hitbox to get near where player was
    public GameObject alertArea;
    //distance beteen player and enemy
    public float distance;
    

    // Start is called before the first frame update
    void Start()
    {
        //set mode to searching and setup random search location
        alertLocation = new Vector3(0, 0, 0);
        EnemyMode = Mode.Searching;
        UpdateMode();

        //get the players game layer for use in ignoring collisions against ray cast on this enemy
        playerLayer = player.layer;

        soundThatPlaysWhenEnemyHasLostYou = GetComponent<AudioSource>();
       
        
        //how long before enemy speeds up

        timeTillAlerted = 5;
    }


    //run updatemode whenever the enemy gains a new mode
    void UpdateMode()
    {
      
        switch (EnemyMode)
        {
            //Set the enemy's speed based on their mode
            case Mode.Searching:
                navigater.speed = searchSpeed;              
                break;            
            case Mode.Alerted:
                navigater.speed = alertSpeed;               
                break;
            case Mode.Hunting:
                navigater.speed = huntSpeed;             
                break;
            default:
                navigater.speed = searchSpeed;
                print("oops i'm not in a mode");
                break;
        }
    }
    public void RecieveAlert(Vector3 alert)
    {
        alertLocation = alert;
    }

       
    // Update is called once per frame
    void Update()
    {
        print(EnemyMode);
        //giving vectors the location
        playerHead = player.transform.position;
        enemyHead = transform.position; 

        //find the distance between and the direction towards
        distance = Vector3.Distance(playerHead, enemyHead);
        Vector3 direction = (playerHead - enemyHead).normalized;         
               
        //move to the most interesting place
        navigater.destination = alertLocation;
        
        //check if you can see player
       // Debug.DrawRay(EnemyHead, direction, Color.black);

        
        RaycastHit hit;
        //can this object see the player
        if (Physics.Raycast(enemyHead, direction,out hit,distance, playerLayer))
        {
            //if it can't 

            //start following the player for a little bit after losing sight
            followPlayerTime = followPlayerTime - Time.deltaTime;
            if(EnemyMode != Mode.Searching & followPlayerTime > 0)
            {
                alertLocation = playerHead;
            }

            //lost player completly
            if(EnemyMode != Mode.Searching & followPlayerTime < 0)
            {
                //set a new search location should be put in here or swap chance to other player
                //sound notification
                soundThatPlaysWhenEnemyHasLostYou.Play(0);
                EnemyMode = Mode.Searching;               
                UpdateMode();
            }

           


        }   
        //if it can see you
        else
        {
            //How long to follow player after losing sight
            followPlayerTime = 5;

            //set the mode to alerted
            if (EnemyMode != Mode.Hunting)
            {
                EnemyMode = Mode.Alerted;
                UpdateMode();
            }
            //can it catch you
            if(distance <= catchRange)
            {
                print("i got you");
            }
            //go to player head
            alertLocation = playerHead;
                            
            //after being allerted to long hunt the player
            timeTillAlerted = timeTillAlerted - Time.deltaTime;
            if(timeTillAlerted <= 0)
            {
                EnemyMode = Mode.Hunting;               
                UpdateMode();
            }
        } 
    }
}
