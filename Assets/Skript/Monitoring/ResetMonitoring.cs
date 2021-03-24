using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMonitoring : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetTheMonitoring()
    {
        GameObject.Find("MonitoringText").GetComponent<DisplayMonitoring>().resetMonitoring();
    }
}
