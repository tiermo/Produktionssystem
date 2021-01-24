using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger2 : MonoBehaviour {

    public static bool enterflag = false;
    bool triggerflag = false;

    void Update()
    {
        triggerflag = trigger1.enterflag;
    }

    void OnTriggerEnter(Collider other)
    {
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
