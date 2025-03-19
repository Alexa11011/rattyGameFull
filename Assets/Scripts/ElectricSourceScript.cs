using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSourceScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
      

    }

    void SwitchElectric()
    {
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
            else 
            {
                child.gameObject.SetActive(true);
            }
            //get the pieces and theres spawns


        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
