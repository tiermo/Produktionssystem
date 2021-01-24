using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Creat_ConveyorBelt : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public GameObject conveyor;
    //private Vector3 mouseworldposition;
    private LayerMask mask;
    private string localEulerAngles;
    private string Collidername;
    RaycastHit hit;

    private Color originalColor;

	public void OnBeginDrag(PointerEventData data) {

        conveyor = Instantiate(conveyor) as GameObject;
        /*originalColor = conveyor.GetComponent<MeshRenderer>().material.color;
        mask = 1 << (LayerMask.NameToLayer("Plane"));*/
		
	}

    public void OnDrag(PointerEventData eventData) {
        
        GetComponent<MeshRenderer>().material.color = Color.red;
        if (conveyor != null)
        {
            //Clone GameObject move with mouse
            Vector3 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(50,0,80));
            if (pos.y > 0.46f == false)
            {
                pos.y = 0.46f;
            }
            conveyor.transform.position=pos;

            //
            /*localEulerAngles = conveyor.transform.localEulerAngles.ToString();
            if (Input.GetKeyDown(KeyCode.W))
            {
                switch (localEulerAngles)
                {
                    case "(0.0, 270.0, 90.0)":
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 90));
                        break;
                    case "(0.0, 180.0, 90.0)":
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                        break;
                    default:
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                        break;
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);  // project green ray

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
            {
                //Vector3 hitpoint = hit.point;
                //Debug.Log("collider name" + hit.collider.name + ", hitpoint" + hitpoint.ToString());
                //Debug.Log("collider position" + hit.collider.transform.position);
                Collidername = hit.collider.name;
                switch (localEulerAngles)
                {
                    case "(0.0, 270.0, 90.0)": //is related to Conveyor1/3/5
                        if (int.Parse(Collidername.Substring(8, 1)) % 2 != 0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.green;
                        }
                        else
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        break;
                    case "(0.0, 180.0, 90.0)": //is related to Conveyor0/2/4/6
                        if (int.Parse(Collidername.Substring(8, 1)) % 2 == 0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.green;
                        }
                        else
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        break;
                }
            }*/
           
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        /*if (conveyor.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)":
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_row = new Vector3(0, -0.46f, 9.75f);
                    conveyor.transform.position = hit.collider.transform.position - _offset_row;
                    //Debug.Log("trans.position" + trans.position);
                    break;
                case "(0.0, 180.0, 90.0)":
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_column = new Vector3(-10, -0.46f, 0);
                    conveyor.transform.position = hit.collider.transform.position - _offset_column;
                    //Debug.Log("trans.position" + trans.position);
                    break;
            }
            conveyor.GetComponent<MeshRenderer>().material.color = originalColor;
        }
        else
        {
            Destroy(conveyor);
        }*/
    }
}
