using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ErkennenScript : MonoBehaviour
{
    private int raylength = 10;
    private bool isObjectDetected = false;       // Object Detected status flag
    private bool EnableModul = false;  //if enable this modul
    private string materialname;
    private string materialInfo;
    private Color originalColor;
    private Color colorToPaint;

    GameObject cube;

    private ConfigurationHelper configHelper = new ConfigurationHelper();
    private string modulname;
    private ConvertTime timeConverter = new ConvertTime();

    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        Debug.Log(transform.parent.GetComponent<MeshRenderer>().material.name);
        modulname = GameObject.Find("Lackieren").GetComponent<Create_Erkennen>().SendModulName();
    }
    
    void Update()
    {
        Vector3 raydirection = transform.TransformDirection(Vector3.back) * raylength;
        Debug.DrawRay(transform.position, raydirection, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, raydirection, out hit))
        {
            if (hit.collider.name.Contains("Cube"))
            {
                isObjectDetected = true;

                cube = hit.collider.gameObject;
               
                
            }
            else {
                isObjectDetected = false;
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

    public void setSensorActive()
    {
        EnableModul = true;
        
    }

    private void Delay()
    {       
        GetComponent<tcpServer_Erkennen>().sendBackMessage("finished");
        transform.parent.GetComponent<MeshRenderer>().material.color = originalColor;
        cube.GetComponent<MeshRenderer>().material.color = colorToPaint;
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
            colorToPaint = chooseColorToPaint(nameSplit[4]);
            // hier den block in bestimmter farbe lackieren
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname)));
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), configHelper.getServiceName(modulname), configHelper.getDrehzahl(configHelper.getServiceName(modulname)), configHelper.getLength(configHelper.getServiceName(modulname))));

        }
        else
        {
            colorToPaint = chooseColorToPaint(nameSplit[4]);
            ConfigManager.changeActiveModule(nameSplit[1], configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4] );
            message = Convert.ToString(timeConverter.calculateTimeDifference(configHelper.getModuleName(modulname), nameSplit[2], nameSplit[3], nameSplit[4]));

        }
        GetComponent<tcpServer_Erkennen>().sendBackMessage(message);
    }

    private Color chooseColorToPaint(string chosenColor)
    {
        if (chosenColor == "green")
        {
            return Color.green;
        } else if(chosenColor == "yellow"){
            return Color.yellow;
        }
        else if (chosenColor == "red")
        {
            return Color.red;
        }
        else if (chosenColor == "blue")
        {
            return Color.blue;
        }
        else if (chosenColor == "orange")
        {
            return Color.HSVToRGB(34f, 85f, 100f);
        } else
        {
            return Color.HSVToRGB(307f, 80f, 100f);
        }

    }

}
