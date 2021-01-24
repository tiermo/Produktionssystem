using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class z_pos : MonoBehaviour {

    public static bool enterflag=false;
    bool triggerflag = false;

    void Update()
    {
        triggerflag = test.enterflag;
    }
    
    void OnTriggerEnter(Collider other) {
        enterflag = true;
        Start_OmniConveyorControl.isObjectOnConveyor = true;
    }

    void OnTriggerExit(Collider other)
    {
        enterflag = false;
        if (!triggerflag)
        {
            Start_OmniConveyorControl.isObjectOnConveyor = false;
        }
    }

    public bool sentFlag()
    {
        return enterflag;
    }
}
