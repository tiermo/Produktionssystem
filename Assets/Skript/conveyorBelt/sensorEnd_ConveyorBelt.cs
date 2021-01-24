using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yu Xiang
//Date: 06.11.2018

//sensor contains functions to control the sensors.
public class sensorEnd_ConveyorBelt : MonoBehaviour
{
    public int rayLength = 10;                  // to project green ray
    private bool isObjectDetected = false;       // Object Detected status flag
    private bool monitorFlag = false;            // flag to enable proximity sensor

    void Start()
    {
        //tr = GetComponent<Transform>();	
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayLength;  // direction of ray
        Debug.DrawRay(transform.position, forward, Color.green);  // project green ray

        if (Physics.Raycast(transform.position, forward, 10))   // if object collides with green ray
        {
            isObjectDetected = true;
        }
        else
        {
            isObjectDetected = false;
        }

        if (monitorFlag && isObjectDetected)
        {   // monitorFlag is true when proximity sensor is enabled
            GetComponent<tcpSensorEnd_ConveyorBelt>().onObjectDetection();  // send acknowledgment
            monitorFlag = false;
        }
    }

    public bool getObjectDetectedStatus()
    {
        return isObjectDetected;
    }

    public void setMonitorFlag()
    {                   // enable proximity sensor
        monitorFlag = true;
    }

    public void deactiveMonitorFlag()
    {                   // enable proximity sensor
        monitorFlag = false;
    }
}
