using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleReceptacleScript : MonoBehaviour
{
    public GameObject setOfPieces;
    GameObject reciever;
    GameObject piece;
    public List<GameObject> recievingObjs;
    public List<GameObject> Receptacles;
    
    public List<Vector3> SpawnLocations;
    // Start is called before the first frame update
    void Start()
    {
        //get all of the puzzle pieces
        foreach (Transform child in setOfPieces.transform)
        {
            //get the pieces and theres spawns
            recievingObjs.Add(child.gameObject);
            SpawnLocations.Add(child.gameObject.transform.position);
        }
        foreach (Transform child in transform)
        {
            Receptacles.Add(child.gameObject);
        }


        //fors are used to cycle through lists
        //randomize the spawn locations
        for (int i = 0; i < SpawnLocations.Count; i++)
        {
            //sort and randomise the spawn location positions
            Vector3 temp = SpawnLocations[i];
            int randomIndex = Random.Range(i, SpawnLocations.Count);
            SpawnLocations[i] = SpawnLocations[randomIndex];
            SpawnLocations[randomIndex] = temp;
        }

        //give each retriever a number
        //give each piece a number
        for (int i = 0; i < recievingObjs.Count; i++)
        {
            recievingObjs[i].transform.name = "P" + i;

            //place recieving objects at the random spawn location
            recievingObjs[i].transform.position = SpawnLocations[i];
        }

        for (int i = 0; i < Receptacles.Count; i++)
        {
            Receptacles[i].transform.name = "R" + i;
        }
    }
         public void TryPlace(GameObject[] RthenP)
        {
        //assign them to the first and second out of the array
            reciever = RthenP[0];
            piece = RthenP[1];
            int PieceInt = -1;
            int RecieveInt = -2;
            int.TryParse(reciever.name.Substring(1), out PieceInt);
            int.TryParse(piece.name.Substring(1), out RecieveInt);


            if (PieceInt == RecieveInt)
            {
                print("correct");
            piece.SendMessage("PutInPlace", reciever.transform);
                //PutInPlace(reciever.transform);

                //check if that was the last piece
                for (int i = 0; i < reciever.GetComponentInParent<PuzzleReceptacleScript>().recievingObjs.Count;)
                {
                    if (reciever.GetComponentInParent<PuzzleReceptacleScript>().recievingObjs[i].GetComponent<PuzzlePiece>().inSlot == true)
                    {
                        i++;
                        if (i == reciever.GetComponentInParent<PuzzleReceptacleScript>().recievingObjs.Count)
                        {
                            print("you did it");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                print("incorrect");
            }
        }

        //randomize piece location
        //for()



    

 


 
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
