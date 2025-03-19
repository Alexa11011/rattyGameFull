using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alexascode : MonoBehaviour
{

    float displaySpeedX = 0.0002f;
    float displaySpeedY = -0.0002f;
    public float[] edges;
    public float[] walls;
    public Transform distransform;
    float timer;
    public SpriteRenderer dasprite; 
    public float width;
    public float height;
    public float mWidth;
    public float mHeight;
    public SpriteRenderer theMonitor;
    public Transform theMonitorTrasnform;
    void RANDOMISEDIRECTION()
    {
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
    // Start is called before the first frame update
    void Start()
    {
        width = dasprite.bounds.size.z;
        height = dasprite.bounds.size.y;
        mWidth = theMonitor.bounds.size.z;
        mHeight = theMonitor.bounds.size.y;

        walls = new float[4];
        edges = new float[4];
      
        RANDOMISEDIRECTION();

        for (int i = 0; i < 5; i++)
    {
        if (i == 0) //top
        {
            walls[i] = theMonitorTrasnform.position.y + mHeight / 2;
        }
        if (i == 1) //right
        {
            walls[i] = theMonitorTrasnform.position.z + mWidth / 2;
        }
        if (i == 2) //bottom
        {
            walls[i] = theMonitorTrasnform.position.y - mHeight / 2;
        }
        if (i == 3) //left
        {
            walls[i] = theMonitorTrasnform.position.z - mWidth / 2;
        }
    }
    //    for(in)
    }   

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x + displaySpeedX, this.transform.localPosition.y + displaySpeedY, this.transform.localPosition.z);
        
        for (int i = 0; i < 4; i++)
    {   
        if (i == 0) //top
        {
            edges[i] = this.transform.position.y + height / 2;
        }
        else if (i == 1) //right
        {
            edges[i] = this.transform.position.z + width / 2;
        }
        else if (i == 2) //bottom
        {
            edges[i] = this.transform.position.y - height / 2;
        }
        else if (i == 3) //left
        {
            edges[i] = this.transform.position.z - width / 2;
        }
    }

    for (int i = 0; i < 4; i++)
    {
      if (i < 2)
        {
            if (edges[i] >= walls[i])
            {
                if (i % 2 == 0)
                {
                    Reverse_Direction(false);
                    print("bounce1");
                }
                else
                {
                    Reverse_Direction(true);
                     print("bounce2");
                }
            }
        }
        else
        {
            if (edges[i] <= walls[i])
            {
             
                if (i % 2 == 0)
                {
                    
                    Reverse_Direction(false);
                  //   print("bounce3");
                }
                else
                {
                    Reverse_Direction(true);
                  //   print("bounce4");
                }
            }
        }
    }
    }
    void Reverse_Direction(bool bounce_x)
    {
      float timer = 0;

    if (timer <= 0)
    {
        if (bounce_x)
        {
            
            displaySpeedX *= -1;
        }
        else
        {
            displaySpeedY *= -1;
        }
        timer = 0.2f;
    }  
    }
}









