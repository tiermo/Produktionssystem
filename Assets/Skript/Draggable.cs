using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {

    //private GameObject conveyorBlend;

    // Use this for initialization
    private Color originalColor;
    private GameObject conveyorblend;
    private Vector3 ObjScreenSpace;
    private Vector3 ObjWorldSpace;
    private Transform trans;
    private Vector3 MouseScreenSpace;
    private Vector3 Offset;


    void Start()
    {
        conveyorblend = GameObject.Find("sensor");
        if (conveyorblend != null)
        {
            Debug.Log("exist");
        }else
        {
            Debug.Log("dont exist");
                
        }
        originalColor = conveyorblend.GetComponent<MeshRenderer>().material.color;

        trans = conveyorblend.GetComponent<Transform>();
        Debug.Log("tran.position" + trans.position);
    }

    void OnMouseOver()
    {
        Debug.Log("Onmouseover");
        conveyorblend.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void OnMouseEnter()
    {
        conveyorblend.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        conveyorblend.GetComponent<MeshRenderer>().material.color = originalColor;
    }

    void OnMouseDown()
    {
        ObjScreenSpace = Camera.main.WorldToScreenPoint(trans.position);

        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        Debug.Log("mouse.position" + MouseScreenSpace);
        Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);
        Debug.Log("offset" + Offset);
    }

    void OnMouseDrag()
    {
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
        trans.position = ObjWorldSpace;
    }
}
