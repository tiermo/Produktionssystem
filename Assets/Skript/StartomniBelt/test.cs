using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    //public Transform tr;
    /*private int raylength = 10;

     void Update()
    {
        Vector3 raydirection = transform.TransformDirection(Vector3.back) * raylength;
        Debug.DrawRay(transform.position, raydirection, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, raydirection, out hit))
        {
            if (hit.collider.name.Contains("Cube"))
            {
                Debug.Log("hit.distance" + hit.distance);
            }
        }
    }*/

    public static bool enterflag = false;
    bool triggerflag = false;

    void Update()
    {
        triggerflag = z_pos.enterflag;
    }

    void OnTriggerEnter(Collider other)
    {
        enterflag = true;
        Start_OmniConveyorControl.isObjectOnConveyor = true;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit");
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
