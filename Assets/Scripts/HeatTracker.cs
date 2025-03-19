using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatTracker : MonoBehaviour
{
    //current heat level
    public float maxheat = 100f;
    public float heat;
    float heatmodStart;
    //this is the normal change of heat over time
    float heatMod = 0.002f;
    //this modifies the speed at which it gets hotter
    float heatChange = 1.001f;
    //this is the change of heat the lever gives
    float leverMod = 0.01f;
    bool leverTurnedOn = false;
    bool isLeverHot;

    //has the puzzle started
    public bool isActive;


    // Start is called before the first frame update
    void Start()
    {
        heat = maxheat / 2;
        heatmodStart = heatMod;
    }
    public void StoppedPulling()
    {
        leverTurnedOn = false;
    }

    public void Pulled(bool leverHot)
    {
        
        isLeverHot = leverHot;
        leverTurnedOn = true;
     
    }
    public void PiecePlacedTempChange(float tempChange)
    {
        heat = heat + tempChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (leverTurnedOn)
        {
            heatMod = heatmodStart;
            if (isLeverHot)
            {
                heat = heat + leverMod;
            }
            else
            {
                heat = heat - leverMod;
            }
        }






        if (isActive)
        {

            if (heat > maxheat / 2)
            {
                //if hot get hotter
                heat = heat + heatMod;
                if (heat > maxheat)
                {
                    print("gameover");
                }
            }
            if (heat < maxheat / 2)
            {
                //if cold get colder
                heat = heat - heatMod;
                if (heat < 0)
                {
                    print("gameover");
                }
            }

            if (heat == maxheat / 2)
            { 
                //randomly go up or down
            heat = heat + (heatMod * Random.Range(-1, 2));
                
            }
            //make the heat vary wilder
            heatMod = heatMod * heatChange;
            



        }

    }
}
