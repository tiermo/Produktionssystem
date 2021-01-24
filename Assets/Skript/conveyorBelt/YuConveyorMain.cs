using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuConveyorMain : MonoBehaviour {

    public string data;    // the information get from opc ua server

    public string direction;   // substring get from data, represent the move direction
    public string speed;  // substring get from data, represent the move speed
    public int spaceposition;  //space position in the data

    //private string localEulerAngles; //determine the rotation of conveyor belt to select "ConveyorScript"


    //directionformat: forw,backw,off
    //speedforamt: low,norm,fast

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //localEulerAngles = transform.localEulerAngles.ToString();
        //Debug.Log("localEulerAngles " + localEulerAngles);
        if (string.Compare(data,"off")==0){
            /*switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                    GetComponent<ConveyorScript>().ConveyorOff();
                    break;
                case "(0.0, 180.0, 90.0)":
                    GetComponent<ConveyorScript_Vertikal>().ConveyorOff();
                    break;
            }*/
            //Debug.Log("off");
            GetComponent<ConveyorScript>().ConveyorOff();
            
        }
        else{
            spaceposition = data.IndexOf(' ');
            direction = data.Substring(0, spaceposition);
            speed = data.Substring(spaceposition + 1);
        }

        if(string.Compare(direction,"forw")==0){
            /*switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                    GetComponent<ConveyorScript>().ConveyorOn();
                    GetComponent<ConveyorScript>().setConveyorDirectionDownRight(speed);
                    break;
                case "(0.0, 180.0, 90.0)":
                    GetComponent<ConveyorScript_Vertikal>().ConveyorOn();
                    GetComponent<ConveyorScript_Vertikal>().setConveyorDirectionDownRight(speed);
                    break;
            }  */
            GetComponent<ConveyorScript>().ConveyorOn();
            GetComponent<ConveyorScript>().setConveyorDirectionDownRight(speed);
        }
        if(string.Compare(direction,"backw")==0)
        {           
            /*switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                     GetComponent<ConveyorScript>().ConveyorOn();
                     GetComponent<ConveyorScript>().setConveyorDirectionUpLeft(speed);
                    break;
                case "(0.0, 180.0, 90.0)":
                     GetComponent<ConveyorScript_Vertikal>().ConveyorOn();
                     GetComponent<ConveyorScript_Vertikal>().setConveyorDirectionUpLeft(speed);
                    break;
            }*/
            GetComponent<ConveyorScript>().ConveyorOn();
            GetComponent<ConveyorScript>().setConveyorDirectionUpLeft(speed);
        }


	}
}
