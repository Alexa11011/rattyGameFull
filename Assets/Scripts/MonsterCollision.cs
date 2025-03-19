using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterCollision : MonoBehaviour
{
    //size should be between 1.5 and 3
    public float size;
    

    // Start is called before the first frame update
    void Start()
    {
        //pick a number between these
        size = Random.Range(1.5f, 3.0f);

        
        //turn off the picture
        gameObject.GetComponent<Renderer>().enabled = false;

        //change z and x size
        this.transform.localScale = new Vector3(size,1,size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
