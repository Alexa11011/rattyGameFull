using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript1 : MonoBehaviour
{
    //A list of all the displays that can hold sprites
    public GameObject Display1;
    public GameObject Display2;
    public GameObject Display3;
    public GameObject Display4;

    //worried that everytime i try to add a new display it will have to be added here//
    public GameObject connectingDisplay;

    // Start is called before the first frame update
    private const int NUMBEROFBUTTONS = 4;

 
    //current players inputs into the door code for the first puzzle
    public int Input1 = 0;
    public int Input2 = 0;
    public int Input3 = 0;
    public int Input4 = 0;

    //a reference to what is the next number they are up to putting in
    public int CurrentInput = 1;

    //what are the 4 buttons needed to be pressed
    public int PuzzleCode1;
    public int PuzzleCode2;
    public int PuzzleCode3;
    public int PuzzleCode4;


    void Start()
    {
        PuzzleCode1 = Random.Range(1, NUMBEROFBUTTONS + 1);
        PuzzleCode2 = Random.Range(1, NUMBEROFBUTTONS + 1);
        PuzzleCode3 = Random.Range(1, NUMBEROFBUTTONS + 1);
        PuzzleCode4 = Random.Range(1, NUMBEROFBUTTONS + 1);
    }

    //takes a string called button name from message send
    public void HandleMessage(string ButtonName)
    {
        //button value is 0 by default
        int ButtonValue = 0;
        //parse is to turn string into an int, Substring is to go to the next letter starting from 0
        // "out" buttonvalue applies the number to the int
        //this runs the code and sees if it was unsuccesful
        int.TryParse(ButtonName.Substring(1), out ButtonValue);

        print(ButtonValue);
        if (ButtonName == null || ButtonName.Length == 1 || ButtonValue == 0)
        {
            //if theres no button name this will trigger
            if (ButtonName != null)
            {
                print("unknown button name " + ButtonName);
            }
            else
                print("buttonname is null");
            //return means do no code after it
            return;
        }
        

        switch (CurrentInput)
        {
            //if on the first digit
            case 1:
                //make it the number passed from the button string
                Input1 = ButtonValue;
                break;
            case 2:
                Input2 = ButtonValue;                
                break;
            case 3:
                Input3 = ButtonValue;                
                break;
            case 4:
                Input4 = ButtonValue;
                break;
                            default:
                EnterPuzzle();
                CurrentInput = 0;
                print("errorpuzzlecountlost");
                break;
        }
        //if all the inputs are filled enter puzzle
        if(CurrentInput == NUMBEROFBUTTONS) 
        {
            CurrentInput = 1;
            EnterPuzzle();
        }
        //move to the next input otherwise
        else 
        {
            CurrentInput += 1;            
        }
    }

    void EnterPuzzle()
    {
        //grab the list of objects from the dislay and colour them accordingly




        //if the first input is the same as the correct answer
        if (Input1 == PuzzleCode1)
        {
            //grab the script off of the display then look for the displayable objects then colour it green if right
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[0].GetComponent<SpriteRenderer>().color = Color.green;

            //this bellow was using a refrence of the script but it came back null for the displayable objects
            //connectingDisplay.displayableObjects[0].GetComponent<SpriteRenderer>().color = Color.green;

            print("Good");
        }

        // if the first input is the same as any number
        else if (Input1 == PuzzleCode2 || Input1 == PuzzleCode3 || Input1 == PuzzleCode4)
        {
            print("almost");
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[0].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        { print("No Way");
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[0].GetComponent<SpriteRenderer>().color = Color.red;
        }


        if (Input2 == PuzzleCode2)
        { print("Good"); 
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[1].GetComponent<SpriteRenderer>().color = Color.green; 
        }

        else if (Input2 == PuzzleCode1 || Input2 == PuzzleCode3 || Input2 == PuzzleCode4)
        {
            print("almost");
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[1].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            print("No Way");
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[1].GetComponent<SpriteRenderer>().color = Color.red;
        }


        if (Input3 == PuzzleCode3)
        { print("Good");
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[2].GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (Input3 == PuzzleCode1 || Input3 == PuzzleCode2 || Input3 == PuzzleCode4)
        { print("almost"); connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[2].GetComponent<SpriteRenderer>().color = Color.yellow; 
        }
        else
        { print("No Way"); 
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[2].GetComponent<SpriteRenderer>().color = Color.red; 
        }

        if (Input4 == PuzzleCode4)
        { print("Good"); 
            connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[3].GetComponent<SpriteRenderer>().color = Color.green; 
        }
        else if (Input4 == PuzzleCode1 || Input4 == PuzzleCode3 || Input4 == PuzzleCode2)
        { connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[3].GetComponent<SpriteRenderer>().color = Color.yellow;
        print("almost"); }
        else
        { print("No Way"); connectingDisplay.GetComponent<DisplayPuzzle1>().displayableObjects[3].GetComponent<SpriteRenderer>().color = Color.red; 
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
