using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : Interactable
{


    //dictate how the heat will change when this piece is placed in
    public float heatLevel;
    float heatLimit;
    public GameObject connectingMonitor;
    bool isHot;
    

    Rigidbody thisrb;
    public bool inSlot;
  

    void Start()
    {
        //can't be carried
        carryable = true;

        //randomly hot or cold
        isHot = Random.Range(0, 2) == 0 ? true : false;

        heatLimit = connectingMonitor.GetComponent<HeatTracker>().maxheat;
        thisrb = GetComponent<Rigidbody>();

        //the amount the heat can change is always between half of the maximum and half of half of the maximum
        if (isHot)
        {
            heatLevel = Random.Range(heatLimit / 2 / 2, heatLimit / 2);
        }
        if (!isHot)
        {
            heatLevel = Random.Range(-heatLimit / 2 / 2, -heatLimit / 2);
        }

    }

    //overriding the original code on the interactable script
    public override void HeldBy(GameObject gameobject, Transform holder,Transform camera)
    {
        if (!inSlot)
        {
            //do whatever is in the orginal code
            base.HeldBy(gameobject, holder,camera);
            thisrb.freezeRotation = true;
            thisrb.useGravity = false;
        }
    }
    public override void Release()
    {
        if (!inSlot)
        {
            base.Release();
            thisrb.freezeRotation = false;
            thisrb.useGravity = true;
        }
    }
  


    public void PutInPlace(Transform slot)
    {
        
        connectingMonitor.SendMessage("PiecePlacedTempChange", heatLevel);
         transform.position = slot.position;
        holderLocation = slot;
        isInteractable = false;
        inSlot = true;
        base.Release();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (IsBeingHeld())
        {
            
            transform.position = holderLocation.position;
        }
    }


}
