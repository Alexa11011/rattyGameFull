using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPuzzle1 : MonoBehaviour
{
    public List<GameObject> displayableObjects;
    public List<Sprite> availablePictures;
    public int currentObject = 0;
    public GameObject ConnectingPuzzle;
    public AudioSource soundThatPlaysWhenYouFinishthisPuzzle;
    public GameObject connectingDoor;

    // Start is called before the first frame update
    void Start()
    {
        //this fills out all the children this object has
        foreach(Transform child in transform)
        {
            displayableObjects.Add(child.gameObject);
            
        }
        soundThatPlaysWhenYouFinishthisPuzzle = GetComponent<AudioSource>();

    }

    public void HandleMessage(string ButtonName)
    {
        //apply the right sprite for the buttonname
        int ButtonValue = 0;
        int.TryParse(ButtonName.Substring(1), out ButtonValue);
        if (ButtonValue == 1)
        {
           
            //get the first child. set its render to the first picture
            displayableObjects[currentObject].GetComponent<SpriteRenderer>().sprite =availablePictures[0];
            //move to the next available picture space
            currentObject += 1;

            

        }
        if (ButtonValue == 2)
        {
           
            displayableObjects[currentObject].GetComponent<SpriteRenderer>().sprite = availablePictures[1];
            currentObject += 1;
        }
        if (ButtonValue == 3)
        {
            
            displayableObjects[currentObject].GetComponent<SpriteRenderer>().sprite = availablePictures[2];
            currentObject += 1;
        }
        if (ButtonValue == 4)
        {
            
            displayableObjects[currentObject].GetComponent<SpriteRenderer>().sprite = availablePictures[3];
            currentObject += 1;
           
        }
        if(currentObject == 4)
        {
            //do a for 
            //turn on all displays and go backto the begining
            currentObject = 0;            

            for(int i = 0; i < displayableObjects.Count; i++)
            {
                displayableObjects[i].GetComponent<SpriteRenderer>().enabled = true;
            }

        }
        if(currentObject == 1)
        {
            for(int i = 0; i < displayableObjects.Count; i++)
            {
                displayableObjects[i].GetComponent<SpriteRenderer>().enabled = false;
                displayableObjects[i].SendMessage("RANDOMISEDIRECTION");
            }
        }

        //if its all green play sound and lock puzzle
        if(displayableObjects[0].GetComponent<SpriteRenderer>().color == Color.green && displayableObjects[1].GetComponent<SpriteRenderer>().color == Color.green && displayableObjects[2].GetComponent<SpriteRenderer>().color == Color.green && displayableObjects[3].GetComponent<SpriteRenderer>().color == Color.green)
        {
            //turn off the connecting puzzle and play a sound to notify the player
            ConnectingPuzzle.SetActive(false);
            //raise the door 2 meters
            connectingDoor.GetComponent<Rigidbody>().MovePosition(new Vector3(connectingDoor.GetComponent<Transform>().position.x, connectingDoor.GetComponent<Transform>().position.y + 2f, connectingDoor.GetComponent<Transform>().position.z));
           // soundThatPlaysWhenYouFinishthisPuzzle.Play(0);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
