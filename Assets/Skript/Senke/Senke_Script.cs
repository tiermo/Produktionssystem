using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Senke_Script : MonoBehaviour {
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
                                                     //drilling audio
    private Vector3 contactedpos;                                       //arm position when drilling head contacted with workpiece
    private Vector3 sollpos;                                            //arm position of "needed depth"
    private int depth;                                                  //needed depth
    private string modulname;

    private ConfigurationHelper configHelper = new ConfigurationHelper();
    private ConvertTime timeConverter = new ConvertTime();

    int i = 0;
    private bool objecthere = false;
    

    private GameObject cube;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        initialPosition = tr.position;
        arm = GetComponent<Rigidbody>();

        upMovement = new Vector3(0.0f, 0.03f, 0.0f);
        //upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));

        if (ConfigManager.getStartCounter() == 9)
        {
            modulname = "Senke 1";
            ConfigManager.setStartCounter();
        }
        else
        {
            modulname = GameObject.Find("Senke").GetComponent<Create_Senke>().SendModulName();
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (triggerEnterFlag && Entered)
        {
            Debug.Log("triggerEnterFlag+Entered");
            GetComponent<tcpServer_Senke>().sendBackMessage("reached");
            triggerEnterFlag = false;
        }
        
        if (Entered == true && objecthere)
        {
            Debug.Log("Entered");
            //movement = zeroMovement;
            Delay();
            objecthere = false;
        }


        float armVerticalPosition = initialPosition.y - arm.position.y;
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));
        float difference = arm.position.y - sollpos.y;

        if (arm.position.y <= (initialPosition.y - 4.0f) && movement == downMovement && triggerEnterFlag)
        {
            Debug.Log("ganz Unten");
            movement = zeroMovement;
            GetComponent<tcpServer_Senke>().sendBackMessage("reached");
        }
        
        if (arm.position.y >= initialPosition.y && movement == upMovement && triggerEnterFlag)
        {
            Debug.Log("GAnz oben");
            movement = zeroMovement;
            GetComponent<tcpServer_Senke>().sendBackMessage("topReached");
            triggerEnterFlag = false;
        }
        

        arm.position += movement;
        
        
        
    }

    public void callTrigger()
    {
        triggerEnterFlag = true;
    }

    public void moveUp()
    {
        Debug.Log("MoveUp");
        //function to move drilling head up
        movement = upMovement;
        lowerLimitReached = false;
        arm.position += movement;
        GetComponent<tcpServer_Senke>().sendBackMessage("finished");
    }

    public void moveDown()
    {                                            //function to move drilling head down
        
        sollpos.y = arm.position.y - 0.1f * 8;
        
        movement = downMovement;
        Debug.Log(movement);
        upperLimitReached = false;
        arm.position += movement;
        GetComponent<tcpServer_Senke>().sendBackMessage("finished");
        Debug.Log("moveDownFinished");
        objecthere = true;

    }

    /*public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f)); //multiply by vector (1,0,1) to stop vertical movement
        arm.velocity = zeroMovement;
    }*/

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Cube"))
        {
            Entered = true;
            cube = other.gameObject;
        }
    }

    private void Delay()
    {
       if (objecthere)
        {
            CancelInvoke("Delay");
            // GetComponent<ConveyorScript>().removeCube(cube);
            // Destroy(cube);
            cube.transform.position = new Vector3(9999f, 9999f, 9999f);
            Entered = false;
            GetComponent<tcpServer_Senke>().sendBackMessage("finished");
            
        }
        
        

    }


    /*public void OnTriggerEnter(Collider other)
    {
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
    }*/
    public void deleteGameobject()
    {

    }

    public void forwardInformation(string data)
    {
        string[] nameSplit;
        nameSplit = data.Split(" "[0]);
        

        if (nameSplit[2] == "servicename")
        {
            // hier den block in bestimmter farbe lackieren
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), "Entnehmen", "0", "0");
            

        }
        else
        {
            // hier den block in bestimmter farbe lackieren
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4]);
           

        }
        GetComponent<tcpServer_Senke>().sendBackMessage("finished");
    }



}