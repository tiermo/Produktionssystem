using UnityEngine;
using System.Collections;

public class StanzenSkript : MonoBehaviour
{
    private Vector3 initialPosition;                                    //initial position of the drilling head
    private Transform tr;                                               //x,y,z location of the drilling head
    private Rigidbody arm;                                              //rigidbody attached to the drilling head/arm
    private Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);          //movement vector of the drilling head initialised to zero
    private bool machineOn = false;                                     //state of the drilling machine   
    private Vector3 zeroMovement = new Vector3(0.0f, 0.0f, 0.0f);       //zero movement vector
    private Vector3 downMovement = new Vector3(0.0f, -0.03f, 0.0f);    //down movement vector
    private Vector3 upMovement;                                         //up movement vector
    private bool upperLimitReached = false;                             //upper limit reached indicator
    private bool lowerLimitFlag = false;                                //flag to control sending back acknowledgement on reaching lower limit
    private bool upperLimitFlag = false;                                //flag to control sending back acknowledgement on reaching upper limit
    private bool lowerLimitReached = false;                             //lower limit reached indicator
    public float speed;
    
    void Start()
    {                                                     //called only at the beginning
        tr = GetComponent<Transform>();
        initialPosition = tr.position;
        arm = GetComponent<Rigidbody>();
        arm.freezeRotation = true;                                      //avoid rotation of the drilling head
    }

    void Update()
    {                                                    //called once in every frame
        float armVerticalPosition = initialPosition.y - arm.position.y;
        Debug.Log("initialpositiony"+initialPosition.y+"armVerticalPosition" + armVerticalPosition);
        upMovement = Vector3.Scale(downMovement, new Vector3(0.0f, -1.0f, 0.0f));

        if (armVerticalPosition > 4.5f)
        {                               //check if lower limit is reached
            lowerLimitReached = true;
            if (lowerLimitFlag)
            {
                GetComponent<tcpServer_Stanzen>().LimitSwitchesReached("limitD");   //send acknowledgement from tcpDrilling class
                lowerLimitFlag = false;
            }
            Debug.Log("lowerLimitReached");
        }

        if (armVerticalPosition > 5f)
        {
            Debug.Log("Extreme lowerLimitReached. Machine stopped movement");
            stopVerticalMovement();                                      //stop movement on reaching extreme limit
        }

        if (armVerticalPosition < -0.1f)
        {                              //check if upper limit is reached
            upperLimitReached = true;
            if (upperLimitFlag)
            {
                GetComponent<tcpServer_Stanzen>().LimitSwitchesReached("limitU");
                upperLimitFlag = false;
            }
            Debug.Log("upperLimitReached");
        }

        if (armVerticalPosition < -1f)
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

    public void callLimitSensorDown()
    {                                  //activate lower limit switch
        lowerLimitFlag = true;
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
        GetComponent<tcpServer_Stanzen>().sendBackMessage("finished");
    }

    public void moveDown()
    {                                            //function to move drilling head down
        movement = downMovement;
        machineOn = true;
        upperLimitReached = false;
        arm.position += movement;
        GetComponent<tcpServer_Stanzen>().sendBackMessage("finished");
    }

    public void stopMovement()
    {                                            //function to stop drilling head
        movement = zeroMovement;
        arm.velocity = zeroMovement;
        machineOn = false;
        GetComponent<tcpServer_Stanzen>().sendBackMessage("finished");
    }

    public void stopVerticalMovement()
    {
        movement = Vector3.Scale(movement, new Vector3(1.0f, 0.0f, 1.0f)); //multiply by vector (1,0,1) to stop vertical movement
        arm.velocity = zeroMovement;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name.Contains("Cube"))
        {
            Transform g;
            g = other.gameObject.transform;
            Vector3 pos = g.localScale;
            pos.y -= 0.05f * speed;
            g.localScale = pos;
        }
    }

    public void SpeedSelect(string s)
    {
        switch (s)
        {
            case "low":
                downMovement = new Vector3(0.0f, -0.01f, 0.0f);
                speed = 0.1f;
                break;
            case "norm":
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                speed = 0.2f;
                break;
            case "fast":
                downMovement = new Vector3(0.0f, -0.06f, 0.0f);
                speed = 0.3f;
                break;
            default:
                downMovement = new Vector3(0.0f, -0.03f, 0.0f);
                speed = 0.2f;
                break;
        }
        GetComponent<tcpServer_Stanzen>().sendBackMessage("selected");
    }
}
