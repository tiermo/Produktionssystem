using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

//Author: Sagar Nayak
//Date: 26.10.2017

//drillingArmScript contains functions to control the drilling head of the drilling machine.
public class BohrenScript : MonoBehaviour
{

    private Vector3 initialPosition;                                    //initial position of the drilling head
    private Transform tr;                                               //x,y,z location of the drilling head
    private Rigidbody arm;                                              //rigidbody attached to the drilling head/arm
    private Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);           //movement vector of the drilling head initialised to zero
    private bool machineOn = false;                                     //state of the drilling machine   
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       //zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.03f, 0.0f);     //down movement vector
    private Vector3 upMovement;                                         //up movement vector
    private bool upperLimitReached = false;                             //upper limit reached indicator
    private bool upperLimitFlag = false;                                //flag to control sending back acknowledgement on reaching upper limit
    private bool lowerLimitReached = false;
    private bool DepthLimitFlag = false;                                //flag to control sending back acknowledgement on reaching depth limit
    private bool triggerEnterFlag = false;

    private bool Entered = false;
    AudioSource audio;                                                  //drilling audio
    private Vector3 contactedpos;                                       //arm position when drilling head contacted with workpiece
    private Vector3 sollpos;                                            //arm position of "needed depth"
    private int depth;                                                  //needed depth
    private string modulname;

    private ConfigurationHelper configHelper = new ConfigurationHelper();
    private ConvertTime timeConverter = new ConvertTime();

    void Start()
    {      
        //called only at the beginning
        tr = GetComponent<Transform>();
        initialPosition = tr.position;
        arm = GetComponent<Rigidbody>();
        arm.freezeRotation = true;                                      //avoid rotation of the drilling head
        audio = GetComponent<AudioSource>();
        audio.Stop();
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));

        if (ConfigManager.getStartCounter() == 10)
        {
            modulname = "Bohren A 1";
            ConfigManager.setStartCounter();
        }
        else
        {
            modulname = GameObject.Find("Bohren").GetComponent<Create_Bohren>().SendModulName();
        }
        
    }

    void Update()
    {                                                    //called once in every frame
        if (arm.position.y < 3.7f)
        {
            lowerLimitReached = true;
        }
        else
        {
            lowerLimitReached = false;
        }

        if (triggerEnterFlag && lowerLimitReached)
        {
            Debug.Log("Extreme lowerLimitReached. Didn't detected workpiece.");
            GetComponent<tcpServer_Bohren>().sendBackMessage("wrong");
            triggerEnterFlag = false;
        }
        
        if (triggerEnterFlag && Entered)
        {
            GetComponent<tcpServer_Bohren>().sendBackMessage("reached");
            triggerEnterFlag = false;
        }
        
        float armVerticalPosition = initialPosition.y - arm.position.y;
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));
        float difference = arm.position.y - sollpos.y;

        if (!lowerLimitReached)
        {
            if (difference < 0.07f)
            {
                if (DepthLimitFlag)
                {
                    GetComponent<tcpServer_Bohren>().LimitSwitchesReached("Depth");  //send acknowledgement from tcpServer_Bohren class
                    DepthLimitFlag = false;
                }
                sollpos.y = 0f;
            }
        }

        if (lowerLimitReached && DepthLimitFlag)
        {
            //Debug.Log("Extreme lowerLimitReached. Can not do fully drilling process for this workpiece");
            GetComponent<tcpServer_Bohren>().sendBackMessage("wrong");
            DepthLimitFlag = false;
        }

        if (armVerticalPosition < -0.5f)
        {                              //check if upper limit is reached
            upperLimitReached = true;
            if (upperLimitFlag)
            {
                GetComponent<tcpServer_Bohren>().LimitSwitchesReached("limitU");
                upperLimitFlag = false;
            }
            
        }

        if (armVerticalPosition < -1.5f)
        {
            Debug.Log("Extreme upperLimitReached. Machine stopped movement");
            stopVerticalMovement();                                     //stop movement on reaching extreme limit
        }

        arm.position += movement;                                       //change arm position if there is a request
    }

    public void callLimitSensorUp()
    {                                    //activate upper limit switch
        upperLimitFlag = true;
    }

    public void callTrigger()
    {
        triggerEnterFlag = true;
    }

    public void callDepthSensor()
    {                                    //activate upper limit switch
        DepthLimitFlag = true;
    }

    public void turnOn()
    {                                               //function to turn drilling head
        audio.Play();
        GetComponent<tcpServer_Bohren>().sendBackMessage("finished");
    }

    public void turnOff()
    {
        audio.Stop();

        GetComponent<tcpServer_Bohren>().sendBackMessage("finished");
    }

    public bool getMachineStatus()
    {
        return machineOn;
    }

    public bool getUpperLimitStatus()
    {
        return upperLimitReached;
    }

    public bool getLowerLimitStatus()
    {
        return lowerLimitReached;
    }

    public void moveUp()
    {                                              //function to move drilling head up
        movement = upMovement;
        machineOn = true;
        lowerLimitReached = false;
        arm.position += movement;
        GetComponent<tcpServer_Bohren>().sendBackMessage("finished");
    }

    public void moveDown(int d)
    {                                            //function to move drilling head down
        if (d == 0f)
        {
            sollpos.y = 0;
        }
        else {
            sollpos.y = arm.position.y - 0.1f * d;
        }
        movement = downMovement;
        machineOn = true;
        upperLimitReached = false;
        arm.position += movement;
        
        GetComponent<tcpServer_Bohren>().sendBackMessage("finished");
    }

    public void stopMovement()
    {                                            //function to stop drilling head
        movement = zeroMovement;
        arm.velocity = zeroMovement;
        machineOn = false;
        GetComponent<tcpServer_Bohren>().sendBackMessage("finished");
    }

    public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f)); //multiply by vector (1,0,1) to stop vertical movement
        arm.velocity = zeroMovement;
    }

    public void SpeedSelect(string s)
    {
        /*switch (s)
        { 
            case "low":
                downMovement = new Vector3(0.0f, -0.01f,0.0f);
                break;
            case "norm":
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                break;
            case "fast":
                downMovement = new Vector3(0.0f, -0.06f, 0.0f);
                break;
            default:
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                break;
        }*/

        downMovement = new Vector3(0.0f, -0.06f, 0.0f);
        GetComponent<tcpServer_Bohren>().sendBackMessage("selected");   
    }

    

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Cube"))
        {
            Entered = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Cube"))
        {
            Entered = false;
        }
    }

    public void forwardInformation(string data)
    {
        string[] nameSplit;
        nameSplit = data.Split(" "[0]);
        string message;

        if (nameSplit[2] == "servicename")
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname)));
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname))));
        }
        else
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4]);
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4]));
        }
        GetComponent<tcpServer_Bohren>().sendBackMessage(message);
    }

   
}
