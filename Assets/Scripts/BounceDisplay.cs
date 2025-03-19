using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceDisplay : MonoBehaviour
{
    public float displaySpeedX = 0.0002f;
    public float displaySpeedY = -0.0002f;
   public Transform monitorPosition;
   public SpriteRenderer monitorSizer;
    float rightBound;float leftBound;float upperBound;float lowerBound;
    float waitTime = 0f;
    void Start()
    { 
        //bounds of sprite boxes are their transforms position plus half their bounding size
        //this one does however assume the sprite has been rotated
        rightBound = monitorPosition.position.z + monitorSizer.bounds.size.z / 2;
        leftBound = monitorPosition.position.z - monitorSizer.bounds.size.z / 2;
        upperBound = monitorPosition.position.y + monitorSizer.bounds.size.y / 2;
        lowerBound = monitorPosition.position.y - monitorSizer.bounds.size.y / 2;

        RANDOMISEDIRECTION();
    }
    // Update is called once per frame
    void Update()
    {
        //find this objects side then check if its past the other objects side and wait a lil to not infi bounche
        //checks both left and right side at same time. same for bottom and top
        waitTime = waitTime - Time.deltaTime;
        if (waitTime<= 0)
        {     
            if (this.transform.position.z + this.GetComponent<SpriteRenderer>().bounds.size.z / 2 > rightBound || this.transform.position.z - this.GetComponent<SpriteRenderer>().bounds.size.z / 2 < leftBound)
            {
                displaySpeedX *= -1;
            }
            else if (this.transform.position.y + this.GetComponent<SpriteRenderer>().bounds.size.y / 2 > upperBound || this.transform.position.y - this.GetComponent<SpriteRenderer>().bounds.size.y / 2 < lowerBound)
            {
                displaySpeedY *= -1;    
            }       
            waitTime = 0.2f; 
        }
        //update position
        this.transform.localPosition = new Vector3(this.transform.localPosition.x + displaySpeedX, this.transform.localPosition.y + displaySpeedY, this.transform.localPosition.z);
    }
     void RANDOMISEDIRECTION()
    {
        //start moving in a random direction
        int randomX = Random.Range(0, 2);
        int randomY = Random.Range(0, 2);
        if (randomX == 1)
        {
            displaySpeedX *= -1;

        }
        if (randomY == 1)
        {
            displaySpeedY *= -1;
        }
    }
}
