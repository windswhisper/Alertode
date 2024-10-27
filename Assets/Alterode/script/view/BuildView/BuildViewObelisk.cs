using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildViewObelisk : BuildView
{
    public GameObject fxOccupying;
    public GameObject fxOccupied;

    protected override void Update()
    {
        if(((BuildObelisk)build).progress==100){
            fxOccupied.SetActive(true);
            fxOccupying.SetActive(false);
        }
        else if(((BuildObelisk)build).isOccupying){
            fxOccupying.SetActive(true);
            fxOccupied.SetActive(false);
        }
        else{
            fxOccupying.SetActive(false);
            fxOccupied.SetActive(false);
        }
    }
}
