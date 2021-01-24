using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragandDrop : MonoBehaviour{

    private Color originalColor;
    public GameObject Obj;
    private Vector3 ObjScreenSpace;
    private Vector3 ObjWorldSpace;
    private Transform trans;
    private Vector3 MouseScreenSpace;
    private Vector3 Offset;


    void Start() {
        originalColor=Obj.GetComponent<MeshRenderer>().material.color;

        trans=Obj.GetComponent<Transform>();
        Debug.Log("tran.position" + trans.position);
    }

    void OnMouseOver()
    {
        Obj.GetComponent<MeshRenderer>().material.color = Color.red;
    }
 
    void OnMouseEnter()
    {
        Obj.GetComponent<MeshRenderer>().material.color = Color.red;
    }

     void OnMouseExit()
     {
         Obj.GetComponent<MeshRenderer>().material.color = originalColor;
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
         ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace)+Offset;
         trans.position = ObjWorldSpace;
     }

}
