using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YutcpSensor : MonoBehaviour {

    public string data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (string.Compare(data, "st") == 0)
        {
            GetComponent<sensor>().setMonitorFlag();
        }
		
	}
}
