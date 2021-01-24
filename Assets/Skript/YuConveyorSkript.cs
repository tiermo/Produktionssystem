using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*public class YuConveyorSkript : MonoBehaviour {

    public bool ConveryorOn;   //turn conveyor on or off
    public float ConveyorDriveSpeed;   //a gived speed that wanted how fast the conveyor runs. it needs 3 levels: low, normal and high. Now will change it manual. after need add new Function to offer this 3 services.

    public float ConveyorSpeed; //the real conveyor speed with direction. turn right, is positiv. turn left, is negativ.
    private float previousConveyorSpeed;                    // store previous conveyor speed
    private Vector3 conveyorDirectionVector;                // vector to indicate direction of travel
    private Vector3 conveyorVelocityVector;                 // vector to indicate the conveyor velocity

    //public float ConveyorSpeedWithDirection;  
    public float offset;                                    // offset for conveyor belt texture

    private List<Rigidbody> listOfRigidbodiesOnConveyor;    // the list of objects on the conveyor belt.
    Rigidbody r;
    private bool isObjectOnConveyor = false;                // flag to indicated if object is present on conveyor


    // Use this for initialization
	void Start () {	
        listOfRigidbodiesOnConveyor=new List<Rigidbody>();
        previousConveyorSpeed = ConveyorSpeed;
        conveyorDirectionVector = transform.rotation * Vector3.down;

	}
	
	// Update is called once per frame
	void Update () {

        if (ConveryorOn){
            ConveyorSpeed = ConveyorDriveSpeed;
        }else{
            ConveyorSpeed = 0f;
        }

        //Shift the belt texture to show that belt is moving
        offset = ConveyorSpeed * Time.time;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, offset));

        //adjust the speed of the objects on the conveyor
        if (ConveyorSpeed != previousConveyorSpeed)
        {
            if (listOfRigidbodiesOnConveyor.Count > 0)
            {
                // Remove the velocity component previously added.
                foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor)
                {
                    rigidbody.velocity -= conveyorVelocityVector;
                }
            }
            //Adjust the velocity component
            conveyorVelocityVector = conveyorDirectionVector * ConveyorSpeed;
            if (listOfRigidbodiesOnConveyor.Count > 0)
            {
                //Add the new velocity component 
                foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor)
                {
                    rigidbody.velocity += conveyorVelocityVector;
                }
                
            }
       previousConveyorSpeed = ConveyorSpeed;
       }    
	}

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        listOfRigidbodiesOnConveyor.Add(rigidbody);
        r = rigidbody;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.6f);
        r.velocity = new Vector3(0f, 0f, 0f);  //set velocity to 0
        r.useGravity = false;
        r.freezeRotation = true;
        r.velocity+= conveyorVelocityVector;
        //agent.enabled = false;
        isObjectOnConveyor = true;
    }

    public void ConveyorOn(){
        ConveryorOn = true;
    }
    
    public void ConveyorOff(){
        ConveryorOn = false;
    }

    public void MoveLeft(string speed){
        ConveyorSpeedSelet(speed);        
        //this.transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
    }


    public void MoveRight(string speed)
    {
        ConveyorSpeedSelet(speed);
        ConveyorDriveSpeed = -ConveyorDriveSpeed;
    }

    void ConveyorSpeedSelet(string speed)
    {
        switch (speed){
            case "low":
                ConveyorDriveSpeed = 0.5f;
                break;
            case "norm":
                ConveyorDriveSpeed = 2f;
                break;
            case "fast":
                ConveyorDriveSpeed = 5f;
                break;
        }
    }



}*/

