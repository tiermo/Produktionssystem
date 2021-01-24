using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BohrenFraesenSkript : MonoBehaviour
{

    private Rigidbody arm;                                              //rigidbody attached to the milling head/arm
    private bool machineOn = false;                                     //state of the milling machine   
    AudioSource audio;                                                  //milling audio

    private Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);          //movement vector of the milling head initialised to zero
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       //zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.03f, 0.0f);    //down movement vector
    private Vector3 upMovement;                                         //up movement vector
    private Vector3 leftMovement = new Vector3(0.03f, 0.0f, 0f);
    private Vector3 rightMovement;

    private bool lowerLimitReached = false;
    private bool lowerLimitFlag = false;                                //flag to control sending back acknowledgement on reaching lower limit
    private bool upperLimitFlag = false;                                //flag to control sending back acknowledgement on reaching upper limit
    private bool middlePositionRtL = false;
    private bool middlePositionLtR = false;
    private bool rightPosition = false;
    private bool leftPosition = false;
    private bool triggerEnterFlag=false;
    private bool ScaleAsk = false;
    
    private Vector3 sollLeftPos;
    private Vector3 sollRightPos;
    private Vector3 sollUpPos;
    private Vector3 sollDownPos;
    private Vector3 sollMiddlePos;
    private Vector3 initalMiddlePos;
    private Vector3 initalUpPos;

    private bool Entered = false;
    private float xScale;
    private bool ChangeScale = false;
    private float distance;


    void Start()
    {                                                     //called only at the beginning
        arm = GetComponent<Rigidbody>();
        arm.freezeRotation = true;                                      //avoid rotation of the milling head
        audio = GetComponent<AudioSource>();
        audio.Stop();
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));
        rightMovement = Vector3.Scale(leftMovement, new Vector3(-1.0f, 0.0f, 0.0f));
        
        sollLeftPos.x = arm.position.x;
        sollUpPos.y = arm.position.y;
        sollDownPos.y = arm.position.y;
        sollRightPos.x = arm.position.x;
        initalUpPos.y = arm.position.y;
        initalMiddlePos.x = arm.position.x;
    }

    void Update()
    {                                                    //called once in every frame
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));
        rightMovement = Vector3.Scale(leftMovement, new Vector3(-1.0f, 0.0f, 0.0f));
        
        if (triggerEnterFlag && lowerLimitReached) 
        {
            Debug.Log("Extreme lowerLimitReached. Didn't detected workpiece.");
            GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("wrong");
            triggerEnterFlag = false;
        }
            
        if (triggerEnterFlag && Entered)
        {
            GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
            triggerEnterFlag = false;
        }

        if (ScaleAsk)
        {
            GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage(xScale.ToString("0.0"));
            ScaleAsk = false;
        }

        float leftDifference = arm.position.x - sollLeftPos.x;
        float rightDifference = arm.position.x - sollRightPos.x;
        float upDifference = arm.position.y - sollUpPos.y;
        float downDifference = arm.position.y - sollDownPos.y;
        float middleDifference = arm.position.x - sollMiddlePos.x;
        
        if (leftDifference > -0.05f)
        {
            if (leftPosition)
            {
                GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                leftPosition = false;
            }
        }
        if (leftDifference > 0.05f)
        {
            if (leftPosition)
            {
                Debug.Log("Needed Left Position reached. Machine stopped movement");
                stopHorizontalMovement();
            }
        }
        
        if (rightDifference < 0.05f)
        {
            if (rightPosition)
            {
                ChangeScale = true;
                GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                rightPosition = false;
            }
        }
        if (rightDifference < -0.05f)
        {
            if (rightPosition)
            {
                Debug.Log("Needed Right Position reached. Machine stopped movement");
                stopHorizontalMovement();
            }
        }

        if (arm.position.x > -9.7f && leftPosition)
        {
            GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("wrong");
            Debug.Log("Extreme leftLimitReached. Machine stopped movement");
            leftPosition = false;
        }

        if (arm.position.x < -16.7f && rightPosition)
        {
            //GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("wrong");
            Debug.Log("Extreme rightimitReached. Machine stopped movement");
            //rightPosition = false;
        }

        if (upDifference >-0.05f)
        {
            if (upperLimitFlag)
            {
                GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                upperLimitFlag = false;
            }
        }

        if (arm.position.y < 3.7f)
        {
            lowerLimitReached = true;
        }
        else
        {
            lowerLimitReached = false;
        }

        if (!lowerLimitReached)
        {
            if (downDifference < 0.05f)
            {
                if (lowerLimitFlag)
                {
                    GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                    lowerLimitFlag = false;
                }
            }
        }

        if (lowerLimitFlag && lowerLimitReached)
        {
            Debug.Log("Extreme lowerLimitReached. Can not do milling process for this workpiece");
            GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("wrong");
            lowerLimitFlag = false;
        }
    

        //Debug.Log("arm x " + arm.position.x + " soll x" + sollMiddlePos.x);
        //Debug.Log("middel dif "+middleDifference);
        
        if (middleDifference > -0.05f)
        {
            if (middlePositionRtL)
            {
                sollLeftPos.x = arm.position.x;
                sollUpPos.y = arm.position.y;
                sollDownPos.y = arm.position.y;
                sollRightPos.x = arm.position.x;
                GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                middlePositionRtL = false;
            }
        }

        if (middleDifference < 0.05f)
        {
            if (middlePositionLtR)
            {
                sollLeftPos.x = arm.position.x;
                sollUpPos.y = arm.position.y;
                sollDownPos.y = arm.position.y;
                sollRightPos.x = arm.position.x;
                GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("reached");
                middlePositionLtR = false;
            }
        }

        if (middleDifference == 0f )
        {
            if (middlePositionLtR || middlePositionRtL)
            {
                Debug.Log("Middle Position reached. Machine stopped movement");
                stopHorizontalMovement();
                middlePositionLtR = false;
                middlePositionRtL = false;
            }
        }
        arm.position += movement;
    }

    public void callLimitSensorUp()
    {                                    //activate upper limit switch
        upperLimitFlag = true;
    }

    public void callLimitSensorDown()
    {                                  //activate lower limit switch
        lowerLimitFlag = true;
    }

    public void callLeftPosSensor()
    {                                  //activate lower limit switch
        leftPosition = true;
    }

    public void callRightPosSensor()
    {                                  //activate lower limit switch
        rightPosition = true;
    }

    public void callMiddleLtRSensor()
    {                                  //activate lower limit switch
        middlePositionLtR = true;
    }


    public void callMiddleRtLSensor()
    {                                  //activate lower limit switch
        middlePositionRtL = true;
    }

    public void callTrigger()
    {                                    //activate upper limit switch
        triggerEnterFlag = true;
    }

    public void callScale()
    {                                    //activate upper limit switch
        ScaleAsk= true;
    }

    public void turnOn()
    {                                               //function to turn drilling head
        audio.Play();
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void turnOff()
    {
        audio.Stop();
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public bool getMachineStatus()
    {
        return machineOn;
    }

    public void moveLeft(float dis)
    {                                            //function to move milling head to left postion
        movement = leftMovement;
        arm.position += movement;
        machineOn = true;
        sollLeftPos.x = arm.position.x + dis;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void moveRight(float dis)
    {                                           //function to move milling head to right postion                  
        movement = rightMovement;
        arm.position += movement;
        machineOn = true;
        sollRightPos.x = arm.position.x - dis;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void moveMiddle()
    {                                          //function to move milling head to middle postion
        if (arm.position.x > initalMiddlePos.x)
        {
            movement = rightMovement;
        }
        else
        {
            movement = leftMovement;
        }
        arm.position += movement;
        machineOn = true;
        sollMiddlePos.x = initalMiddlePos.x;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void moveUp(int d)
    {                                              //function to move milling head up
        movement = upMovement;
        machineOn = true;
        if (d == 10)
        {
            sollUpPos.y = initalUpPos.y;
        }
        else
        {
            sollUpPos.y = arm.position.y + d * 0.1f;
        }
        arm.position += movement;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void moveDown(int d)
    {                                             //function to move milling head down
        movement = downMovement;
        machineOn = true;
        if (d == 0f)
        {
            sollDownPos.y = 0;
        }
        else
        {
            sollDownPos.y = arm.position.y - 0.1f * d;
        }
        arm.position += movement;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void stopMovement()
    {                                         //function to stop milling head
        movement = zeroMovement;
        arm.velocity = zeroMovement;
        machineOn = false;
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("finished");
    }

    public void stopHorizontalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(0.0f, 1.0f, 1.0f));
        arm.velocity = zeroMovement;
    }

    public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f));
        arm.velocity = zeroMovement;
    }

    public void SpeedSelect(string s)
    {
        switch (s)
        {
            case "low":
                downMovement = new Vector3(0.0f, -0.01f, 0.0f);
                leftMovement = new Vector3(0.02f, 0.0f, 0.0f);
                break;
            case "norm":
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                leftMovement = new Vector3(0.04f, 0.0f, 0.0f);
                break;
            case "fast":
                downMovement = new Vector3(0.0f, -0.06f, 0.0f);
                leftMovement = new Vector3(0.07f, 0.0f, 0.0f);
                break;
            default:
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                leftMovement = new Vector3(0.04f, 0.0f, 0.0f);
                break;
        }
        GetComponent<tcpServer_Bohren_Fraesen>().sendBackMessage("selected");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Cube"))
        {
            Debug.Log("enter");
            Entered = true;
            xScale=other.gameObject.transform.localScale.x;        
        }  
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Cube"))
        {
            if (ChangeScale)
            {
                Transform g;
                g = other.gameObject.transform;
                Vector3 pos = g.localScale;
                pos.y -= 0.2f;
                g.localScale = pos;
                ChangeScale = false;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Entered = false;
    }
}