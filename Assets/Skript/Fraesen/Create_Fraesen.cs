﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Create_Fraesen : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject modul; //clone the instance of modul
    private GameObject son;   //son object is arm of milling machine
    private Color originalcolor;  //save the original color of the gameobject
    private Color originalcolorSon;

    private string localEulerAngles; //in order to change the rotation of modul

    private LayerMask mask; //Calculate the number of "Plane" layermask
    RaycastHit hit;

    private string Collidername; //the name of colider
    private string Modulname;

    private int num = 0; //the number of modul

    private Dropdown dropdown;
    private string configurationName;

    //private ConfigManager ConfigManager = new ConfigManager(); // so the Config can be updated

    public void OnBeginDrag(PointerEventData data)
    {
        dropdown = GetComponent<Dropdown>();
        if (dropdown.value == 0)
        {
            configurationName = "A";
            modul = Instantiate(Resources.Load("Modul_FraesenA")) as GameObject;  //clone Prefab from Folder "Resources"
        }
        else if (dropdown.value == 1)
        {
            configurationName = "B";
            modul = Instantiate(Resources.Load("Modul_FraesenB")) as GameObject;  //clone Prefab from Folder "Resources"
        }
        else
        {
            configurationName = "C";
            modul = Instantiate(Resources.Load("Modul_FraesenC")) as GameObject;  //clone Prefab from Folder "Resources"
        }

        son = modul.transform.Find("Arm").gameObject;
        originalcolor = modul.GetComponent<MeshRenderer>().material.color;
        originalcolorSon = son.GetComponent<MeshRenderer>().material.color;
        modul.GetComponent<MeshRenderer>().material.color = Color.yellow;
        son.GetComponent<MeshRenderer>().material.color = Color.yellow;

        num++;
        modul.name = "Fraesen " + configurationName + " " + num.ToString();
        Modulname = modul.name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        

        //drag the gameobject in order to move with mouse
        if (modul != null)
        {
            //Clone GameObject move with mouse
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(50, 0, 30));
            if (pos.y > 7.72f == false)
            {
                pos.y = 8f;
            }
            modul.transform.position = pos;
        }
        modul.GetComponent<MeshRenderer>().material.color = Color.yellow;
        son.GetComponent<MeshRenderer>().material.color = Color.yellow;

        //place the gameobject
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Collidername = hit.collider.name;
            switch (localEulerAngles)
            {
                case "(270.0, 0.0, 0.0)": //should put on the side of horizontal conveyor
                    if (int.Parse(Collidername.Substring(6, 1)) % 2 == 0)   //Format is "Modul#2#",get the middle number, it should be even.
                    {
                        modul.GetComponent<MeshRenderer>().material.color = Color.green;
                        son.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        modul.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        son.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    break;
                case "(270.0, 270.0, 0.0)": //should put on the side of vertical conveyor
                    if (int.Parse(Collidername.Substring(6, 1)) % 2 != 0)   //Format is "Modul#1/3/5#",get the middle number, it should be odd number.
                    {
                        modul.GetComponent<MeshRenderer>().material.color = Color.green;
                        son.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        modul.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        son.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    break;
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (modul.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            switch (localEulerAngles)
            {
                case "(270.0, 0.0, 0.0)":
                    Vector3 _offset_row = new Vector3(10.9f, -7.72f, 1.56f);
                    modul.transform.position = hit.collider.transform.position - _offset_row;
                    break;
                case "(270.0, 270.0, 0.0)":
                    Vector3 _offset_column = new Vector3(-1.4f, -7.72f, 10.71f);
                    modul.transform.position = hit.collider.transform.position - _offset_column;
                    break;
            }
            modul.GetComponent<MeshRenderer>().material.color = originalcolor;
            son.GetComponent<MeshRenderer>().material.color = originalcolorSon;
            hit.collider.GetComponent<BoxCollider>().enabled = false;

            ConfigManager.changeConfig("PM", Collidername, getModulName(configurationName), true); // Update the current Config

            modul.AddComponent<Drag_Fraesen>();
            modul.GetComponent<ConstructorClient_Fraesen>().enabled = true;

            modul = null;
        }
        else
        {
            Destroy(modul);
            num--;
        }
    }

    void Start()
    {
        mask = 1 << (LayerMask.NameToLayer("Modul"));
    }

    void Update()
    {

        if (modul != null)
        {
            localEulerAngles = modul.transform.localEulerAngles.ToString();
            if (Input.GetKeyDown(KeyCode.W))
            {
                switch (localEulerAngles)
                {
                    case "(270.0, 0.0, 0.0)":
                        modul.transform.rotation = Quaternion.Euler(new Vector3(270, 270, 0));
                        break;
                    case "(270.0, 270.0, 0.0)":
                        modul.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
                        break;
                    default:
                        modul.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
                        break;
                }
            }
        }
    }

    public string SendColliderName()
    {
        return Collidername;
    }

    public string SendModulName()
    {
        return Modulname;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modulName"></param>
    /// <returns></returns>
    private ProductionModule getModulName(string modulName)
    {
        if (modulName == "A")
        {
            return ProductionModule.ModulFraesenA;
        }
        else if (modulName == "B")
        {
            return ProductionModule.ModulFraesenB;
        }
        else 
        {
            return ProductionModule.ModulFraesenC;
        }
        
    }
}
