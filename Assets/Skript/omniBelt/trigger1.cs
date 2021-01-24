using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger1 : MonoBehaviour {

    public static bool enterflag=false;
    bool triggerflag = false;

    void Update()
    {
        triggerflag = trigger2.enterflag;
    }
    
    void OnTriggerEnter(Collider other) {
        enterflag = true;
        OmniConveyorControl.isObjectOnConveyor = true;
    }

    void OnTriggerExit(Collider other)
    {
        enterflag = false;
        if (!triggerflag)
        {
            OmniConveyorControl.isObjectOnConveyor = false;
        }
    }

    /*public bool sentFlag()
    {
        return enterflag;
    }*/
}
