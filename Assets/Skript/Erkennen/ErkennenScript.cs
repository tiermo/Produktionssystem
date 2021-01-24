using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErkennenScript : MonoBehaviour
{
    private int raylength = 10;
    private bool isObjectDetected = false;       // Object Detected status flag
    private bool EnableModul = false;  //if enable this modul
    private string materialname;
    private string materialInfo;
    private Color originalColor;

    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        Debug.Log(transform.parent.GetComponent<MeshRenderer>().material.name);
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
                materialname = hit.collider.gameObject.GetComponent<MeshRenderer>().material.name;
                switch (materialname)
                {
                    case "black (Instance)":
                        materialInfo = "Plastic, color black";
                        break;
                    case "red (Instance)":
                        materialInfo = "Plastic, color red";
                        break;
                    case "Gold (Instance)":
                        materialInfo = "Metall";
                        break;
                }
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
        GetComponent<tcpServer_Erkennen>().sendBackMessage(materialInfo);
        transform.parent.GetComponent<MeshRenderer>().material.color = originalColor;
        EnableModul = false;
        CancelInvoke("Delay");
    }
}
