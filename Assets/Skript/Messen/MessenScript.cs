using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessenScript : MonoBehaviour {
    private int raylength = 10;
    private bool isObjectDetected = false;       // Object Detected status flag
    private float totalheight;   //distance between sensor and conveyor belt
    private float Height;  //the height of workpiece
    private string high;   
    private bool EnableModul = false;  //if enable this modul
    private Color originalColor;

    private string modulname;
    private ConfigurationHelper configHelper = new ConfigurationHelper();
    private ConvertTime timeConverter = new ConvertTime();

    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        modulname = GameObject.Find("Pruefen").GetComponent<Create_Pruefen>().SendModulName();
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

    public void forwardInformation(string data)
    {
        string[] nameSplit;
        nameSplit = data.Split(" "[0]);
        string message;

        if (nameSplit[2] == "servicename")
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname)));
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname))));

        }
        else
        {
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4] + " " + nameSplit[5]);
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4] + " " + nameSplit[5]));

        }
        GetComponent<tcpServer_Messen>().sendBackMessage(message);

    }
}
