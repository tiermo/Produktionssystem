using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessenScript_SM : MonoBehaviour {
    private int raylength = 10;
    private bool isObjectDetected = false;       // Object Detected status flag
    private float totalheight;   //distance between sensor and conveyor belt
    private float Height;  //the height of workpiece
    private string high;   
    private bool EnableModul = false;  //if enable this modul
    private float DefaultHeight = 7.94f;
    
    void Update ()
    {
        Vector3 raydirection = transform.TransformDirection(Vector3.back) * raylength;
        Debug.DrawRay(transform.position, raydirection, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, raydirection, out hit))
        {
            if (hit.collider.name.Contains("conveyorBelt"))
            {
                totalheight = hit.distance;
                isObjectDetected = false;
            }

            if (hit.collider.name.Contains("Cube"))
            {
                isObjectDetected = true;
                Height = totalheight - hit.distance;
                Height = Mathf.Round(Height * 10f) / 10f; //Keep 2 decimal places
                if (Height < 0)
                {
                    Height = DefaultHeight - hit.distance;
                }
                high = Height.ToString("0.0");
            }

            if (EnableModul && isObjectDetected)
            {
                GetComponent<tcpServer_Messen_SM> ().HeightMessen (high);  // send acknowledgment
                EnableModul = false;
            }
        }
        
	}

    public void setDistanceSensorActive()
    {
        EnableModul = true;
    }
}
