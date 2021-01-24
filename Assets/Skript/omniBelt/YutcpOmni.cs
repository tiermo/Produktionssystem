using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YutcpOmni : MonoBehaviour {

    public string data;

    public string direction;
    public string speed;
    public int spaceposition;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (string.Compare(data, "st") == 0)
        {
            GetComponent<OmniConveyorControl>().setMonitorFlag();
        }
        else
        {
            spaceposition = data.IndexOf(' ');
            direction = data.Substring(0, spaceposition);
            speed = data.Substring(spaceposition + 1);

            if (string.Compare(direction, "left") == 0)
            {
                //transform.Find("z+").gameObject.GetComponent<Test_Omni>().moveLeft();
                GetComponent<OmniConveyorControl>().moveLeft(speed);
                //sendBackMessage(conveyorLeft);
            }
            if (string.Compare(direction, "right") == 0)
            {
                GetComponent<OmniConveyorControl>().moveRight(speed);
                //sendBackMessage(conveyorRight);
            }

            if (string.Compare(direction, "up") == 0)
            {
                GetComponent<OmniConveyorControl>().moveUp(speed);
                //sendBackMessage("finish");
            }
            if (string.Compare(direction, "down") == 0)
            {
                GetComponent<OmniConveyorControl>().moveDown(speed);
                //sendBackMessage(conveyorDown);
            }
        }
	}
}

//transform.Find("z+").gameObject.GetComponent<Test_Omni>().moveUp();

