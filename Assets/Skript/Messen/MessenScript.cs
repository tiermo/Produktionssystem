using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessenScript : MonoBehaviour {
    private int raylength = 10;
    private bool isObjectDetected = false;       // Object Detected status flag
    private float totalheight;   //distance between sensor and conveyor belt
    private float Height;  //the height of workpiece
    private string high;   
    private bool EnableModul = false;  //if enable this modul
    private Color originalColor;

    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
    }
    
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
                high = Height.ToString("0.0");
            }

            if (EnableModul && isObjectDetected)
            {
                Invoke("Delay", 3);
            }
        }

        if (EnableModul)
        {
            transform.parent.GetComponent<MeshRenderer>().material.color = Color.green;
        }
	}

    public void setDistanceSensorActive()
    {
        EnableModul = true;
    }

    private void Delay()
    {
        GetComponent<tcpServer_Messen>().HeightMessen(high);  // send acknowledgment
        transform.parent.GetComponent<MeshRenderer>().material.color = originalColor;
        EnableModul = false;
        CancelInvoke("Delay");
    }
}
