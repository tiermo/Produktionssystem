using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{

    private Color originalColor;
    private Color color;
    //public GameObject Obj;
    private bool isDrag=false;
    private Vector3 ObjScreenSpace;
    private Vector3 ObjWorldSpace;
    private Transform trans;
    private Vector3 MouseScreenSpace;
    private Vector3 Offset;
    private Vector3 previousposition;
    private LayerMask mask;

    RaycastHit hit;

    private int rayLength = 10;

    private string Collidername;

    private GameObject g;

    void Start()
    {
        g = transform.Find("conveyorBlend").gameObject;
        if (g != null)
        {
            Debug.Log("ja");
        }else{
            Debug.Log("nein");
        }
        
        originalColor = g.GetComponent<MeshRenderer>().material.color;
        color = g.GetComponent<MeshRenderer>().material.color;
        //trans = GetComponent<Transform>();
        //previousposition = trans.position;
        //Debug.Log("previousposition" + previousposition);
        //Debug.Log("tran.position" + trans.position);

        //mask = 1 << (LayerMask.NameToLayer("Plane"));



    }


    void OnMouseEnter()
    {
        if (!isDrag)
        {
            color = Color.red;
        }
    }

    void OnMouseExit()
    {
        if (!isDrag)
        {
            color = originalColor;
        }
    }

    /*void OnMouseDown()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        ObjScreenSpace = Camera.main.WorldToScreenPoint(trans.position);

        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        //Debug.Log("mouse.position" + MouseScreenSpace);
        Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);
        //Debug.Log("offset" + Offset);
    }

    void OnMouseDrag()
    {
        isDrag = true;
        GetComponent<MeshRenderer>().material.color = Color.red;
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
        if (ObjWorldSpace.y > 1f==false) {
            ObjWorldSpace.y = 1f;
        }
        trans.position = ObjWorldSpace;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);  // project green ray

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
          Vector3 hitpoint = hit.point;
            Debug.Log("collider name" + hit.collider.name + ", hitpoint" + hitpoint.ToString());
              //Debug.Log("collider position" + hit.collider.transform.position);
            Collidername = hit.collider.name;
            if (Collidername.Contains("Omni") == true)
            {
                GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    void OnMouseUp()
    {
        if (GetComponent<MeshRenderer>().material.color == Color.green)
        {
            //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
            Vector3 _offset = new Vector3(0, -0.55f, 0);
            trans.position = hit.collider.transform.position-_offset;
            //Debug.Log("trans.position" + trans.position);
            previousposition = trans.position;
        }
        else {
            trans.position = previousposition;
        }
        GetComponent<MeshRenderer>().material.color = originalColor;
        isDrag = false;
    }*/
    
}



//wenn drag move the model,not follow the mouse
/*void OnMouseDown()
{
    p1 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
}

void OnMouseDrag()
{
    p2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    float dx = p2.x - p1.x;
    float dy = p2.y - p1.y;
    trans.Translate(dy * Vector3.forward * Time.deltaTime);
    /*trans.position.x=dy*Vector3.right*Time.deltaTime;
    trans.position.y = 0.46f;
    trans.position.z=-dx*Vector3.forword*Time.deltaTime;
    trans.Translate(new Vector3(trans.position.x,trans.position.y,tran.position.z));
}*/

//this IEnumerator dont need StartCoroutine
/*IEnumerator OnMouseDown()
    {
        ObjScreenSpace = Camera.main.WorldToScreenPoint(trans.position);
        MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
        var Offset = trans.position - Camera.main.ScreenToWorldPoint(MouseScreenSpace);
        while (Input.GetMouseButton(0))
        {
            MouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjScreenSpace.z);
            var ObjWorldSpace = Camera.main.ScreenToWorldPoint(MouseScreenSpace) + Offset;
            ObjWorldSpace.y = Mathf.Clamp(ObjWorldSpace.y, 0f, 5f);
            trans.position = ObjWorldSpace;
            yield return new WaitForFixedUpdate();
        }
    }*/