using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    //This holds all the modes an enemy can have and makes a reference to the current mode
    enum Mode { Searching, Alerted, Hunting};
    Mode EnemyMode;

    AudioSource SoundThatPlaysWhenEnemyHasLostYou;


    //Start of ray
    public Vector3 EnemyHead;
    //end of ray
    public Vector3 PlayerHead;
    //refrence to play location
    public GameObject Player;
    public NavMeshAgent Navigater;
    //a number to count time with with
    public float TimeTillAlerted;
    public float TimeTillGiveUp;
    public float FollowPlayerTime;
    //Speeds while in each mode
    public float SearchSpeed;
    public float AlertSpeed;
    public float HuntSpeed;
    //location of where an alert is
    public Vector3 AlertLocation;
    //get the player object and number his layer
    private int PlayerLayer;
    public GameObject player;
    //how far the enemy can kill your from
    public float CatchRange;   
    //hitbox to get near where player was
    public GameObject AlertArea;
    //distance beteen player and enemy
    public float distance;
    

    // Start is called before the first frame update
    void Start()
    {
        //set mode to searching and setup random search location
        AlertLocation = new Vector3(0, 0, 0);
        EnemyMode = Mode.Searching;
        UpdateMode();

        //get the players game layer for use in ignoring collisions against ray cast on this enemy
        PlayerLayer = player.layer;

        SoundThatPlaysWhenEnemyHasLostYou = GetComponent<AudioSource>();
       
        
        //how long before enemy speeds up

        TimeTillAlerted = 5;
    }


    //run updatemode whenever the enemy gains a new mode
    void UpdateMode()
    {
      
        switch (EnemyMode)
        {
            //Set the enemy's speed based on their mode
            case Mode.Searching:
                Navigater.speed = SearchSpeed;              
                break;            
            case Mode.Alerted:
                Navigater.speed = AlertSpeed;               
                break;
            case Mode.Hunting:
                Navigater.speed = HuntSpeed;             
                break;
            default:
                Navigater.speed = SearchSpeed;
                print("oops i'm not in a mode");
                break;
        }
    }
    public void RecieveAlert(Vector3 alert)
    {
        AlertLocation = alert;
    }

       
    // Update is called once per frame
    void Update()
    {
        print(EnemyMode);
        //giving vectors the location
        PlayerHead = Player.transform.position;
        EnemyHead = transform.position; 

        //find the distance between and the direction towards
        distance = Vector3.Distance(PlayerHead, EnemyHead);
        Vector3 direction = (PlayerHead - EnemyHead).normalized;         
               
        //move to the most interesting place
        Navigater.destination = AlertLocation;
        
        //check if you can see player
       // Debug.DrawRay(EnemyHead, direction, Color.black);

        
        RaycastHit hit;
        //can this object see the player
        if (Physics.Raycast(EnemyHead, direction,out hit,distance, PlayerLayer))
        {
            //if it can't 

            //start following the player for a little bit after losing sight
            FollowPlayerTime = FollowPlayerTime - Time.deltaTime;
            if(EnemyMode != Mode.Searching & FollowPlayerTime > 0)
            {
                AlertLocation = PlayerHead;
            }

            //lost player completly
            if(EnemyMode != Mode.Searching & FollowPlayerTime < 0)
            {
                //set a new search location should be put in here or swap chance to other player
                //sound notification
                SoundThatPlaysWhenEnemyHasLostYou.Play(0);
                EnemyMode = Mode.Searching;               
                UpdateMode();
            }

           


        }   
        //if it can see you
        else
        {
            //How long to follow player after losing sight
            FollowPlayerTime = 5;

            //set the mode to alerted
            if (EnemyMode != Mode.Hunting)
            {
                EnemyMode = Mode.Alerted;
                UpdateMode();
            }
            //can it catch you
            if(distance <= CatchRange)
            {
                print("i got you");
            }
            //go to player head
            AlertLocation = PlayerHead;
                            
            //after being allerted to long hunt the player
            TimeTillAlerted = TimeTillAlerted - Time.deltaTime;
            if(TimeTillAlerted <= 0)
            {
                EnemyMode = Mode.Hunting;               
                UpdateMode();
            }
        } 
    }
}
